namespace Shared.Entities
{
    [Table("Colaborator")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(DocumentNumber), IsUnique = true)]
    [Index(nameof(DocumentNumber), nameof(Email), IsUnique = true)]
    public class Colaborator
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;

        [Timestamp]
        public byte[] Version { get; set; } = [];

        public Security? Security { get; set; }
    }
}
