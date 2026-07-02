using System.ComponentModel;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Infrastructure.Data;

namespace Renee.McpServer.Tools;

[McpServerToolType]
public class AccompanyingFileCreateTools(ReneeDbContext context, IHttpContextAccessor httpContextAccessor)
{
	[McpServerTool]
	[Description("Cree un nouveau dossier d'accompagnement dans la base de donnees a partir d'informations minimales fournies par l'utilisateur. Les informations à renseigner sont : prenom et nom de l'occupant principal, code postal du logement et indication si le dossier est dans le programme Tzee. Le prenom et le nom de l'ensemblier solidaire sont optionnels : si non fournis, l'utilisateur connecte est utilise. L'agent initialise les valeurs par defaut des champs requis, cree les entites associees (equipe d'accompagnement, logement et menage), genere une reference de dossier unique et persiste les donnees. En cas de succes, elle retourne les details du dossier cree en JSON, incluant la reference generee.")]
	[Authorize]
	public async Task<CallToolResult> CreateAccompanyingFile(
		[Description("Prenom de l'occupant principal. L'occupant principal est le resident ou locataire principal du logement.")] string mainOccupantFirstName,
		[Description("Nom de l'occupant principal. L'occupant principal est le resident ou locataire principal du logement.")] string mainOccupantLastName,
		[Description("Code postal du logement.")] string housingPostalCode,
		[Description("Prenom de l'ensemblier solidaire. Si non fourni, l'utilisateur connecte est utilise.")] string solidarBuilderFirstName = "",
		[Description("Nom de l'ensemblier solidaire. Si non fourni, l'utilisateur connecte est utilise.")] string solidarBuilderLastName = "",
		[Description("Indique si le dossier est dans le programme Tzee. Optionnel.")] bool isInTzee = true
	)
	{
		User? solidarBuilder;

		if (string.IsNullOrEmpty(solidarBuilderFirstName) && string.IsNullOrEmpty(solidarBuilderLastName))
		{
			var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!Guid.TryParse(userIdClaim, out var userId))
			{
				return new CallToolResult
				{
					IsError = true,
					Content = [new TextContentBlock { Text = "Impossible de déterminer l'utilisateur connecté. Veuillez fournir le prénom et le nom de l'ensemblier solidaire." }]
				};
			}

			solidarBuilder = await context.Users.FindAsync(userId);
			if (solidarBuilder == null)
			{
				return new CallToolResult
				{
					IsError = true,
					Content = [new TextContentBlock { Text = $"L'utilisateur connecté (id: {userId}) n'a pas été trouvé dans la base de données." }]
				};
			}
		}
		else
		{
			solidarBuilder = await context.Users
				.Where(u => u.FirstName == solidarBuilderFirstName && u.LastName == solidarBuilderLastName)
				.FirstOrDefaultAsync();

			if (solidarBuilder == null)
			{
				return new CallToolResult
				{
					IsError = true,
					Content = [new TextContentBlock { Text = $"Ensemblier solidaire '{solidarBuilderFirstName} {solidarBuilderLastName}' introuvable." }]
				};
			}
		}

		AccompanyingFile accompanyingFile = new()
		{
			AccompanyingFileMilestone = (int)AccompanyingFileStage.Identify,
			AccompanyingFileStatus = (int)AccompanyingFileStatus.InProgress,
			AccompanyingFilePreWorkPlanNavigation = new PreWorkPlan(),
			AccompanyingFilePreFinancingPlanNavigation = new PreFinancingPlan(),
			CreatedBy = solidarBuilder.Id,
			OpeningDate = DateTime.UtcNow,
			ZeroEnergyExclusionTerritoriesProgram = isInTzee,
			IsDeleted = false,
			AccompanyingFileSupportTeamNavigation = new SupportTeam
			{
				MarkerNature = 0,
				SolidarBuilder = solidarBuilder.Id
			},
			AccompanyingFileHousingNavigation = new Housing
			{
				HousingAddressNavigation = new Address
				{
					PostalCode = housingPostalCode,
					City = string.Empty,
					Department = string.Empty,
					Region = string.Empty,
					Street = string.Empty,
					HouseNumber = string.Empty,
					Label = string.Empty
				},
				HousingInitialStateNavigation = new HousingInitialState(),
				HousingAfterWorkStateNavigation = new HousingAfterWorkState()
			},
			AccompanyingFileHouseholdNavigation = new Household
			{
				MainOccupantNavigation = new MainOccupant
				{
					Trigram = GenerateTrigram(mainOccupantFirstName, mainOccupantLastName),
					FirstName = mainOccupantFirstName,
					LastName = mainOccupantLastName
				}
			}
		};

		accompanyingFile.AccompanyingFileReference = string.Concat(
			solidarBuilder.FirstName, " ", solidarBuilder.LastName, "-",
			accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.Trigram.ToUpper(), "-",
			housingPostalCode, "-",
			DateTime.UtcNow.ToString("dd/MM/yyyy")
		);

		context.AccompanyingFiles.Add(accompanyingFile);
		var result = await context.SaveChangesAsync();

		if (result > 0)
		{
			return new CallToolResult
			{
				StructuredContent = JsonSerializer.SerializeToElement(accompanyingFile.AccompanyingFileReference)
			};
		}

		return new CallToolResult
		{
			IsError = true,
			Content = [new TextContentBlock { Text = "Error creating AccompanyingFile." }]
		};

		static string GenerateTrigram(string? firstName, string? lastName)
		{
			string firstNameInitial = !string.IsNullOrEmpty(firstName)
				? firstName[..1].ToUpper()
				: "X";

			string lastNameInitials;

			if (!string.IsNullOrEmpty(lastName))
			{
				var part = lastName
					.Split([' ', '-'], StringSplitOptions.RemoveEmptyEntries)
					.FirstOrDefault();

				lastNameInitials = !string.IsNullOrEmpty(part) && part.Length >= 2
					? part[..2].ToUpper()
					: (!string.IsNullOrEmpty(part) ? part.ToUpper().PadRight(2, 'Y') : "YY");
			}
			else
			{
				lastNameInitials = "YY";
			}
			return firstNameInitial + lastNameInitials;
		}
	}
}
