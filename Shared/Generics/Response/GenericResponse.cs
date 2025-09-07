namespace Shared.Generics.Response
{
    public class GenericResponse<T>(string message, T data)
    {
        public string Code { get; } = "200";
        public string Message { get; } = message;
        public bool IsSucces { get; } = true;
        public T Data { get; } = data;
    }
}
