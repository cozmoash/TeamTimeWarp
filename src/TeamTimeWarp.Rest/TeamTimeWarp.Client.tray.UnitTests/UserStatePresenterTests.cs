using System;
using System.Threading;
using NUnit.Framework;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Tray;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray;

namespace TeamTimeWarp.Client.tray.UnitTests
{
    [TestFixture]
    public class WhenTheUserStateIsChanged
    {
        [Test]
        public void ThenAnEventIsRaisedToNotifyTheUi()
        {
            var mockUserStateService = new MockUserStateService();
            using (var userStatePresenter = new UserStateListener(mockUserStateService, null))
            {
                userStatePresenter.Start();
                
                bool success;
                using (var manualResetEvent = new ManualResetEvent(false))
                {
                    userStatePresenter.UserStateChanged += (sender, e) => manualResetEvent.Set();

                    var workingState = new UserStateInfoResponse(1,"test", new DateTime(2000, 12, 12),
                                                                 TimeWarpState.Working, new DateTime(2000, 12, 12),
                                                                 TimeSpan.FromSeconds(12), 0.2,true, TimeWarpAgent.VisualStudio);
                    mockUserStateService.SetUserState(workingState);

                    success = manualResetEvent.WaitOne(TimeSpan.FromSeconds(10));
                }

                Assert.IsTrue(success);
                Assert.AreEqual(TimeWarpState.Working, userStatePresenter.CurrentTimeWarpState);
            }
        }
    }

    [TestFixture]
    public class WhenThereIsAnErrorRetreivingTheUserState
    {
        [Test]
        public void ThenAnErrorEventIsRaisedOnce()
        {
            var mockUserStateService = new MockInvalidUserStateService();
            using (var userStatePresenter = new UserStateListener(mockUserStateService, null))
            {
                userStatePresenter.Start();

                bool success;
                using (var manualResetEvent = new ManualResetEvent(false))
                {
                    userStatePresenter.Error += (sender, e) => manualResetEvent.Set();
                    
                    success = manualResetEvent.WaitOne(TimeSpan.FromSeconds(10));
                }

                Assert.IsTrue(success);
                
            }
        }
    }
}