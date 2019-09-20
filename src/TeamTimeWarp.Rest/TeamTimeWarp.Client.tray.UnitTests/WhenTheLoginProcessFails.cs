using System.Security;
using NUnit.Framework;
using TeamTimeWarp.Client.Tray.Wpf.ViewModel;

namespace TeamTimeWarp.Client.tray.UnitTests
{
    [TestFixture]
    public class WhenTheLoginProcessFails
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly MockAuthenticationService _mockAuth;
        private readonly SecurityException _exception;

        public WhenTheLoginProcessFails()
        {
            _mockAuth = new MockAuthenticationService();

            _loginViewModel = new LoginViewModel(_mockAuth) { QuickLoginUsername = "testLogin" };
            _loginViewModel.SignInCommand.Execute(null);
            _exception = new SecurityException("login failed");

            _mockAuth.RaiseLoginComplete(_exception);
        }

        [Test]
        public void ThenTheLoadingMessageIsSetToSuccess()
        {
            Assert.AreEqual(_exception.Message, _loginViewModel.LoadingMessage);
        }

        [Test]
        public void ThenTheLoginButtonsAreReenabled()
        {
            Assert.IsTrue(_loginViewModel.InputEnabled);
        }
    }
}