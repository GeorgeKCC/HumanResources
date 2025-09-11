using Ex = System.Exception;

namespace Shared.Exception
{
    public class ExistColaboratorCustomException(string message) : Ex(message)
    {
    }
}
