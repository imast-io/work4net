namespace Work4net.Worker
{
    /// <summary>
    /// The definitions of job management constants
    /// </summary>
    public class JobConstants
    {
        /// <summary>
        /// The default cluster name
        /// </summary>
        public static readonly string DEFAULT_CLUSTER = "DEFAULT_CLUSTER";

        /// <summary>
        /// The job session context
        /// </summary>
        public static readonly string JOB_SESSION_CONTEXT = "SESSION";

        /// <summary>
        /// The job definition 
        /// </summary>
        public static readonly string JOB_DEFINITION = "DEFINITION";

        /// <summary>
        /// The job id key
        /// </summary>
        public static readonly string PAYLOAD_JOB_ID = "_PLD_JOB_ID";
        
        /// <summary>
        /// The job code key
        /// </summary>
        public static readonly string PAYLOAD_JOB_CODE = "_PLD_JOB_CODE";

        /// <summary>
        /// The job group key
        /// </summary>
        public static readonly string PAYLOAD_JOB_GROUP = "_PLD_JOB_GROUP";

        /// <summary>
        /// The job type key
        /// </summary>
        public static readonly string PAYLOAD_JOB_TYPE = "_PLD_JOB_TYPE";

        /// <summary>
        /// The job tenant key
        /// </summary>
        public static readonly string PAYLOAD_JOB_TENANT = "_PLD_JOB_TENANT";

        /// <summary>
        /// The job status key
        /// </summary>
        public static readonly string PAYLOAD_JOB_STATUS = "_PLD_JOB_STATUS";

        /// <summary>
        /// The job cluster key
        /// </summary>
        public static readonly string PAYLOAD_JOB_CLUSTER = "_PLD_JOB_CLUSTER";

        /// <summary>
        /// The job execution key
        /// </summary>
        public static readonly string PAYLOAD_JOB_EXECUTION = "_PLD_JOB_EXECUTION";

        /// <summary>
        /// The job created key
        /// </summary>
        public static readonly string PAYLOAD_JOB_CREATED = "_PLD_JOB_CREATED";

        /// <summary>
        /// The job updated key
        /// </summary>
        public static readonly string PAYLOAD_JOB_MODIFIED = "_PLD_JOB_MODIFIED";

        /// <summary>
        /// The job module key
        /// </summary>
        public static readonly string JOB_MODULES = "JOB_MODULES";

        /// <summary>
        /// The job factory
        /// </summary>
        public static readonly string WORKER_FACTORY = "WORKER_FACTORY";
    }
}