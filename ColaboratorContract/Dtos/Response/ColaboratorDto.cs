namespace ColaboratorContract.Dtos.Response
{
    public record ColaboratorDto
    (
        int Id,
        string Name,
        string LastName,
        string Email,
        string DocumentType,
        string DocumentNumber,
        bool IsActive
    );
}
