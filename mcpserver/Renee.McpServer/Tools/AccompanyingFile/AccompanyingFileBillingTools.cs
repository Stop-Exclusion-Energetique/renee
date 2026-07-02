using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Renee.Infrastructure.Data;
using Renee.McpServer.Extensions;
using Renee.McpServer.Models;

namespace Renee.McpServer.Tools;

[McpServerToolType]
public class AccompanyingFileBillingTools(ReneeDbContext context, IHttpContextAccessor httpContextAccessor)
{
	private static readonly JsonSerializerOptions jsonSerializerOptions = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		WriteIndented = false
	};

	[McpServerTool]
	[Description("Recupere des informations de facturation detaillees liees a un dossier d'accompagnement depuis la base de donnees au format JSON. L'entree requise est la reference complete du dossier, correspondant au nom ou identifiant unique du dossier (ex. 'Lucas ES_LB-EME-33000-01/07/2024'). Cet outil doit etre utilise systematiquement quand une demande contient le mot-cle 'facturation' avec une reference de dossier. Il retourne les montants, le statut de facturation, les numeros et dates de facture, ainsi que d'autres donnees de suivi utiles.")]
	[Authorize]
	public async Task<CallToolResult> GetAccompanyingFileBillingInfo(
		[Description("AccompanyingFile Reference")] string fileReference
	)
	{
		var fileExists = await context.AccompanyingFiles
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.AnyAsync(af => af.AccompanyingFileReference == fileReference);

		if (!fileExists)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Aucun dossier trouvé avec la référence {fileReference} ou accès non autorisé." }]
			};
		}

		var billingInfo = await context.AccompanyingFiles
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.Where(af => af.AccompanyingFileReference == fileReference)
			.Select(af => new BillingInfoDto
			{
				Reference = af.AccompanyingFileReference,
				Status = af.AccompanyingFileStatus,
				Milestone = af.AccompanyingFileMilestone,
				BilledJalon1 = af.AccompanyingFileBillingLog!.BilledJalon1,
				AmountBilledFirstStage = af.AccompanyingFileBillingLog.AmountBilledFirstStage,
				FundraisingLaunchDateForFirstStage = af.AccompanyingFileBillingLog.FundraisingLauchDateForFirstStage,
				BillingCallNumberFirstStage = af.AccompanyingFileBillingLog.BillingCallNumberFirstStage,
				InvoiceNumberFirstStage = af.AccompanyingFileBillingLog.InvoiceNumberFirstStage,
				BillingDateFirstStage = af.AccompanyingFileBillingLog.BillingDateFirstStage,
				BilledJalon2 = af.AccompanyingFileBillingLog.BilledJalon2,
				AmountBilledSecondStage = af.AccompanyingFileBillingLog.AmountBilledSecondStage,
				FundraisingLaunchDateForSecondStage = af.AccompanyingFileBillingLog.FundraisingLauchDateForSecondStage,
				BillingCallNumberSecondStage = af.AccompanyingFileBillingLog.BillingCallNumberSecondStage,
				InvoiceNumberSecondStage = af.AccompanyingFileBillingLog.InvoiceNumberSecondStage,
				BillingDateSecondStage = af.AccompanyingFileBillingLog.BillingDateSecondStage,
				BilledJalon3 = af.AccompanyingFileBillingLog.BilledJalon3,
				AmountBilledThirdStage = af.AccompanyingFileBillingLog.AmountBilledThirdStage,
				FundraisingLaunchDateForThirdStage = af.AccompanyingFileBillingLog.FundraisingLauchDateForThirdStage,
				BillingCallNumberThirdStage = af.AccompanyingFileBillingLog.BillingCallNumberThirdStage,
				InvoiceNumberThirdStage = af.AccompanyingFileBillingLog.InvoiceNumberThirdStage,
				BillingDateThirdStage = af.AccompanyingFileBillingLog.BillingDateThirdStage,
				LastUpdate = af.AccompanyingFileBillingLog.LastUpdate
			})
			.FirstOrDefaultAsync();

		if (billingInfo == null)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Aucune information de facturation disponible pour le dossier {fileReference}." }]
			};
		}

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(billingInfo, jsonSerializerOptions)
		};
	}
}
