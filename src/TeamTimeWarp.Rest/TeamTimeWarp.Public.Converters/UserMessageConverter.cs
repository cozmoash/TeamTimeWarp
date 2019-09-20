using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Public.Converters
{
    public static class UserMessageConverter
    {
        public static UserMessageReceipt Convert(this UserMessage userMessage)
        {
            return new UserMessageReceipt(userMessage.FromAccount.Id, userMessage.FromAccount.Name,
                                          userMessage.TextMessage, userMessage.SendTime);
        }
    }
}