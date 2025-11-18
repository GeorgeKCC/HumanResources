using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using Microsoft.AspNetCore.SignalR;
using Shared.hub;

namespace ColaboratorModule.Features.NotificationHubFeature
{
    internal class ColaboratorNotificationHub(IHubContext<NotificationHub> hubContext) : IColaboratorNotificationHub
    {
        public async Task NotificationCreateColaborator(ColaboratorDto colaboratorDto)
        {
            await hubContext.Clients.All.SendAsync("CreateCollaboratorNotification", colaboratorDto);
        }
    }
}
