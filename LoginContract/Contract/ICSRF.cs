using LoginContract.Dtos.Responses;

namespace LoginContract.Contract
{
    public interface ICSRF
    {
        CSRFDto GetToken();
    }
}
