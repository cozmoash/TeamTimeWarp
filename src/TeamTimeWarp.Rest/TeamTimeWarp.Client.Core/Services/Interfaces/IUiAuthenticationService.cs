using System;
using System.ComponentModel;

namespace TeamTimeWarp.Client.Core.Services.Interfaces
{
    public interface IUiAuthenticationService
    {
        event EventHandler<AsyncCompletedEventArgs> LoginCompleted;  
        
        bool IsLoggedIn { get; }
        long AccountId { get; }
        
        //void Login(string username, string password);
        void LoginAsync(string emailAddress, string password,bool rememberLogin = false);
        void QuickLoginAsync(string username);

        void Logout();
    }
}