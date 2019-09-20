using System.Security;

namespace TimeManager.Client.Tray
{
    public interface IUiAccountService
    {
        void CreateAccount(string username, string emailAddress, SecureString password);
    }
}