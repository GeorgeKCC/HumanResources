using ColaboratorContract.Dtos.Response;
using Shared.Generics.Response;

namespace ColaboratorContract.Contracts
{
    public interface IGetAllColaborator
    {
        Task<GenericResponse<IEnumerable<ColaboratorDto>>> GetAllAsync();
    }
}
