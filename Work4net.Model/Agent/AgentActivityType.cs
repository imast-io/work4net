using System;

namespace Work4net.Model.Agent
{
    /// <summary>
    /// The agent activity types
    /// </summary>
    [Serializable]
    public enum AgentActivityType
    {
        /// <summary>
        /// The agent registers in the system
        /// </summary>
        REGISTER,

        /// <summary>
        /// The agent heartbeat signal
        /// </summary>
        HEARTBEAT,

        /// <summary>
        /// The agent shutdown indication
        /// </summary>
        SHUTDOWN
    }
}