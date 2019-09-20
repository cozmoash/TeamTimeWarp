using NUnit.Framework;

namespace TeamTimeWarp.Server.IntegrationTests.Accounts
{
    [TestFixture]
    public class WhenAUserLogsInWithTheIncorrectPassword : IntegrationTestBase
    {

        public WhenAUserLogsInWithTheIncorrectPassword()
        {
            CreateAccount();
            Logout();
            Password = "wrongPassword";
            
        }

        [Test]
        public void ThenAUserCannotPerformAnAction()
        {
            Assert.Throws<AssertionException>(Login);
        }
    }
}