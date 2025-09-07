using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using Shared.Entities;

namespace ColaboratorModule.mappers
{
    internal static class ColaboratorMappers
    {
        internal static Colaborator Map(this CreateColaboratorRequest request)
        {
            return new Colaborator()
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                DocumentNumber = request.DocumentNumber,
                DocumentType = request.DocumentType,
            };
        }

        internal static ColaboratorDto Map(this Colaborator colaborator, int id)
        {
            return new ColaboratorDto()
            {
                Id = id,
                Name = colaborator.Name,
                LastName = colaborator.LastName,
                Email = colaborator.Email,
                DocumentNumber = colaborator.DocumentNumber,
                DocumentType = colaborator.DocumentType,
            };
        }
    }
}
