using NUnit.Framework;
using TeamTimeWarp.Client.Tray.Wpf.ViewModel;

namespace TeamTimeWarp.Client.tray.UnitTests
{
    [TestFixture]
    public class WhenTheLoginButtonIsPressed
    {
        private readonly LoginViewModel _loginViewModel;

        public WhenTheLoginButtonIsPressed()
        {
            var mockAuth = new MockAuthenticationService();

            _loginViewModel = new LoginViewModel(mockAuth) { QuickLoginUsername = "testLogin" };
            _loginViewModel.SignInCommand.Execute(null);
        }

        [Test]
        public void ThenALoadingMessageIsDisplayed()
        {
           Assert.AreEqual("Logging in...", _loginViewModel.LoadingMessage);
        }

        [Test]
        public void ThenTheLoginButtonsAreDisabled()
        {
            Assert.IsFalse(_loginViewModel.InputEnabled);
        }
    }
}