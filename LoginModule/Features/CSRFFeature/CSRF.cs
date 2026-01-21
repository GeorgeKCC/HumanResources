using LoginContract.Contract;
using LoginContract.Dtos.Responses;
using Shared.Securities.Contracts;

namespace LoginModule.Features.CSRFFeature
{
    internal class CSRF(IGetAndStoreTokensAntiforgery getAndStoreTokensAntiforgery) : ICSRF
    {
        public CSRFDto GetToken()
        {
            var tokens = getAndStoreTokensAntiforgery.GetToken();
            
            return new CSRFDto(tokens);
        }
    }
}
