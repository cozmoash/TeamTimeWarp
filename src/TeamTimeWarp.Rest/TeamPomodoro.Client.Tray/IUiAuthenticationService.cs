using System.Security;

namespace TimeManager.Client.Tray
{
    public interface IUiAuthenticationService
    {
        LoginToken Login(string username, SecureString password);
        void Logout();
    }
}