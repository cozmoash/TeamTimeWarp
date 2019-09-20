using Newtonsoft.Json;

namespace TeamTimeWarp.Client.Core.Services
{
    public static class JsonHelper
    {
        public static T SafeDeserializeObject<T>(string contents) where T : class
        {
            T result = null;
            try
            {
                result = JsonConvert.DeserializeObject<T>(contents);
            }
            catch(JsonSerializationException)
            {}
            catch (JsonReaderException)
            {}

            return result;
        }
    }
}