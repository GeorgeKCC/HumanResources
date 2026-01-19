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
            await hubContext.Clients.Client(GetConnectionId()).SendAsync("StatusUpdateCollaboratorNotification", status);
        }

        public async Task NotificationCompleteUpdateColaborator(int colaboratorId)
        {
            await hubContext.Clients.Client(GetConnectionId()).SendAsync("CompleteUpdateCollaboratorNotification", colaboratorId);
        }

        private string GetConnectionId()
        {
            var userEmail = httpContextAccessor?.HttpContext?.User.GetEmail() ?? string.Empty;
            var conectionId = NotificationHub.GetConnectionIdByEmail(userEmail) ?? string.Empty;
            return conectionId;
        }
    }
}
