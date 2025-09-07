namespace ColaboratorContract.Dtos.Request
{
    public class UpdateColaboratorRequest
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string DocumentType { get; set; }
        public required string DocumentNumber { get; set; }
    }
}
