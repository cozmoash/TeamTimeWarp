using System;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence.Accounts;
using TeamTimeWarp.UnitTests;

namespace TeamTimeWarp.Database.UnitTests
{
    [TestFixture]
    public class AuthenticationSessionPersistenceTest
    {
        [Test]
        public void CanSaveAuthenticationSession()
        {
            DateTime now = DateTime.Now;

            AuthenticationSessionRepository authenticationSessionRepository = new AuthenticationSessionRepository();

            GetAuthenticationSession(authenticationSessionRepository, now);

            AuthenticationSession result;
            bool success = authenticationSessionRepository.TryGetByToken("testToken" + now.Ticks, out result);
            Assert.IsTrue(success);

        }

        private static AuthenticationSession GetAuthenticationSession(AuthenticationSessionRepository authenticationSessionRepository, DateTime now)
        {
            DateTime dateTime = new DateTime(2012, 12, 12);
            var account1 = TestHelper.AccountMock();

            var accountRepository = new AccountRepository();
            accountRepository.Add(account1);

            var authenticationSession = new AuthenticationSession(account1, "testToken" + now.Ticks, dateTime);
            authenticationSessionRepository.Add(authenticationSession);

            Assert.That(authenticationSession.Id != 0);
            return authenticationSession;
        }


        [Test]
        public void CanUpdateLastValidatedTime()
        {
            DateTime now = DateTime.Now;

            AuthenticationSessionRepository authenticationSessionRepository = new AuthenticationSessionRepository();
            var session = GetAuthenticationSession(authenticationSessionRepository, now);
            session.LastValidation = new DateTime(2013, 12, 12);
            session.Token = "updatedToken" + now;

            authenticationSessionRepository.Add(session);

            AuthenticationSession result;
            bool success = authenticationSessionRepository.TryGetByToken("testToken" + now, out result);
            Assert.IsFalse(success);

            success = authenticationSessionRepository.TryGetByToken("updatedToken" + now, out result);
            Assert.IsTrue(success);
        }

    }
}