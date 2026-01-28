namespace DeamonNotification.Features.ProcessGenerate
{
    internal class ProcessGenerateCollaborator : IProcessGenerateCollaborator
    {
        public Task Process()
        {
            Console.WriteLine("Sincronizando ProcessGenerateCollaborator...");
            return Task.CompletedTask;
        }
    }
}
