using System.Threading.Tasks;
using Quartz;

namespace Work4net.Worker.Job
{
    /// <summary>
    /// The quartz-based executor job 
    /// </summary>
    public class QuartzExecutorJob : IJob
    {
        /// <summary>
        /// Create and invoke corresponding executor by type
        /// </summary>
        /// <param name="context">The context of job execution</param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            // wrap context into an abstract type
            var executorContext = new QuartzExecutorContext(context);

            // the job type
            var type = executorContext.GetJobType();

            // no type defined
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new JobExecutionException("Type is missing");
            }

            // get supplier of job executor
            var supplier = JobOps.GetExecutor(type, context);

            // no supplier for job type
            if (supplier == null)
            {
                throw new JobExecutionException("The requested job type is not supported");
            }

            // create a job instance
            var instance = supplier.Invoke(executorContext);

            // check instance
            if (instance == null)
            {
                throw new JobExecutionException("Supplier of executor returned null. Cannot execute...");
            }

            return Task.Run(instance.Execute);
        }
    }
}