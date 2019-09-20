using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Rest.Authentication
{
    public interface ITimeWarpAuthenticationManager
    {
        bool TryAuthenticate(string emailAddress, string password, out  ServiceLoginToken token);
        string AddUser(AccountPassword accountPassword);
        bool TryAuthenticateForWriteOperation(string token, out long accountId);
        bool TryAuthenticateForReadOperation(string token, out long accountId);
        bool Invalidate(string token);

    }
}