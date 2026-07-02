using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum SocioProfessionalCategory
{
	[Description(SocioProfessionalCategoryLabel.Farmer)]
	Farmer,
	[Description(SocioProfessionalCategoryLabel.Artisan)]
	Artisan,
	[Description(SocioProfessionalCategoryLabel.Cadre)]
	Cadre,
	[Description(SocioProfessionalCategoryLabel.Employee)]
	Employee,
	[Description(SocioProfessionalCategoryLabel.SearchingJob)]
	SearchingJob,
	[Description(SocioProfessionalCategoryLabel.Worker)]
	Worker,
	[Description(SocioProfessionalCategoryLabel.IntermediateProfession)]
	IntermediateProfession,
	[Description(SocioProfessionalCategoryLabel.Retired)]
	Retired,
	[Description(SocioProfessionalCategoryLabel.Unemployed)]
	Unemployed
}