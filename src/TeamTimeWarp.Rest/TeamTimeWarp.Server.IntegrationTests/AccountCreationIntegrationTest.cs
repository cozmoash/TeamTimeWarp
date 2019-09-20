using System.Net;
using NUnit.Framework;
using RestSharp;
using TeamTimeWarp.UnitTests;

namespace TeamTimeWarp.Server.IntegrationTests
{
    public class IntegrationTestBase
    {
        protected RestClient Client;

        public IntegrationTestBase()
        {
            Client = new RestClient("http://localhost:3536/api");
        }
    }


    [TestFixture]
    public class AddNewUser : IntegrationTestBase
    {
        [Test]
        public void ShouldAddNewUser()
        {
            var request = new RestRequest(string.Format("account/?name={0}&emailAddress={1}",TestHelper.NameMock, TestHelper.EmailAddressMock), Method.POST);
            request.AddHeader("password", "bean");

            IRestResponse result = Client.Execute(request);

            

            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);
        }


    }
}
