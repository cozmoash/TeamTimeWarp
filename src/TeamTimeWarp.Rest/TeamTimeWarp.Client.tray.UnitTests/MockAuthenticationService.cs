using System;
using System.ComponentModel;
using TeamTimeWarp.Client.Core.Services.Interfaces;

namespace TeamTimeWarp.Client.tray.UnitTests
{
    public class MockAuthenticationService : IUiAuthenticationService
    {
        public event EventHandler<AsyncCompletedEventArgs> LoginCompleted;

        public bool IsLoggedIn { get; private set; }
        public long AccountId { get; private set; }

        public void RaiseLoginComplete(Exception exception = null)
        {
            IsLoggedIn = true;
            EventHandler<AsyncCompletedEventArgs> handler = LoginCompleted;
            if (handler != null) handler(this, new AsyncCompletedEventArgs(exception, false, null));
        }

        public bool LoginAsyncCalled { get; private set; }
        public void LoginAsync(string emailAddress, string password, bool rememberLogin = false)
        {
            LoginAsyncCalled = true;
        }

        public bool QuickLoginCalled { get; private set; }
        public void QuickLoginAsync(string username)
        {
            QuickLoginCalled = true;
        }

        public void Logout()
        {

        }
    }
}