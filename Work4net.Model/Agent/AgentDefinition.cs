using System;

namespace Work4net.Model.Agent
{
    /// <summary>
    /// The agent definition
    /// </summary>
    [Serializable]
    public class AgentDefinition
    {
        /// <summary>
        /// The agent identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The agent cluster
        /// </summary>
        public string Cluster { get; set; }
        
        /// <summary>
        /// The name of worker
        /// </summary>
        public string Worker { get; set; }

        /// <summary>
        /// The agent name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The agent health
        /// </summary>
        public AgentHealth Health { get; set; }

        /// <summary>
        /// The expected heartbeat frequency
        /// </summary>
        public long HeartbeatFreq { get; set; }

        /// <summary>
        /// The registration timestamp
        /// </summary>
        public DateTime Registered { get; set; }
    }
}