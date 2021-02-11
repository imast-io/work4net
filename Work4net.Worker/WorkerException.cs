using System;

namespace Work4net.Worker
{
    /// <summary>
    /// The worker exception type
    /// </summary>
    public class WorkerException : Exception
    {
        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="inner">The inner exception</param>
        public WorkerException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Creates new instance of exception
        /// </summary>
        /// <param name="message">The exception message</param>
        public WorkerException(string message) : this(message, null)
        {
        }
    }
}