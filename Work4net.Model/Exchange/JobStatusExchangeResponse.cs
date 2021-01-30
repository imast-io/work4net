using System;
using System.Collections.Generic;

namespace Work4net.Model.Exchange
{
    /// <summary>
    /// The job status exchange response
    /// </summary>
    [Serializable]
    public class JobStatusExchangeResponse
    {
        /// <summary>
        /// The target request group
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The target request type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The set of removed job codes
        /// </summary>
        public List<string> Removed { get; set; }

        /// <summary>
        /// The set of updated job definitions
        /// </summary>
        public IDictionary<string, JobDefinition> Updated { get; set; }

        /// <summary>
        /// The set of added job definitions
        /// </summary>
        public IDictionary<string, JobDefinition> Added { get; set; }
    }
}