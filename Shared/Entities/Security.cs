namespace Shared.Entities
{
    [Table("Security")]
    public class Security
    {
        [Key]
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Salt { get; set; }
        public bool Active { get; set; }

        public int ColaboratorId { get; set; }

        [ForeignKey("ColaboratorId")]
        public Colaborator? Colaborator { get; set; }
    }
}
