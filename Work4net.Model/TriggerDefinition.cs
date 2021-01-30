using System;
using System.Collections.Generic;

namespace Work4net.Model
{
    /// <summary>
    /// The trigger definition object
    /// </summary>
    [Serializable]
    public class TriggerDefinition
    {
        /// <summary>
        /// The name of trigger
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The trigger type
        /// </summary>
        public TriggerType Type { get; set; }

        /// <summary>
        /// The cron expression
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// The static period to use in milliseconds
        /// </summary>
        public double Period { get; set; }

        /// <summary>
        /// The start time for trigger
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// The end time for trigger
        /// </summary>
        public DateTime EndAt { get; set; }

        /// <summary>
        /// The payload of trigger
        /// </summary>
        public IDictionary<string, object> Payload { get; set; }

        /// <summary>
        /// The time zone
        /// </summary>
        public string Timezone { get; set; }
    }
}