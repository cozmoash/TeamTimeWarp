using System.Threading;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Tray
{
    public class ServiceContainer : IServiceContainer
    {
        public ServiceContainer(SynchronizationContext synchronizationContext)
        {
            var tokenStore = new TokenStore();
            var uri = new RestServiceUriFactory();
            var loginPersiter = new TokenPersister();
            AuthenticationService = new AuthenticationService(tokenStore, uri, loginPersiter,synchronizationContext);
            RoomService = new RoomService(tokenStore, uri);
            GlobalRoomService = new GlobalRoomsService(tokenStore, uri, synchronizationContext);
            UserStateService = new UserStateService(tokenStore, uri, new AgentTypeProvider(TimeWarpAgent.WindowsTrayClient));
            UserMessageService = new UserMessageService(tokenStore,uri,synchronizationContext);
        }

        public IUiAuthenticationService AuthenticationService { get; private set; }
        public IUiRoomService RoomService { get; private set; }
        public IUserStateService UserStateService { get; private set; }
        public IUiGlobalRoomsService GlobalRoomService { get; private set; }
        public IUiUserMessageService UserMessageService { get; private set; }

        //don't logout as other devices may be connected..
        public void Dispose()
        {
            AuthenticationService.Logout();
        }
    }
}