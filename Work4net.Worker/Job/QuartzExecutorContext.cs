using System.Collections.Generic;
using Quartz;
using Work4net.Execution;

namespace Work4net.Worker.Job
{
    /// <summary>
    /// The quartz executor context 
    /// </summary>
    public class QuartzExecutorContext : IJobExecutorContext
    {
        /// <summary>
        /// The quartz job execution context
        /// </summary>
        private readonly IJobExecutionContext context;

        /// <summary>
        /// Creates new executor context
        /// </summary>
        /// <param name="context">The quartz execution context</param>
        public QuartzExecutorContext(IJobExecutionContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the job code
        /// </summary>
        /// <returns></returns>
        public virtual string GetCode()
        {
            return JobOps.GetValue<string>(this.context.JobDetail.JobDataMap, JobConstants.PAYLOAD_JOB_CODE);
        }

        /// <summary>
        /// Gets the job group
        /// </summary>
        /// <returns></returns>
        public virtual string GetGroup()
        {
            return JobOps.GetValue<string>(this.context.JobDetail.JobDataMap, JobConstants.PAYLOAD_JOB_GROUP);
        }

        /// <summary>
        /// Gets the job type
        /// </summary>
        /// <returns></returns>
        public virtual string GetJobType()
        {
            return JobOps.GetValue<string>(this.context.JobDetail.JobDataMap, JobConstants.PAYLOAD_JOB_TYPE);
        }

        /// <summary>
        /// Gets registered module with given key and type
        /// </summary>
        /// <typeparam name="T">The module type</typeparam>
        /// <param name="key">The key of module</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        public virtual T GetModuleOr<T>(string key, T defaultValue) where T : class
        {
            return JobOps.GetContextModule<T>(key, this.GetJobType(), this.context) ?? defaultValue;
        }

        /// <summary>
        /// Gets job value with given key and type
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="key">The key of value</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        public virtual T GetJobValueOr<T>(string key, T defaultValue)
        {
            return JobOps.GetValueOr(this.context.JobDetail.JobDataMap, key, defaultValue);
        }

        /// <summary>
        /// Gets trigger value with given key and type
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="key">The key of value</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        public virtual T GetTriggerValueOr<T>(string key, T defaultValue)
        {
            return JobOps.GetValueOr(this.context.Trigger.JobDataMap, key, defaultValue);
        }

        /// <summary>
        /// Gets value (lookup in trigger, then in job) with given key and type
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="key">The key of value</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        public virtual T GetValue<T>(string key, T defaultValue)
        {
            return JobOps.GetValueOr(this.context.MergedJobDataMap, key, defaultValue);
        }

        /// <summary>
        /// Sets the output payload
        /// </summary>
        /// <param name="payload">The payload of output</param>
        public virtual void SetOutput(IDictionary<string, object> payload)
        {
            this.context.Result = payload;
        }
    }
}