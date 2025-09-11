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

        internal static Colaborator Map(this UpdateColaboratorRequest updateColaboratorRequest, Colaborator colaborator)
        {
            colaborator.Email = updateColaboratorRequest.Email;
            colaborator.DocumentNumber = updateColaboratorRequest.DocumentNumber;
            colaborator.DocumentType = updateColaboratorRequest.DocumentType;
            colaborator.LastName = updateColaboratorRequest.LastName;
            colaborator.Name = updateColaboratorRequest.Name;

            return colaborator;
        }

        internal static IEnumerable<ColaboratorDto> Map(this List<Colaborator> colaborators)
        {
            return colaborators.Select(x => new ColaboratorDto()
            {
                Id = x.Id,
                Name = x.Name,
                LastName= x.LastName,
                Email = x.Email,
                DocumentNumber = x.DocumentNumber,
                DocumentType = x.DocumentType
            });
        }
    }
}
