namespace Shared.Securities.Models
{
    public class HashPasswordResponse
    {
        public required string Hash { get; set; }
        public required string Salt { get; set; }
    }
}
