using System;

namespace Work4net.Execution
{
    /// <summary>
    /// The job executor exception
    /// </summary>
    public class JobExecutorException : Exception
    {
        /// <summary>
        /// Creates new instance of exception based on message
        /// </summary>
        /// <param name="message">The exception message</param>
        public JobExecutorException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates new instance of exception based on message and cause
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The cause of exception</param>
        public JobExecutorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
