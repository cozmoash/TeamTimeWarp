using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using DbUp;
using DbUp.ScriptProviders;
using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;
using TeamTimeWarp.Database;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Server.IntegrationTests
{
   // [TestFixture]
   //// [Ignore]
   // public class CreateDevAccount : IntegrationTestBase
   // {
   //     [Test]
   //     public void Create()
   //     {
   //         UserName = "devUser";
   //         Password = "dev";
   //         EmailAddress = "dev";

   //         CreateAccount();

   //         Assert.IsNotNull(LoginToken);
   //     }

   // }

    public abstract class IntegrationTestBase
    {
        protected RestClient Client;
        protected string UserName;
        protected string Password;
        protected string EmailAddress;
        protected string LoginToken;
        private AccountCreationResponse _accountCreationResponse;

        private void DeployDatabase()
        {
            var upgrader = new DatabaseUpgrader(
                "server=(local);Initial Catalog=TeamTimeWarp;Integrated Security=True",
                new EmbeddedScriptProvider(typeof(DbScripts).Assembly)
            );

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                throw result.Error;
            }
        }

        protected IntegrationTestBase()
        {
            DeployDatabase();


#if DEBUG
            Client = new RestClient("http://localhost:3536/api");
#else
            Client = new RestClient("http://localhost:84/api");
#endif
            var testId = GetType().Name + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            UserName = "username_" + testId;
            Password = "password_" + testId;
            EmailAddress = "test@" + testId + ".com";
        }

        public void Logout()
        {
            var request =
                new RestRequest(
                    string.Format("logout"),
                    Method.POST);
            request.AddHeader("login-token", _accountCreationResponse.Token);

            IRestResponse result = Client.Execute(request);

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        public void Login()
        {
            var request =
                new RestRequest(
                    string.Format("logon"),
                    Method.POST);
            request.AddHeader("password", Password);
            request.AddHeader("email-address", EmailAddress);

            IRestResponse result = Client.Execute(request);

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(result.Content);

            Assert.IsNotNull(loginResponse);

            LoginToken = loginResponse.Token;
            Assert.IsNotNullOrEmpty(LoginToken);
        }

        public void LoginWithNoPassword()
        {
            var request =
                new RestRequest(
                    string.Format("logon"),
                    Method.POST);
            
            request.AddHeader("email-address", EmailAddress);

            IRestResponse result = Client.Execute(request);

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(result.Content);

            Assert.IsNotNull(loginResponse);

            LoginToken = loginResponse.Token;
            Assert.IsNotNullOrEmpty(LoginToken);
        }


        public AccountCreationResponse CreateQuickLoginAccount()
        {
            var request =
                new RestRequest(
                    string.Format("account/?name={0}", UserName),
                    Method.POST);
            
            IRestResponse result = Client.Execute(request);

            _accountCreationResponse = JsonConvert.DeserializeObject<AccountCreationResponse>(result.Content);

            LoginToken = _accountCreationResponse.Token;
            return _accountCreationResponse;
        }

        public AccountCreationResponse CreateAccountWithNoPassword()
        {
            var request =
                new RestRequest(
                    string.Format("account/?name={0}&emailAddress={1}", UserName, EmailAddress),
                    Method.POST);
            
            IRestResponse result = Client.Execute(request);

            _accountCreationResponse = JsonConvert.DeserializeObject<AccountCreationResponse>(result.Content);

            LoginToken = _accountCreationResponse.Token;
            return _accountCreationResponse;
        }

        public AccountCreationResponse CreateAccount()
        {
            var request =
                new RestRequest(
                    string.Format("account/?name={0}&emailAddress={1}", UserName, EmailAddress),
                    Method.POST);
            request.AddHeader("password", Password);

            IRestResponse result = Client.Execute(request);

            _accountCreationResponse = JsonConvert.DeserializeObject<AccountCreationResponse>(result.Content);

            LoginToken = _accountCreationResponse.Token;
            return _accountCreationResponse;
        }

        public IRestResponse StartWork(TimeWarpAgent timeWarpAgent = TimeWarpAgent.Unknown)
        {
            return ChangeState(TimeWarpCommand.Work, timeWarpAgent);
        }

        public IRestResponse StartRest(TimeWarpAgent timeWarpAgent = TimeWarpAgent.Unknown)
        {
            return ChangeState(TimeWarpCommand.Rest, timeWarpAgent);
        }

        public IEnumerable<RoomInfo> GetRoomsForLoggedInUser()
        {
            var request =
                new RestRequest(
                    string.Format("roominfo"),
                    Method.GET);
            request.AddHeader("login-token", LoginToken);
            IRestResponse result = Client.Execute(request);

            var roomInfo = JsonConvert.DeserializeObject<IEnumerable<RoomInfo>>(result.Content);

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            return roomInfo;

        }


        public IEnumerable<UserStateInfoResponse> RoomStatus(int roomId)
        {
            var request =
                new RestRequest(
                    string.Format("roomstate/?roomId={0}", roomId),//is this right?
                    Method.GET);
            request.AddHeader("login-token", LoginToken);
            IRestResponse result = Client.Execute(request);

            var roomInfo = JsonConvert.DeserializeObject<IEnumerable<UserStateInfoResponse>>(result.Content);

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            return roomInfo;

        }

        public IRestResponse SendMessage(long toId, string message)
        {
            var request =
                new RestRequest(
                    string.Format("usermessage/?toAccount={0}", toId),
                    Method.POST);
            request.AddHeader("login-token", LoginToken);
            request.AddHeader("textMessage", message);
            return Client.Execute(request);
        }

        public ICollection<UserMessageReceipt> GetMessages()
        {
            var request = new RestRequest("usermessage",Method.GET);
            request.AddHeader("login-token", LoginToken);
            IRestResponse result = Client.Execute(request);
            return JsonConvert.DeserializeObject<ICollection<UserMessageReceipt>>(result.Content);
        }

        public RoomInfo CreateRoom(string roomName)
        {
            var request =
                new RestRequest(
                    string.Format("roominfo/?roomname={0}", roomName),
                    Method.POST);
            request.AddHeader("login-token", LoginToken);
            IRestResponse result = Client.Execute(request);

            var roomInfo =  JsonConvert.DeserializeObject<RoomInfo>(result.Content);

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            return roomInfo;
        }

        public IEnumerable<RoomInfo> SearchRoom(string searchString)
        {
            var request =
                   new RestRequest(
                       string.Format("globalroominfo/?searchstring={0}", searchString),
                       Method.GET);

            IRestResponse result = Client.Execute(request);

            var rooms = JsonConvert.DeserializeObject<IEnumerable<RoomInfo>>(result.Content);

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            return rooms;
        }



        public void JoinRoom(int roomId)
        {
            ExecuteRoomCommand(roomId, UserRoomCommand.Join);
        }

        public void LeaveRoom(int roomId)
        {
            ExecuteRoomCommand(roomId, UserRoomCommand.Leave);
        }
        
        private void ExecuteRoomCommand(int roomId, UserRoomCommand command)
        {
            var request =
                new RestRequest(
                    string.Format("roominfo/?roomId={0}&userRoomCommand={1}", roomId,
                                   command),
                    Method.POST);
            request.AddHeader("login-token", LoginToken);
            IRestResponse result = Client.Execute(request);
            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        private IRestResponse ChangeState(TimeWarpCommand command, TimeWarpAgent timeWarpAgent)
        {
            var request =
                new RestRequest(
                    string.Format("userstate/?command={0}&agent={1}", (int)command, (int)timeWarpAgent),
                    Method.POST);
            request.AddHeader("login-token", LoginToken);
            var response =  Client.Execute(request);

            return response;
            // Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        public UserStateInfoResponse GetCurrentState()
        {
            var request =
                new RestRequest(
                    string.Format("userstate"),
                    Method.GET);
            request.AddHeader("login-token", LoginToken);
            IRestResponse result = Client.Execute(request);

            var response = JsonConvert.DeserializeObject<UserStateInfoResponse>(result.Content);

            //Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            return response;
        }
    }
}