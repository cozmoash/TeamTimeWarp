using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public interface IUserStateService
    {
        void StartWork();
        void StopWork();
        UserStateInfoResponse GetUserState();

    }
}