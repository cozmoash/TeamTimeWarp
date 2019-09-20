using NUnit.Framework;
using TeamTimeWarp.Rest.Controllers;

namespace TeamTimeWarp.Rest.Tests.Controllers
{
    [TestFixture]
    public class VersionControllerTest
    {
        
        [Test]
        public void Get()
        {
            VersionController versionController = new VersionController();

            var result = versionController.Get();

            Assert.AreEqual(1,result);
        }

    }
}