namespace Renee.Domain.Entity
{
    public class CguVersion
    {
        public Guid Id { get; set; }
        public string Label { get; set; } = null!;
        public string  Version { get; set; } = null!;
        public  DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
