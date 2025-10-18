namespace Shared.Entities
{
    [Table("Colaborator")]
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
        public byte[] Version { get; set; } = Array.Empty<byte>();
    }
}
