using System;
using System.Collections.Generic;
using System.Text;

namespace CoreFramework.Network
{
    public interface IAgentProcessor
    {
        void AddAgent(Agent agent);


        void RemoveAgent(Agent agent);
    }
}
