namespace ColaboratorContract.Dtos.Request
{
    public record CreateColaboratorRequest
    (
        string Name,
        string LastName,
        string Email,
        string DocumentType,
        string DocumentNumber
    );
}
