using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core
{
    public class AgentTypeProvider : IAgentTypeProvider
    {
        public AgentTypeProvider(TimeWarpAgent agent)
        {
            Agent = agent;
        }
        
        public TimeWarpAgent Agent { get; private set; }
    }
}