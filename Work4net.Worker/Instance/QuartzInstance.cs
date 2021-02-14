using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using log4net;
using Quartz;
using Quartz.Impl.Matchers;
using Work4net.Common.Log;
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
        /// Logger
        /// </summary>
        private readonly ILog log = LogBuilder.Get(MethodBase.GetCurrentMethod()?.DeclaringType);

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
        /// Schedules job definition
        /// </summary>
        /// <param name="job">The job definition</param>
        public void Schedule(JobDefinition job)
        {
            // safety check
            if (job == null)
            {
                return;
            }

            // make sure to access scheduler without sync issues
            lock (this.scheduler)
            {
                this.ScheduleImpl(job);
            }
        }

        /// <summary>
        /// Reschedules job definition
        /// </summary>
        /// <param name="job">The job definition</param>
        public void Reschedule(JobDefinition job)
        {
            // safety check
            if (job == null)
            {
                return;
            }

            // make sure to access scheduler without sync issues
            lock (this.scheduler)
            {
                this.Reschedule(job);
            }
        }

        /// <summary>
        /// Unschedule job definition
        /// </summary>
        /// <param name="job">The job definition</param>
        public void Unschedule(JobDefinition job)
        {
            // safety check
            if (job == null)
            {
                return;
            }

            // make sure to access scheduler without sync issues
            lock (this.scheduler)
            {
                this.Unschedule(job);
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
        /// Compute current status for exchange
        /// </summary>
        /// <param name="group">The group of jobs</param>
        /// <param name="type">The target job type</param>
        /// <returns></returns>
        public JobStatusExchangeRequest GetStatus(string group, string type)
        {
            lock (this.scheduler)
            {
                return this.GetStatusImpl(group, type);
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
                    log.Error("QuartzInstance: Unable to schedule job that has been already scheduled");
                    return;
                }

                // build job definition
                var jobDetail = this.factory.CreateJob(key, jobDefinition);

                // could not create job details
                if (jobDetail == null)
                {
                    log.Error("QuartzInstance: Unable to create job via factory");
                    return;
                }

                // init mandatory fields
                this.factory.InitJob(jobDetail, jobDefinition);

                // build triggers for job
                var triggers = this.factory.CreateTriggers(jobDefinition);

                // schedule jobs
                this.scheduler.ScheduleJob(jobDetail, triggers.ToImmutableList(), true);

                log.Info($"QuartzInstance: Job ({jobDefinition.Code} in {jobDefinition.Group})  is scheduled");
            }
            catch (Exception ex)
            {
                log.Error("QuartzInstance: Failed to schedule the job", ex);
            }
        }

        /// <summary>
        /// Reschedules job definition
        /// </summary>
        /// <param name="jobDefinition">The job definition</param>
        protected void RescheduleImpl(JobDefinition jobDefinition)
        {
            try
            {
                // build job key
                var key = JobKey.Create(jobDefinition.Code, jobDefinition.Group);

                // check if job exists
                var exists = this.scheduler.CheckExists(key).Result;

                // job does not exists then nothing to do
                if (!exists)
                {
                    log.Error("QuartzInstance: Job cannot be updated because it does not exist");
                    return;
                }

                // try create job
                var jobDetail = this.scheduler.GetJobDetail(key).Result;

                // unschedule if exists
                if (jobDetail == null)
                {
                    log.Error("QuartzInstance: Unable to find job by the key factory");
                    return;
                }

                // init mandatory fields
                this.factory.InitJob(jobDetail, jobDefinition);

                // unschedule the triggers
                this.UnscheduleTriggers(key);

                // build triggers for job
                var triggers = this.factory.CreateTriggers(jobDefinition);

                // schedule jobs
                this.scheduler.ScheduleJob(jobDetail, triggers.ToImmutableList(), true);

                log.Info($"QuartzInstance: Job ({jobDefinition.Code} in {jobDefinition.Group})  is rescheduled");
            }
            catch (Exception ex)
            {
                log.Error("QuartzInstance: Failed to reschedule the job", ex);
            }
        }

        /// <summary>
        /// Unschedule job definition
        /// </summary>
        /// <param name="jobDefinition">The job definition</param>
        protected void UnscheduleImpl(JobDefinition jobDefinition)
        {
            try
            {
                // build job key
                var key = JobKey.Create(jobDefinition.Code, jobDefinition.Group);

                // check if job exists
                var exists = this.scheduler.CheckExists(key).Result;

                // job does not exists then nothing to do
                if (!exists)
                {
                    log.Error("QuartzInstance: Job cannot be unscheduled because it does not exist");
                    return;
                }
                
                // unschedule the triggers
                this.UnscheduleTriggers(key);

                // delete the job
                var deleted = this.scheduler.DeleteJob(key).Result;

                if (!deleted)
                {
                    log.Warn("Something went wrong while deleting the job");
                }

                log.Info($"QuartzInstance: Job ({jobDefinition.Code} in {jobDefinition.Group})  is deleted");
            }
            catch (Exception ex)
            {
                log.Error("QuartzInstance: Failed to reschedule the job", ex);
            }
        }

        /// <summary>
        /// Unschedule triggers of job definition
        /// </summary>
        /// <param name="key">The job key</param>
        protected void UnscheduleTriggers(JobKey key)
        {
            try
            {
                // job does not exists then nothing to do
                if (!this.scheduler.CheckExists(key).Result)
                {
                    log.Error("QuartzInstance: Job cannot be updated because it does not exist");
                    return;
                }

                // get triggers of job
                var triggers = this.scheduler.GetTriggersOfJob(key).Result;
                
                // process each trigger to unschedule
                foreach (var trigger in triggers)
                {
                    var result = this.scheduler.UnscheduleJob(trigger.Key).Result;

                    if (!result)
                    {
                        log.Warn("Something went wrong while unscheduling the trigger");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("QuartzInstance: Failed to unschedule triggers", ex);
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

            try
            {
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
            }
            catch (SchedulerException ex)
            {
                log.Error("QuartzInstance: Could not compute status of executing jobs.", ex);
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