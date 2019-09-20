using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using TeamTimeWarp.Rest.Authentication;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Rest.Tests.Controllers.Mocks;
using TeamTimeWarp.UnitTests;

namespace TeamTimeWarp.Rest.UnitTests.Controllers.Accounts
{
    [TestFixture]
    public class WhenANewAccountIsCreated : AccountTestBase
    {
        private readonly HttpRequestMessage _request;
        
        public WhenANewAccountIsCreated()
        {
            _request = HttpRequestMock.MockRequest();
            _request.Headers.Add("password", "beans");

            AccountController.Post(_request, TestHelper.NameMock, TestHelper.EmailAddressMock);
        }

        [Test]
        public void ThenCreatingAnotherAccountWithTheSameEmailAddressResultsInAnError()
        {
            var accountCreator = new AccountCreator(AccountRepository, AccountPasswordRepository,
                                                    TimeWarpUserStateRepository, AuthenticationManager);

            var accountController = new AccountController(accountCreator);

            var exception =
                Assert.Throws<HttpResponseException>(() => accountController.Post(_request, TestHelper.NameMock,
                                                                                  TestHelper.EmailAddressMock));
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [Test]
        public void ThenTheNewAccountShouldBePersisted()
        {
            var stored = AccountRepository.GetAll().Single();

            Assert.AreEqual(TestHelper.NameMock, stored.Name);
            Assert.AreEqual(TestHelper.EmailAddressMock, stored.Email);
        }
    }
}