namespace Shared.Entities
{
    [Table("OutboxMessage")]
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }
        public string Type { get; set; } = default!;
        public string Payload { get; set; } = default!;
        public bool Processed { get; set; }
        public DateTime? ProcessedOn { get; set; }
    }
}
