using DeamonNotification.Features.ProcessGenerate;
using Shared.Quartz.Contract;

namespace DeamonNotification.Workers
{
    internal class WorkerRecurrentProcessGenerate(IProcessGenerateCollaborator processGenerateCollaborator) : IScheduledTask
    {
        public string Name => typeof(WorkerRecurrentProcessGenerate).Name;

        public async Task ExecuteAsync()
        {
            await processGenerateCollaborator.Process();
        }
    }
}
