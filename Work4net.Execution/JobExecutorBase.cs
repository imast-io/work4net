namespace Work4net.Execution
{
    /// <summary>
    /// The base and abstract job executor 
    /// </summary>
    public abstract class JobExecutorBase
    {
        /// <summary>
        /// The job executor context instance 
        /// </summary>
        protected readonly IJobExecutorContext context;

        /// <summary>
        /// Creates new instance of executor based on context
        /// </summary>
        /// <param name="context">The given context</param>
        protected JobExecutorBase(IJobExecutorContext context)
        {
            this.context = context;
        }
    }
}