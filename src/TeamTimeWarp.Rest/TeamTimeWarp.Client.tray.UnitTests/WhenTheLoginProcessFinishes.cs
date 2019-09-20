using NUnit.Framework;
using TeamTimeWarp.Client.Tray.Wpf.ViewModel;

namespace TeamTimeWarp.Client.tray.UnitTests
{
    [TestFixture]
    public class WhenTheLoginProcessFinishes
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly MockAuthenticationService _mockAuth;

        public WhenTheLoginProcessFinishes()
        {
            _mockAuth = new MockAuthenticationService();

            _loginViewModel = new LoginViewModel(_mockAuth) { QuickLoginUsername = "testLogin" };
            _loginViewModel.SignInCommand.Execute(null);
            _mockAuth.RaiseLoginComplete();
        }

        [Test]
        public void ThenTheLoadingMessageIsSetToSuccess()
        {
            Assert.AreEqual("success!",_loginViewModel.LoadingMessage);
        }

        [Test]
        public void ThenTheLoginButtonsAreDisabled()
        {
            Assert.IsFalse(_loginViewModel.InputEnabled);
        }
    }
}