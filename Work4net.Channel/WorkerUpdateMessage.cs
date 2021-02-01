using Work4net.Model;

namespace Work4net.Channel
{
    /// <summary>
    /// The update message for worker
    /// </summary>
    public class WorkerUpdateMessage
    {
        /// <summary>
        /// The update operation type
        /// </summary>
        public UpdateOperation Operation { get; set; }

        /// <summary>
        /// The job definition code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The job definition group
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The job definition instance 
        /// </summary>
        public JobDefinition Definition { get; set; }
    }
}
