using System;

namespace TeamTimeWarp.Client.Core.Services
{
    public interface IUiAuthenticationService
    {
        event EventHandler<EventArgs> LoggedIn;  
        
        bool IsLoggedIn { get; }
        
        void Login(string username, string password);
        void Logout();
    }
}