using System;

namespace Work4net.Model.Agent
{
    /// <summary>
    /// The agent health structure
    /// </summary>
    [Serializable]
    public class AgentHealth
    {
        /// <summary>
        /// The time of last update 
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// The last activity type
        /// </summary>
        public AgentActivityType LastActivity { get; set; }
    }
}