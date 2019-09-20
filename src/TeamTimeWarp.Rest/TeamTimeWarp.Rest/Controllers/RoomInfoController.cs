using System.Collections.Generic;
using System.ComponentModel;
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
    public class RoomInfoController : ApiController
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IAccountRepository _accounts;
        private readonly ITimeWarpAuthenticationManager _authenticationManager;

        private readonly INowProvider _nowProvider;
        private static readonly ILog Log = LogManager.GetLogger(typeof(RoomInfoController));

        public RoomInfoController(IRoomRepository roomRepository, IAccountRepository accounts, ITimeWarpAuthenticationManager authenticationManager,
                                INowProvider nowProvider)
        {
            _roomRepository = roomRepository;
            _nowProvider = nowProvider;
            _accounts = accounts;
            _authenticationManager = authenticationManager;
        }

        //gets all rooms for logged in user.
        public IEnumerable<RoomInfo> Get(HttpRequestMessage request)
        {
            long accountId;
            if (!_authenticationManager.TryAuthenticateForReadOperation(request.GetToken(), out accountId))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));

            var rooms = _roomRepository.GetRooms(accountId);
                
            return rooms.Select(x=>x.ConvertToPublicV001()).ToList();
        }
        
        //creates a room
        public RoomInfo Post(HttpRequestMessage request, string roomName)
        {
            if(Log.IsDebugEnabled)
                Log.DebugFormat("creating new room ({0})", roomName);

            long accountId;
            if(!_authenticationManager.TryAuthenticateForReadOperation(request.GetToken(),out accountId))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
     
            if (string.IsNullOrWhiteSpace(roomName))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No Room name" });

            var existingRoom = _roomRepository.GetRoom(roomName);
            if(existingRoom != null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Room already exists" });
            
            var queryTime = _nowProvider.Now;
            //todo: add created by.
            var newRoom = new Room(0, roomName, queryTime);
            _roomRepository.Add(newRoom);
            return newRoom.ConvertToPublicV001();
        }

        //adds a user to a room
        public void Post(HttpRequestMessage request, int roomId, UserRoomCommand userRoomCommand)
        {
            long userId;
            if (!_authenticationManager.TryAuthenticateForWriteOperation(request.GetToken(), out userId))
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("room entry failed due to authentication");

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }

            if(Log.IsDebugEnabled)
                Log.DebugFormat("user ({0}) room ({1}) command ({2})",userId, roomId,userRoomCommand );

            var room = _roomRepository.GetRoom(roomId);

            if (room == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No Room found" });

            var account = _accounts.Get(userId);

            if (account == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No Account Found" });

            if(room.Users.Contains(account) && userRoomCommand == UserRoomCommand.Join)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Already in room" });
            
            //remove from all current rooms...
            if (UserRoomCommand.Join == userRoomCommand)
            {
                IEnumerable<Room> currentRooms = _roomRepository.GetRooms(account.Id);
                foreach (var currentRoom in currentRooms)
                {
                    currentRoom.Remove(account);
                    _roomRepository.Add(currentRoom);
                }
            }

            lock (room)
            {
                switch (userRoomCommand)
                {
                    case (UserRoomCommand.Join):
                        room.Add(account);
                        break;
                    case (UserRoomCommand.Leave):
                        room.Remove(account);
                        break;
                    default:
                        throw new InvalidEnumArgumentException("userRoomCommand", (int) userRoomCommand,
                                                                typeof (UserRoomCommand));
                }
            }
            
            if (Log.IsDebugEnabled)
                Log.Debug("adding to room");

            //todo: dont do this if the user collection is unchanged.
            _roomRepository.Add(room);


            

        }

        //todo: deletes a room
        public void Delete(int id)
        {
            
        }
    }
}