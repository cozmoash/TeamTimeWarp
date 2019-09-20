using System;
using NUnit.Framework;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Rest.Tests.Controllers.Users
{
    [TestFixture]
    public class WhenUserInformationIsRetrieved : UserControllerTestBase
    {
        private readonly UserStateInfoResponse _result;
        
        public WhenUserInformationIsRetrieved()
        {
            FakeNowProvider.Now = new DateTime(2000, 12, 12, 12, 12, 0, 5);
            _result = Controller.Get(Request);
        }

        [Test]
        public void ThenTheUserInformationContainsTheAccountId()
        {
            Assert.AreEqual(AccountId, _result.AccountId);
        }

        [Test]
        public void ThenTheUserInformationContainsTheTimeWarpState()
        {
            Assert.AreEqual(TimeWarpState.None, _result.State);
        }
    }
}