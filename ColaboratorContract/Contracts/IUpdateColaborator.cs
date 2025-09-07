using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using Shared.Generics.Response;

namespace ColaboratorContract.Contracts
{
    public interface IUpdateColaborator
    {
        Task<GenericResponse<ColaboratorDto>> UpdateColaboratorAsync(UpdateColaboratorRequest updateColaboratorRequest);
    }
}
