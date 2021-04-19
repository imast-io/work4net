using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Impl;
using Work4net.Channel;
using Work4net.Common.Log;
using Work4net.Model;
using Work4net.Model.Iterate;
using Work4net.Worker.Job;

namespace Work4net.Worker.Instance
{
    /// <summary>
    /// The every job listener 
    /// </summary>
    public class EveryJobListener : IJobListener
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILog log = LogBuilder.Get(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// The scheduler channel instance
        /// </summary>
        protected readonly ISchedulerChannel schedulerChannel;

        /// <summary>
        /// Creates new instance of every job listener
        /// </summary>
        /// <param name="schedulerChannel">The scheduler channel</param>
        public EveryJobListener(ISchedulerChannel schedulerChannel)
        {
            this.schedulerChannel = schedulerChannel;
        }

        /// <summary>
        /// The name of listener
        /// </summary>
        public string Name => "WORK4NET_JOB_LISTENER";

        /// <summary>
        /// The job is about to be executed
        /// </summary>
        /// <param name="context">The execution context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job is vetoed by trigger listener
        /// </summary>
        /// <param name="context">The execution context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job was executed 
        /// </summary>
        /// <param name="context">The execution context</param>
        /// <param name="jobException">The exception if any</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = new CancellationToken())
        {
            // the job data map
            var jobData = context.JobDetail.JobDataMap;

            // the job id
            var jobId = JobOps.GetValueOr(jobData, JobConstants.PAYLOAD_JOB_ID, default(string));

            // the job code
            var jobCode = JobOps.GetValueOr(jobData, JobConstants.PAYLOAD_JOB_CODE, default(string));

            // the job group
            var jobGroup = JobOps.GetValueOr(jobData, JobConstants.PAYLOAD_JOB_GROUP, default(string));

            // deduce the status of iteration
            var status = jobException == null ? IterationStatus.SUCCESS : IterationStatus.FAILURE;

            // the execution 
            var execution = JobOps.GetValueOr(jobData, JobConstants.PAYLOAD_JOB_EXECUTION, default(JobExecutionOptions));

            // if silent reporting is enabled
            var silent = execution != null && execution.SilentIterations;

            // the output of iteration
            var output = context.Result;

            // silent reporting is on
            if (silent)
            {
                return Task.CompletedTask;
            }

            // the job runtime record
            var runtime = (long)context.JobRunTime.TotalMilliseconds;

            // build new iteration entity
            var iteration = new JobIteration
            {
                Id = null,
                JobId = jobId,
                Status = status,
                Runtime = runtime,
                Payload = output as IDictionary<string, object>,
                Message = jobException?.Message,
                Timestamp = DateTime.Now
            };

            // report iteration
            var result = this.schedulerChannel.Iterate(iteration);

            // check if no result
            if (result == null)
            {
                log.Warn($"EveryJobListener: Could not register {status} iteration for job {jobCode} ({jobGroup})");
            }

            return Task.CompletedTask;
        }
    }
}