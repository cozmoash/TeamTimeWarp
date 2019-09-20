using TeamTimeWarp.Rest.Authentication;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Rest.Tests.Controllers.Accounts;
using TeamTimeWarp.Rest.Tests.Controllers.Mocks;

namespace TeamTimeWarp.Rest.UnitTests.Controllers.Accounts
{
    public abstract class AccountTestBase
    {
        protected AccountController AccountController;
        protected MockAccountsRepository AccountRepository;
        protected MockUserStateRepository TimeWarpUserStateRepository;
        protected MockAccountPasswordRepository AccountPasswordRepository;
        protected MockTimeWarpAuthenticationManager AuthenticationManager;

        public AccountTestBase()
        {
            AccountRepository = new MockAccountsRepository();
            AccountPasswordRepository = new MockAccountPasswordRepository();
            TimeWarpUserStateRepository = new MockUserStateRepository();
            AuthenticationManager = new MockTimeWarpAuthenticationManager();

            FakeTimeCalculatorFactory.GetTimeWarpStateCalculator();

            var accountCreator = new AccountCreator(AccountRepository, AccountPasswordRepository,
                                        TimeWarpUserStateRepository, AuthenticationManager);

            AccountController = new AccountController(accountCreator);

        }
    }
}