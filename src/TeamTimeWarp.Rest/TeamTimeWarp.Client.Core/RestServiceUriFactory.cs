namespace TeamTimeWarp.Client.Core
{
    public class RestServiceUriFactory : IRestServiceUriFactory
    {
        public string Get()
        {
            return "http://www.teamtimewarp.com/api/";
            //return "http://localhost:3536/api";
        }
    }
}