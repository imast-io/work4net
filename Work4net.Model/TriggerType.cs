using System;

namespace Work4net.Model
{
    /// <summary>
    /// The trigger types
    /// </summary>
    [Serializable]
    public enum TriggerType
    {
        /// <summary>
        /// The static period schedule
        /// </summary>
        STATIC_PERIOD,

        /// <summary>
        /// The cron type schedule
        /// </summary>
        CRON,

        /// <summary>
        /// The one-time execution schedule
        /// </summary>
        ONE_TIME
    }
}
