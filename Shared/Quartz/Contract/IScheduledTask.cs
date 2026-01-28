namespace Shared.Quartz.Contract
{
    public interface IScheduledTask
    {
        string Name { get; }
        Task ExecuteAsync();
    }
}
