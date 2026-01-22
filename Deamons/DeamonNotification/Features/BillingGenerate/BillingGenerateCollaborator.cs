namespace DeamonNotification.Features.BillingGenerate
{
    internal class BillingGenerateCollaborator : IBillingGenerateCollaborator
    {
        public Task Process()
        {
            Console.WriteLine("Sincronizando BillingGenerateCollaborator...");
            return Task.CompletedTask;
        }
    }
}
