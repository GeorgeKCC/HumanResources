using ColaboratorContract.Dtos.Response;
using Shared.Generics.Response;

namespace ColaboratorContract.Contracts
{
    public interface IGetByEmailColaborator
    {
        Task<GenericResponse<ColaboratorDto>> GetByEmailAsync(string email);
    }
}
