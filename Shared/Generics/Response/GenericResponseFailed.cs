namespace Shared.Generics.Response
{
    public class GenericResponseFailed<T>(string code, string message, T data)
    {
        public string Code { get; } = code;
        public string Message { get; } = message;
        public bool IsSucces { get; } = false;
        public T Data { get; } = data;
    }
}
