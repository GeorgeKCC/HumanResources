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
            return new ColaboratorDto(id, colaborator.Name,colaborator.LastName, colaborator.Email,
                                      colaborator.DocumentNumber, colaborator.DocumentType, colaborator.Security?.Active ?? false);
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
            return colaborators.Select(x => new ColaboratorDto(x.Id, x.Name, x.LastName, x.Email,
                                                              x.DocumentNumber, x.DocumentType,
                                                              x.Security?.Active ?? false));
        }

        internal static Colaborator Map(this ColaboratorDto colaboratorDto)
        {
            return new Colaborator()
            {
                DocumentNumber = colaboratorDto.DocumentNumber,
                Email = colaboratorDto.Email,
                Id = colaboratorDto.Id,
                Name = colaboratorDto.Name,
                LastName = colaboratorDto.LastName,
                DocumentType = colaboratorDto.DocumentType
            };
        }
    }
}
