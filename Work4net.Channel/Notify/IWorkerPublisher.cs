namespace Work4net.Channel.Notify
{
    /// <summary>
    /// The special publisher interface for worker communication
    /// </summary>
    public interface IWorkerPublisher
    {
        /// <summary>
        /// Publish the message to worker
        /// </summary>
        /// <param name="message">The message to publish</param>
        void Publish(WorkerUpdateMessage message);
    }
}