using System;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence.Accounts;
using TeamTimeWarp.Persistence.UserState;
using TeamTimeWarp.UnitTests;

namespace TeamTimeWarp.Database.UnitTests
{
    [TestFixture]
    public class UserStatePersistenceTest
    {
        [Test]
        public void CanUpdateState()
        {
            var account = TestHelper.AccountMock();
            var state1 = new TimeWarpUserState(account, TimeWarpState.Working, new DateTime(2000, 12, 12, 12, 12, 12),
                                              new TimeSpan(1, 30, 2), 3.0, 4);
            var state2 = new TimeWarpUserState(account, TimeWarpState.Resting, new DateTime(2001, 12, 12, 12, 12, 12),
                                  new TimeSpan(1, 30, 2), 3.0, 3);

            CreateInitialValuesInDatabase(account, state1, state2);

            var timeWarpUserState = UpdateValuesInDatabase(account);

            Assert.IsNotNull(timeWarpUserState);
            Assert.AreEqual(account.Id,timeWarpUserState.Account.Id);
            Assert.AreEqual(account.Name, timeWarpUserState.Account.Name);
            Assert.AreEqual(TimeWarpState.Resting,timeWarpUserState.State);
            Assert.AreEqual(3, timeWarpUserState.AgentType);
        }

        private static TimeWarpUserState UpdateValuesInDatabase(Account account)
        {
            var repository = new TimeWarpUserStateRepository();
            TimeWarpUserState timeWarpUserState = repository.GetLatestStateByAccountId(account.Id);
            return timeWarpUserState;
        }

        private static void CreateInitialValuesInDatabase(Account account, TimeWarpUserState state1, TimeWarpUserState state2)
        {
            var accountRepository = new AccountRepository();
            accountRepository.Add(account);

            var repository = new TimeWarpUserStateRepository();
            repository.Add(state1);
            repository.Add(state2);
        }
    }
}