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

namespace TeamTimeWarp.Rest.Controllers
{

    public class RoomRemovalPolicy : IRoomRemovalPolicy
    {
        public RoomRemovalPolicy()
        {
            StandardUsers = TimeSpan.FromDays(2);
            QuickUsers = TimeSpan.FromHours(1);            
        }

        public TimeSpan QuickUsers { get; private set; }
        public TimeSpan StandardUsers { get; private set; }
    }

    public interface IRoomRemovalPolicy
    {
        TimeSpan QuickUsers { get; }
        TimeSpan StandardUsers { get; }
    }

    public class RoomStateController : ApiController
    {
        private readonly IUserStateManager _userStateManager;
        private readonly IRoomRepository _rooms;
        private readonly IRoomRemovalPolicy _roomRemovalPolicy;
        private readonly INowProvider _nowProvider;

        public RoomStateController(IUserStateManager userStateManager, IRoomRepository rooms, IRoomRemovalPolicy roomRemovalPolicy, INowProvider nowProvider)
        {
            _userStateManager = userStateManager;
            _rooms = rooms;
            _roomRemovalPolicy = roomRemovalPolicy;
            _nowProvider = nowProvider;
        }


        public IEnumerable<UserStateInfoResponse> Get(int roomId)
        {
            var queryTime = _nowProvider.Now;

            var room = _rooms.GetRoom(roomId);
            if (room == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent));

            var accounts = room.Users;

            var results = new List<UserStateInfoResponse>();

            foreach(var account in accounts)
            {
                var removalTimeSpan = account.AccountType == AccountType.Quick
                                          ? _roomRemovalPolicy.QuickUsers
                                          : _roomRemovalPolicy.StandardUsers;

                TimeWarpUserState state;
                if (_userStateManager.TryGetCurrentState(account.Id, queryTime, out state))
                {
                    if ((queryTime.Subtract(state.PeriodStartTime) < removalTimeSpan))
                    {
                        var publicObject = state.ConvertToPublicV001(queryTime);
                        results.Add(publicObject);
                    }
                }
            }

            return results;
        }

    }
}