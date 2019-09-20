using System.Reflection;
using NUnit.Framework;
using TeamTimeWarp.Rest.Controllers;

namespace TeamTimeWarp.Rest.UnitTests.Controllers.Version
{
    [TestFixture]
    public class WhenTheVersionIsRetreived
    {
        [Test]
        public void ThenTheVersionIsTheAssembly()
        {
            var versionController = new VersionController();
            var result = versionController.Get();
            var expected = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Assert.AreEqual(expected, result);
        }

    }
}