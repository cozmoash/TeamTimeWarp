using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Service;

namespace TeamTimeWarp.Rest.Tests.Controllers
{
    [TestFixture]
    public class AccountsControllerTest
    {
        [Test]
        public void CreateNewAccountTest()
        {
            var accountsCache = new AccountsCache(Enumerable.Empty<Account>());
            var userStateCache = new UsersCache(Enumerable.Empty<TimeWarpUser>());
            var persistence = new FakeEntityPersistence<Account>();
            var calc = FakeTimeCalculatorFactory.GetTimeWarpStateCalculator();
            AccountController accountController = new AccountController(accountsCache, persistence, userStateCache, calc);

            var result = accountController.Post("new user", "eamil@sdaisd", "newPassword");

            var stored = persistence.SavedItems.Single();

            Assert.AreEqual(0,result);
            Assert.AreEqual("new user",stored.Name);
            Assert.AreEqual("eamil@sdaisd", stored.Email);
        }
    }
}