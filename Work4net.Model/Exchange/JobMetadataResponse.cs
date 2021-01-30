using System;
using System.Collections.Generic;

namespace Work4net.Model.Exchange
{
    /// <summary>
    /// The job metadata response
    /// </summary>
    [Serializable]
    public class JobMetadataResponse
    {
        /// <summary>
        /// The cluster for metadata
        /// </summary>
        public string Cluster { get; set; }

        /// <summary>
        /// The set of existing groups
        /// </summary>
        public List<string> Groups { get; set; }

        /// <summary>
        /// THe set of existing types
        /// </summary>
        public List<string> Types { get; set; }
    }
}