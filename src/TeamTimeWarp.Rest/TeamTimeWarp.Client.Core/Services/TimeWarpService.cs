using RestSharp;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public class TimeWarpService : UiServiceBase, IUiTimeWarpService
    {
        public TimeWarpService(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory)
            : base(tokenStore, restServiceUriFactory)
        {
        }

        public void StartWork()
        {
            ExecuteRequest(string.Format("userstate/?command={0}", (int) TimeWarpCommand.Work),
                           Method.POST);
        }

        public void StopWork()
        {
            ExecuteRequest(string.Format("userstate/?command={0}", (int)TimeWarpCommand.Rest),
                           Method.POST);

        }
    }
}