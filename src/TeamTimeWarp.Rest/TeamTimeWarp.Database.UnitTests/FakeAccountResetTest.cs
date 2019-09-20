using NUnit.Framework;
using TeamTimeWarp.Rest.RandomDataGenerator;

namespace TeamTimeWarp.Database.UnitTests
{
    [TestFixture]
    public class FakeAccountResetTest
    {
        [Test]
        public void Reset()
        {
            Assert.DoesNotThrow(FakeAccountUserStateResetter.Reset);
        }

    }
}