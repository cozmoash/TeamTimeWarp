namespace TimeManager.Client.Tray
{
    public class RestServiceUriFactory : IRestServiceUriFactory
    {
        public string Get()
        {
            return "http://localhost:3536/api";
        }
    }
}