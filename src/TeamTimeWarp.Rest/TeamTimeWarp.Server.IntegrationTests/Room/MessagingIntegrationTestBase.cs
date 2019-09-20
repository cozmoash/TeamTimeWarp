using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests.Room
{
    public abstract class MessagingIntegrationTestBase
    {
        protected AccountCreationResponse Account1;
        protected AccountCreationResponse Account2;
        protected MessagingTestHelper Messaging1;
        protected MessagingTestHelper Messaging2;

        protected MessagingIntegrationTestBase()
        {
            Messaging1 = new MessagingTestHelper();
            Messaging2 = new MessagingTestHelper();
            Account1 = Messaging1.CreateAccount();
            Account2 = Messaging2.CreateAccount();
        }

        protected class MessagingTestHelper : IntegrationTestBase
        {

        }
    }
}