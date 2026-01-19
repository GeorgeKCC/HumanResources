using ColaboratorContract.Dtos.Response;
using Shared.Generics.Response;

namespace ColaboratorContract.Contracts
{
    public interface IGetByIdColaborator
    {
        Task<GenericResponse<ColaboratorDto>> GetByIdAsync(int id);
    }
}
