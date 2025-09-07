using Ex = System.Exception;

namespace Shared.Exception
{
    public class NotFoundCustomException(string message) : Ex(message)
    {
    }
}