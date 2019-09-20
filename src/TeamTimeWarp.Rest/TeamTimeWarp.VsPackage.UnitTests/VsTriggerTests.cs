using System;
using NUnit.Framework;
using TeamTimeWarp.Client.tray.UnitTests;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.TeamTimeWarp_VsPackage;

namespace TeamTimeWarp.VsPackage.UnitTests
{


    public class FakeDteTrigger : IDteTrigger
    {

        public void Dispose()
        {
            
        }

        public event EventHandler<EventArgs> OnDteTrigger;

        public virtual void OnOnDteTrigger()
        {
            EventHandler<EventArgs> handler = OnDteTrigger;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

    [TestFixture]
    public class VsTriggerTests
    {
        [Test]
        public void DoesTriggerWorkOnStart()
        {
            FakeDteTrigger fakeDteTrigger = new FakeDteTrigger();
            MockUserStateService mockUserStateService = new MockUserStateService();
            MockAuthenticationService mockAuthenticationService = new MockAuthenticationService();

            mockAuthenticationService.RaiseLoginComplete();
            mockUserStateService.SetUserState(new UserStateInfoResponse(1, "ae", new DateTime(2000, 12, 12),
                                                                        TimeWarpState.None, new DateTime(2012, 12, 12),
                                                                        TimeSpan.Zero, 0, false, TimeWarpAgent.VisualStudio));
            VsWorkStarter vsWorkStarter = new VsWorkStarter(mockUserStateService,mockAuthenticationService,fakeDteTrigger);

            vsWorkStarter.StartWorkAsync();

            Assert.AreEqual(1,mockUserStateService.StartWorkCalled);
        }


    }
}
