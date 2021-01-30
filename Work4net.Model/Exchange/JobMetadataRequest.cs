using System;

namespace Work4net.Model.Exchange
{
    /// <summary>
    /// The job metadata request
    /// </summary>
    [Serializable]
    public class JobMetadataRequest
    {
        /// <summary>
        /// The target cluster for metadata
        /// </summary>
        public string Cluster { get; set; }
    }
}