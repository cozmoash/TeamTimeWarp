using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Converters;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Authentication;
using log4net;

namespace TeamTimeWarp.Rest.Controllers
{
    public class UserMessageController : ApiController
    {
        private readonly IUserMessageRepository _userMessageRepository;
        private readonly ITimeWarpAuthenticationManager _authenticationManager;
        private readonly IRoomRepository _roomRepository;
        private readonly INowProvider _nowProvider;

        private static readonly ILog Log = LogManager.GetLogger(typeof(UserMessageController));

        public UserMessageController(IUserMessageRepository userMessageRepository, ITimeWarpAuthenticationManager authenticationManager, IRoomRepository roomRepository, INowProvider nowProvider)
        {
            _userMessageRepository = userMessageRepository;
            _authenticationManager = authenticationManager;
            _roomRepository = roomRepository;
            _nowProvider = nowProvider;
        }

        public void Post(HttpRequestMessage request, long toAccount)
        {
            long senderAccountId;
            if (!_authenticationManager.TryAuthenticateForWriteOperation(request.GetToken(), out senderAccountId))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            
            if (!UsersInTheSameRoom(toAccount, senderAccountId))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }


            var messageStr = request.GetHeader("textMessage");

            if(string.IsNullOrWhiteSpace(messageStr))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var message = new UserMessage(new LazyAccount(toAccount), new LazyAccount(senderAccountId), _nowProvider.Now,
                                          messageStr);
            try
            {
                _userMessageRepository.Add(message);
            }
            catch (Exception ex)
            {
                Log.Warn("unable to send message",ex);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            
        }

        private bool UsersInTheSameRoom(long toAccount, long accountId)
        {
            IEnumerable<Account> userIsAbleToMessage = _roomRepository.GetRooms(accountId).SelectMany(x => x.Users);
            var account = userIsAbleToMessage.FirstOrDefault(x => x.Id == toAccount);
            return account != null;
        }

        public ICollection<UserMessageReceipt> Get(HttpRequestMessage request)
        {
            long accountId;
            if (!_authenticationManager.TryAuthenticateForWriteOperation(request.GetToken(), out accountId))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }

            IList<UserMessage> messages = _userMessageRepository.GetAllPendingMessagesForAccount(accountId);

            foreach (var message in messages)
            {
                message.HasBeenReceived = true;
                _userMessageRepository.Add(message);
            }

            return messages.Select(message => message.Convert()).ToArray();
        }

        
    }
}
