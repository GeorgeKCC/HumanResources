using DeamonNotification.Features.BillingGenerate;
using Shared.Quartz.Contract;

namespace DeamonNotification.Workers
{
    public class WorkerRecurrentBillingGenerate(IBillingGenerateCollaborator billingGenerateCollaborator) : IScheduledTask
    {
        public string Name => typeof(WorkerRecurrentBillingGenerate).Name;

        public async Task ExecuteAsync()
        {
            await billingGenerateCollaborator.Process();
        }
    }
}
