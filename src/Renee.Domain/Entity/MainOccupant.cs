namespace Renee.Domain.Entity;

public class MainOccupant
{
    public Guid Id { get; set; }

    public string Trigram { get; set; } = null!;

    public DateTime? Birthdate { get; set; }

    public int? Age { get; set; }

    public int? SocioProfessionalCategory { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Job { get; set; }

    public int? SocialProtectionFund { get; set; }

    public string? CommentOnSocialProtectionFund { get; set; }

    public int? PensionFund { get; set; }

    public string? CommentOnPensionFund { get; set; }

    public int? AdditionnalFund { get; set; }

    public string? CommentOnAdditionnalFund { get; set; }

    public int? Position { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<Household> Households { get; set; } = new List<Household>();
}