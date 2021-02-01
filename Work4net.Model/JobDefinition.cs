using System;
using System.Collections.Generic;

namespace Work4net.Model
{
    /// <summary>
    /// The job definition entry
    /// </summary>
    [Serializable]
    public class JobDefinition
    {
        /// <summary>
        /// The job definition id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The job definition code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The job definition group
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The job definition type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The collection of defined triggers
        /// </summary>
        public List<TriggerDefinition> Triggers { get; set; }

        /// <summary>
        /// The job status 
        /// </summary>
        public JobStatus Status { get; set; }

        /// <summary>
        /// The cluster assigned to job
        /// </summary>
        public string Cluster { get; set; }

        /// <summary>
        /// The job execution options
        /// </summary>
        public JobExecutionOptions Execution { get; set; }

        /// <summary>
        /// The set of job selectors
        /// </summary>
        public IDictionary<string, string> Selectors { get; set; }

        /// <summary>
        /// The job definition payload
        /// </summary>
        public IDictionary<string, object> Payload { get; set; }

        /// <summary>
        /// The creator user
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// The modifying user
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// The creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The last modification time
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// The extra data of job definition
        /// </summary>
        public IDictionary<string, object> Extra { get; set; }

        /// <summary>
        /// Shallow clone the instance
        /// </summary>
        /// <returns></returns>
        public JobDefinition ShallowClone()
        {
            return (JobDefinition)this.MemberwiseClone();
        }
    }
}