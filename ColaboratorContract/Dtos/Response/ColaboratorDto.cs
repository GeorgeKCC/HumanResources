namespace ColaboratorContract.Dtos.Response
{
    public class ColaboratorDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string DocumentType { get; set; }
        public required string DocumentNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
