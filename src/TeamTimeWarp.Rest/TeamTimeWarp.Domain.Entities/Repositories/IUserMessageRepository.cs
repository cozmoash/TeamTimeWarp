using System.Collections.Generic;

namespace TeamTimeWarp.Domain.Entities.Repositories
{
    public interface IUserMessageRepository : IRepository<UserMessage>
    {
        IList<UserMessage> GetAllPendingMessagesForAccount(long accountId);
    }
}