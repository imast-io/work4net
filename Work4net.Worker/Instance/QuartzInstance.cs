using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Quartz;
using Quartz.Impl.Matchers;
using Work4net.Model;
using Work4net.Model.Exchange;
using Work4net.Worker.Job;

namespace Work4net.Worker.Instance
{
    /// <summary>
    /// The quartz worker instance implementation
    /// </summary>
    public class QuartzInstance
    {
        /// <summary>
        /// The worker name
        /// </summary>
        public string Worker { get; }

        /// <summary>
        /// The cluster name
        /// </summary>
        public string Cluster { get; set; }

        /// <summary>
        /// The scheduler instance
        /// </summary>
        protected readonly IScheduler scheduler;

        /// <summary>
        /// The worker factory instance
        /// </summary>
        protected readonly WorkerFactory factory;

        /// <summary>
        /// Creates new instance of quartz worker instance
        /// </summary>
        /// <param name="worker">The worker name</param>
        /// <param name="cluster">The cluster name</param>
        /// <param name="scheduler">The scheduler instance</param>
        /// <param name="factory">The worker factory instance</param>
        public QuartzInstance(string worker, string cluster, IScheduler scheduler, WorkerFactory factory)
        {
            this.Worker = worker;
            this.Cluster = cluster;
            this.scheduler = scheduler;
            this.factory = factory;
        }

        /// <summary>
        /// Starts the scheduler
        /// </summary>
        public void Start()
        {
            try
            {
                this.scheduler.Start();
            }
            catch (SchedulerException ex)
            {
                throw new WorkerException("Could not start the worker", ex);
            }
        }

        /// <summary>
        /// Stops the scheduler
        /// </summary>
        public void Stop()
        {
            try
            {
                this.scheduler.Shutdown();
            }
            catch (SchedulerException ex)
            {
                throw new WorkerException("Could not stop the worker", ex);
            }
        }

        /// <summary>
        /// Gets all jobs in the group
        /// </summary>
        /// <param name="group">The group of jobs</param>
        /// <returns></returns>
        public ISet<string> GetJobs(string group)
        {
            try {
                return this.scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group)).Result.Select(key => key.Name).ToHashSet();
            }
            catch(SchedulerException ex){
                throw new WorkerException("Unable to read job group names", ex);
            }
        }

        /// <summary>
        /// Gets the job types
        /// </summary>
        /// <returns></returns>
        public ISet<string> GetTypes()
        {
            return this.factory.GetTypes();
        }

        /// <summary>
        /// Schedules job definition
        /// </summary>
        /// <param name="jobDefinition">The job definition</param>
        protected void ScheduleImpl(JobDefinition jobDefinition)
        {
            try
            {
                // build job key
                var key = JobKey.Create(jobDefinition.Code, jobDefinition.Group);

                // check if job exists
                var exists = this.scheduler.CheckExists(key).Result;

                // job exists then nothing to do
                if (exists)
                {
                    return;
                }

                // build job definition
                var jobDetail = this.factory.CreateJob(key, jobDefinition);

                // could not create job details
                if (jobDetail == null)
                {
                    return;
                }

                // init mandatory fields
                this.factory.InitJob(jobDetail, jobDefinition);

                // build triggers for job
                var triggers = this.factory.CreateTriggers(jobDefinition);

                // schedule jobs
                this.scheduler.ScheduleJob(jobDetail, triggers.ToImmutableList(), true);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        /// <summary>
        /// Compute current status for exchange
        /// </summary>
        /// <param name="group">The group of jobs</param>
        /// <param name="type">The target job type</param>
        /// <returns></returns>
        private JobStatusExchangeRequest GetStatusImpl(string group, string type)
        {
            // the current status of jobs
            var status = new Dictionary<string, DateTime>();

            // get all job keys in group
            var keys = this.scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group)).Result;
            
            // process all jobs in group
            foreach (var jobKey in keys)
            {
                // get job details
                var job = this.scheduler.GetJobDetail(jobKey).Result;

                // skip if does not exist
                if (job == null)
                {
                    continue;
                }

                // the job code
                var code = JobOps.GetValueOr(job.JobDataMap, JobConstants.PAYLOAD_JOB_CODE, default(string));

                // get the job type
                var jobType = JobOps.GetValueOr(job.JobDataMap, JobConstants.PAYLOAD_JOB_TYPE, default(string));

                // modified time of job
                var modified = JobOps.GetValueOr(job.JobDataMap, JobConstants.PAYLOAD_JOB_MODIFIED, default(DateTime));

                // nothing about job modification is known
                if (modified == default)
                {
                    continue;
                }

                // consider jobs only of same type
                if (!string.Equals(jobType, type))
                {
                    continue;
                }

                status[code] = modified;
            }

            return new JobStatusExchangeRequest
            {
                Cluster = this.Cluster,
                Group = group,
                Type = type,
                State = status
            };
        }
    }
}