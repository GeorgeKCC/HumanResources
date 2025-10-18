using ManagementContract.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Entities;
using Shared.Exception;

namespace ManagementModule.Features.GetByEmailSecurityFeature
{
    internal class GetByEmailSecurity(DatabaseContext databaseContext) : IGetByEmailSecurity
    {
        public async Task<Security> GetByEmailAsync(string email)
        {
           var security = await databaseContext.Securities.FirstOrDefaultAsync(x => x.Email == email)
                          ?? throw new ExistColaboratorCustomException("Not exist colaborator");

           return security;

        }
    }
}
