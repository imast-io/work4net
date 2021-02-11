namespace Work4net.Worker
{
    /// <summary>
    /// The worker configuration
    /// </summary>
    public class WorkerConfiguration
    {
        /// <summary>
        /// The worker name
        /// </summary>
        public string Worker { get; set; }

        /// <summary>
        /// The cluster name
        /// </summary>
        public string Cluster { get; set; }

        /// <summary>
        /// The level of parallelism
        /// </summary>
        public long Parallelism { get; set; }

        /// <summary>
        /// The frequency of polling update (milliseconds)
        /// </summary>
        public long PollingRate { get; set; }

        /// <summary>
        /// The frequency of heartbeat update (milliseconds)
        /// </summary>
        public long HeartbeatRate { get; set; }

        /// <summary>
        /// The number of tries to register an agent
        /// </summary>
        public int WorkerRegistrationTries { get; set; }

        /// <summary>
        /// The type of clustering
        /// </summary>
        public ClusteringType ClusteringType { get; set; }

        /// <summary>
        /// The data source
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// The data source URI
        /// </summary>
        public string DataSourceUri { get; set; }

        /// <summary>
        /// The data source username
        /// </summary>
        public string DataSourceUsername { get; set; }

        /// <summary>
        /// The data source password
        /// </summary>
        public string DataSourcePassword { get; set; }
    }
}