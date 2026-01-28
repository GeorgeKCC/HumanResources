using Quartz;
using Shared.Quartz.Contract;

namespace Shared.Quartz
{
    internal sealed class ScheduledJob(IEnumerable<IScheduledTask> tasks) : IJob
    {
        private readonly IEnumerable<IScheduledTask> _tasks = tasks;

        public async Task Execute(IJobExecutionContext context)
        {
            var taskName = context.MergedJobDataMap.GetString("task");

            var task = _tasks.FirstOrDefault(t => t.Name == taskName);

            if (task is null)
                throw new InvalidOperationException($"Task '{taskName}' not found");

            await task.ExecuteAsync();
        }
    }
}
