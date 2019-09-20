using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Rest.Authentication;
using TeamTimeWarp.Rest.Tests.Controllers.Accounts;
using TeamTimeWarp.Rest.Tests.Controllers.Mocks;

namespace TeamTimeWarp.Rest.Tests.Controllers.Authentication
{


    public class FakeAuthenticationSessionRepository : IAuthenticationSessionRepository
    {
        readonly IDictionary<string,AuthenticationSession> _authenticationSessions = new Dictionary<string, AuthenticationSession>(); 

        public IList<AuthenticationSession> GetAll()
        {
            return _authenticationSessions.Values.ToArray();
        }

        public void Add(AuthenticationSession item)
        {
            _authenticationSessions.Add(item.Token,item);
        }

        public bool TryGetByToken(string token, out AuthenticationSession result)
        {
            return _authenticationSessions.TryGetValue(token, out result);
        }
    }

    [TestFixture]
    public class AuthenticationManagerTests
    {
        private MockAccountPasswordRepository _mockAccountPasswordRepository;
        private TimeWarpAuthenticationManager _timeWarpAuthenticationManager;

        [SetUp]
        public void Setup()
        {
            _mockAccountPasswordRepository = new MockAccountPasswordRepository();
            _mockAccountPasswordRepository.Add(new AccountPassword(new Account(1, "name", "bean@bean.com",AccountType.Full), "bean"));

            _timeWarpAuthenticationManager = new TimeWarpAuthenticationManager(new FakeAuthenticationSessionRepository(), _mockAccountPasswordRepository, new FakeNowProvider() { Now = new DateTime(2000, 12, 12) });
            
        }

        [Test]
        public void SuccessfulAuthenticationTest()
        {
            ServiceLoginToken token;
            bool success = _timeWarpAuthenticationManager.TryAuthenticate("bean@bean.com", "bean", out token);

            Assert.IsTrue(success);
            Assert.AreEqual(1,token.AccountId);
            Assert.IsNotNullOrEmpty(token.Token);
        }

        [Test]
        public void UnSuccessfulAuthenticationTest()
        {
            ServiceLoginToken token;
            bool success = _timeWarpAuthenticationManager.TryAuthenticate("asdpjpj.com", "bean", out token);

            Assert.IsFalse(success);
            Assert.IsNull(token);
        }

    }
}
