using System;

namespace Work4net.Model
{
    /// <summary>
    /// The job status types
    /// </summary>
    [Serializable]
    public enum JobStatus
    {
        /// <summary>
        /// The job is defined but not submitted
        /// </summary>
        DEFINED,

        /// <summary>
        /// The job is created and stored as active
        /// </summary>
        ACTIVE,

        /// <summary>
        /// The job execution is paused
        /// </summary>
        PAUSED,

        /// <summary>
        /// The job execution failed
        /// </summary>
        FAILED,

        /// <summary>
        /// The job execution is completed
        /// </summary>
        COMPLETED
    }
}
