using System;
using System.Net.Http;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Rest.Tests.Controllers.Accounts;
using TeamTimeWarp.Rest.Tests.Controllers.Mocks;
using TeamTimeWarp.UnitTests;

namespace TeamTimeWarp.Rest.Tests.Controllers.Users
{
    public abstract class UserControllerTestBase
    {
        protected UserStateController Controller;
        protected FakeNowProvider FakeNowProvider;
        protected readonly HttpRequestMessage Request;
        protected const long AccountId = 1;

        protected UserControllerTestBase()
        {
            FakeNowProvider = new FakeNowProvider {Now = new DateTime(2000, 12, 12, 12, 12, 0)};
            var usersCache = new MockUserStateRepository();
            var calc = FakeTimeCalculatorFactory.GetTimeWarpStateCalculator();
            var account = new Account(AccountId, TestHelper.NameMock, TestHelper.EmailAddressMock,AccountType.Full );
            var accountRepository = new MockAccountsRepository();
            var authenticationManager = new MockTimeWarpAuthenticationManager();
            var token = authenticationManager.AddUser(new AccountPassword(account,"bean"));

            accountRepository.Add(account);
            
            var timeWarpUser = new UserStateManager(calc, usersCache);

            Request = HttpRequestMock.MockRequest();
            Request.Headers.Add("login-token", token);

            Controller = new UserStateController(timeWarpUser, FakeNowProvider, accountRepository, authenticationManager);
        }
    }
}