using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence.Accounts;
using TeamTimeWarp.UnitTests;

namespace TeamTimeWarp.Database.UnitTests
{
    [TestFixture]
    public class AccountsPersistenceTest
    {
        [Test]
        public void CanAddNewAccounts()
        {
            var account1 = TestHelper.AccountMock();
            var account2 = TestHelper.AccountMock();
            var accountPassword1 = new AccountPassword(account1, "hello1");
            var accountPassword2 = new AccountPassword(account2, "hello2");
            
            var accountRepository = new AccountRepository();
            var accountPasswordRepository = new AccountPasswordRepository();

            accountRepository.Add(account1);
            accountRepository.Add(account2);

            accountPasswordRepository.Add(accountPassword1);
            accountPasswordRepository.Add(accountPassword2);

            accountRepository.GetAll();
            var accountPasswords = accountPasswordRepository.GetAll();
            
            Assert.That(account1.Id != 0);
            Assert.That(account2.Id != 0);

            Assert.That(accountPasswords.Count != 0);


        }
    }
}
