namespace TeamTimeWarp.Domain.Entities.Repositories
{
    public interface IAuthenticationSessionRepository : IRepository<AuthenticationSession>
    {
        bool TryGetByToken(string token, out AuthenticationSession result);
    }
}