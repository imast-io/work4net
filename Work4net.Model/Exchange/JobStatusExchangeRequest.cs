using System;
using System.Collections.Generic;

namespace Work4net.Model.Exchange
{
    /// <summary>
    /// The job status exchange request
    /// </summary>
    [Serializable]
    public class JobStatusExchangeRequest
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
        /// The target request cluster
        /// </summary>
        public string Cluster { get; set; }

        /// <summary>
        /// The request state of current entities
        /// </summary>
        public IDictionary<string, DateTime> State { get; set; }
    }
}