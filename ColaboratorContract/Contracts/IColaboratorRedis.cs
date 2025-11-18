namespace ColaboratorContract.Contracts
{
    public interface IColaboratorRedis
    {
        Task RemoveListAll();
        Task RemoveById(int id);
    }
}
