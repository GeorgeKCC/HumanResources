using ManagementContract.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Context.HumanResource_Context;
using Shared.Entities;
using Shared.Exception;

namespace ManagementModule.Features.GetByEmailSecurityFeature
{
    internal class GetByEmailSecurity(DatabaseHumanResourceContext databaseContext) : IGetByEmailSecurity
    {
        public async Task<Security> GetByEmailAsync(string email)
        {
           var security = await databaseContext.Securities.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email)
                          ?? throw new ExistColaboratorCustomException("Not exist colaborator");

           return security;

        }
    }
}
