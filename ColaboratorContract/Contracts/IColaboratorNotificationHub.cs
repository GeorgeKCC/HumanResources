using ColaboratorContract.Dtos.Response;

namespace ColaboratorContract.Contracts
{
    public interface IColaboratorNotificationHub
    {
        Task NotificationCreateColaborator(ColaboratorDto colaboratorDto);

        Task NotificationStatusUpdateColaborator(string status);
    }
}
