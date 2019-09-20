using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core
{
    public interface IAgentTypeProvider
    {
        TimeWarpAgent Agent { get; }
    }
}