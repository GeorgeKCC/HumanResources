namespace ColaboratorContract.Dtos.Request
{
    public record UpdateColaboratorRequest
    (
        string Name,
        string LastName,
        string Email,
        string DocumentType,
        string DocumentNumber
    );
}
