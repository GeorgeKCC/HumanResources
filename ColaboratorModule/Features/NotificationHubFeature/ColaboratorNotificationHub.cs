using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;
using Shared.hub;

namespace ColaboratorModule.Features.NotificationHubFeature
{
    internal class ColaboratorNotificationHub(IHubContext<NotificationHub> hubContext,
                                              IHttpContextAccessor httpContextAccessor) : IColaboratorNotificationHub
    {
        public async Task NotificationCreateColaborator(ColaboratorDto colaboratorDto)
        {
            await hubContext.Clients.All.SendAsync("CreateCollaboratorNotification", colaboratorDto);
        }

        public async Task NotificationStatusUpdateColaborator(string status)
        {
            var conectionId = NotificationHub.GetConnectionIdByEmail(httpContextAccessor.HttpContext.User.GetEmail());
            await hubContext.Clients.Client(conectionId).SendAsync("StatusUpdateCollaboratorNotification", status);
        }
    }
}
