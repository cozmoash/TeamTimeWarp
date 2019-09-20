namespace TeamTimeWarp.Domain.Entities.Repositories
{
    public interface IAccountPasswordRepository : IRepository<AccountPassword>
    {
        AccountPassword GetByEmail(string email);
    }
}