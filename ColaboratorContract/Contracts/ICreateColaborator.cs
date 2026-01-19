using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using Shared.Generics.Response;

namespace ColaboratorContract.Contracts
{
    public interface ICreateColaborator
    {
        Task<GenericResponse<ColaboratorDto>> CreateAsync(CreateColaboratorRequest createColaboratorRequest);
    }
}