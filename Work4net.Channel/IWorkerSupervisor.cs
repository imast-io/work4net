using System;

namespace Work4net.Channel
{
    /// <summary>
    /// The worker supervision module
    /// </summary>
    public interface IWorkerSupervisor
    {
        /// <summary>
        /// Starts the supervision
        /// </summary>
        void Start();

        /// <summary>
        /// Adds a consumer to supervisor
        /// </summary>
        /// <param name="consumer">The consumer instance</param>
        void Add(Action<WorkerUpdateMessage> consumer);

        /// <summary>
        /// Removes a consumer from supervisor
        /// </summary>
        /// <param name="consumer">The consumer instance</param>
        void Remove(Action<WorkerUpdateMessage> consumer);

        /// <summary>
        /// Stops the supervision
        /// </summary>
        void Stop();
    }
}