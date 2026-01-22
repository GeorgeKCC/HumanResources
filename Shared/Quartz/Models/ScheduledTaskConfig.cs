namespace Shared.Quartz.Models
{
    internal class ScheduledTaskConfig
    {
        public string Name { get; init; } = default!;
        public string Cron { get; init; } = default!;
    }
}
