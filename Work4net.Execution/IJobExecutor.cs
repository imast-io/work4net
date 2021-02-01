namespace Work4net.Execution
{
    /// <summary>
    /// A special interface to define execution logic of the job
    /// </summary>
    public interface IJobExecutor
    {
        /// <summary>
        /// Executes the specified logic for the single triggered job instance
        /// </summary>
        void Execute();
    }
}