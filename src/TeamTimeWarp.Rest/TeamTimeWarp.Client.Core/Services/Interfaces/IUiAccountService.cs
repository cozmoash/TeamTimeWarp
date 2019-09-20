namespace TeamTimeWarp.Client.Core.Services
{
    public interface IUiAccountService
    {
        LoginToken CreateAccount(string username, string emailAddress, string password);
    }
}