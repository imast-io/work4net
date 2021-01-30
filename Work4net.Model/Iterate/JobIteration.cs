using System;
using System.Collections.Generic;

namespace Work4net.Model.Iterate
{
    /// <summary>
    /// The job iteration structure
    /// </summary>
    [Serializable]
    public class JobIteration
    {
        /// <summary>
        /// The iteration id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The job identifier
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// The iteration status
        /// </summary>
        public IterationStatus Status { get; set; }

        /// <summary>
        /// The iteration result payload
        /// </summary>
        public IDictionary<string, object> Payload { get; set; }

        /// <summary>
        /// The iteration result message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The iteration runtime in milliseconds
        /// </summary>
        public long Runtime { get; set; }

        /// <summary>
        /// The iteration timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}