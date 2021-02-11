using System;
using System.Collections.Generic;
using System.Linq;
using Quartz;
using Work4net.Execution;
using Work4net.Model;
using Work4net.Worker.Job;

namespace Work4net.Worker
{
    /// <summary>
    /// The worker factory
    /// </summary>
    public class WorkerFactory
    {
        /// <summary>
        /// The job suppliers map
        /// </summary>
        protected readonly IDictionary<string, Func<IJobExecutorContext, IJobExecutor>> jobClasses;

        /// <summary>
        /// Creates new instance of worker factory
        /// </summary>
        public WorkerFactory()
        {
            this.jobClasses = new Dictionary<string, Func<IJobExecutorContext, IJobExecutor>>();
        }

        /// <summary>
        /// Gets the job types
        /// </summary>
        /// <returns></returns>
        public ISet<string> GetTypes()
        {
            return this.jobClasses.Keys.ToHashSet();
        }

        /// <summary>
        /// Registers the supplier of executor by type
        /// </summary>
        /// <param name="type">The type code</param>
        /// <param name="executorSupplier">The supplier of executor</param>
        public void RegisterExecutor(string type, Func<IJobExecutorContext, IJobExecutor> executorSupplier)
        {
            this.jobClasses[type] = executorSupplier;
        }

        /// <summary>
        /// Gets the executor by type
        /// </summary>
        /// <param name="type">The type of job</param>
        /// <returns></returns>
        public Func<IJobExecutorContext, IJobExecutor> GetExecutor(string type)
        {
            return this.jobClasses.TryGetValue(type, out var supplier) ? supplier : null;
        }

        /// <summary>
        /// Creates the job for definition
        /// </summary>
        /// <param name="key">The key of job</param>
        /// <param name="definition">The job definition</param>
        /// <returns></returns>
        public IJobDetail CreateJob(JobKey key, JobDefinition definition)
        {
            return JobBuilder
                .Create<QuartzExecutorJob>()
                .WithIdentity(key)
                .StoreDurably(false)
                .Build();
        }

        /// <summary>
        /// Initialize the job definition
        /// </summary>
        /// <param name="job">The job details</param>
        /// <param name="definition">The job definition</param>
        /// <returns></returns>
        public IJobDetail InitJob(IJobDetail job, JobDefinition definition)
        {
            // the data map by default
            var systemData = new Dictionary<string, object>
            {
                {JobConstants.PAYLOAD_JOB_ID, definition.Id},
                {JobConstants.PAYLOAD_JOB_CODE, definition.Code},
                {JobConstants.PAYLOAD_JOB_GROUP, definition.Group},
                {JobConstants.PAYLOAD_JOB_TYPE, definition.Type},
                {JobConstants.PAYLOAD_JOB_TENANT, definition.Tenant},
                {JobConstants.PAYLOAD_JOB_STATUS, definition.Status},
                {JobConstants.PAYLOAD_JOB_CLUSTER, definition.Cluster},
                {JobConstants.PAYLOAD_JOB_EXECUTION, definition.Execution},
                {JobConstants.PAYLOAD_JOB_CREATED, definition.Created},
                {JobConstants.PAYLOAD_JOB_MODIFIED, definition.Modified}
            };

            // put all system data
            job.JobDataMap.PutAll(systemData);

            // if there is a payload in job add
            if (definition.Payload != null)
            {
                job.JobDataMap.PutAll(definition.Payload);
            }

            return job;
        }

        /// <summary>
        /// Creates a trigger key
        /// </summary>
        /// <param name="trigger">The trigger definition</param>
        /// <returns></returns>
        private string TriggerKey(TriggerDefinition trigger)
        {
            return string.IsNullOrWhiteSpace(trigger.Name) ? Guid.NewGuid().ToString("n").Substring(0, 8) : trigger.Name;
        }

        /// <summary>
        /// Gets the timezone info 
        /// </summary>
        /// <param name="timezone">The timezone</param>
        /// <returns></returns>
        private TimeZoneInfo GetTimezone(string timezone)
        {
            // no timezone is given
            if (string.IsNullOrWhiteSpace(timezone))
            {
                return null;
            }

            try
            {
                // try find by id
                return TimeZoneInfo.FindSystemTimeZoneById(timezone);
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                // try deserialize
                return TimeZoneInfo.FromSerializedString(timezone);
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        /// <summary>
        /// Creates the set of period triggers if required
        /// </summary>
        /// <param name="jobDefinition">The job definition</param>
        /// <param name="trigger">The trigger definition</param>
        /// <returns></returns>
        protected ISet<ITrigger> PeriodTrigger(JobDefinition jobDefinition, TriggerDefinition trigger)
        {
            // the job triggers
            var result = new HashSet<ITrigger>();

            // period in milliseconds
            var periodMs = trigger.Period;

            // not a valid period
            if (periodMs == null || double.IsNaN(periodMs.Value) || Math.Abs(periodMs.Value) < 0.00000001)
            {
                return result;
            }

            // period in seconds
            var periodSecond = (int) (periodMs.Value / 1000);

            // 0 second should not be considered
            if (periodSecond == 0)
            {
                return null;
            }

            // start building a trigger
            var triggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(this.TriggerKey(trigger), JobOps.Identity(jobDefinition))
                .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(periodSecond));

            // set start time if given
            if (trigger.StartAt != null)
            {
                triggerBuilder.StartAt(trigger.StartAt.Value);
            }

            // set start time if given
            if (trigger.EndAt != null)
            {
                triggerBuilder.EndAt(trigger.EndAt.Value);
            }

            // add ready trigger
            result.Add(triggerBuilder.Build());

            return result;
        }

        /// <summary>
        /// Creates the one-time trigger if required
        /// </summary>
        /// <param name="jobDefinition">The job definition</param>
        /// <param name="trigger">The trigger definition</param>
        /// <returns></returns>
        protected ISet<ITrigger> OneTimeTrigger(JobDefinition jobDefinition, TriggerDefinition trigger)
        {
            // the job triggers
            var result = new HashSet<ITrigger>();

            // start building a trigger
            var triggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(this.TriggerKey(trigger), JobOps.Identity(jobDefinition));

            // set start time if given
            if (trigger.StartAt != null)
            {
                triggerBuilder.StartAt(trigger.StartAt.Value);
            }
            else
            {
                triggerBuilder.StartNow();
            }

            // add ready trigger
            result.Add(triggerBuilder.Build());

            return result;
        }

        /// <summary>
        /// Creates the set of period triggers if required
        /// </summary>
        /// <param name="jobDefinition">The job definition</param>
        /// <param name="trigger">The trigger definition</param>
        /// <returns></returns>
        protected ISet<ITrigger> CronTrigger(JobDefinition jobDefinition, TriggerDefinition trigger)
        {
            // the job triggers
            var result = new HashSet<ITrigger>();

            // the cron expression
            var cronExpression = trigger.Cron;

            // skip if not a valid cron expression
            if (string.IsNullOrWhiteSpace(cronExpression) || !CronExpression.IsValidExpression(cronExpression))
            {
                return result;
            }

            // build the cron schedule
            var schedule = CronScheduleBuilder
                .CronSchedule(cronExpression)
                .InTimeZone(this.GetTimezone(trigger.Timezone));

            // start building a trigger
            var triggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(this.TriggerKey(trigger), JobOps.Identity(jobDefinition))
                .WithSchedule(schedule);

            // set start time if given
            if (trigger.StartAt != null)
            {
                triggerBuilder.StartAt(trigger.StartAt.Value);
            }

            // set start time if given
            if (trigger.EndAt != null)
            {
                triggerBuilder.EndAt(trigger.EndAt.Value);
            }

            // add ready trigger
            result.Add(triggerBuilder.Build());

            return result;
        }

        /// <summary>
        /// Creates the set of triggers for given job definition
        /// </summary>
        /// <param name="jobDefinition">The job definition</param>
        /// <returns></returns>
        public ISet<ITrigger> CreateTriggers(JobDefinition jobDefinition)
        {
            // set of definitions
            var triggers = jobDefinition.Triggers;

            // final trigger set
            var result = new HashSet<ITrigger>();

            // nothing to build
            if (triggers == null || triggers.Count == 0)
            {
                return new HashSet<ITrigger>();
            }

            triggers.ForEach(trigger =>
            {
                // get payload or create it
                var payload = trigger.Payload ?? new Dictionary<string, object>();

                // create set of quartz triggers based on type
                var quartzTriggers = trigger.Type switch
                {
                    TriggerType.STATIC_PERIOD => this.PeriodTrigger(jobDefinition, trigger),
                    TriggerType.CRON => this.CronTrigger(jobDefinition, trigger),
                    TriggerType.ONE_TIME => this.OneTimeTrigger(jobDefinition, trigger),
                    _ => new HashSet<ITrigger>()
                };

                // add payload of trigger to each and every quartz trigger and add to final result set
                foreach (var quartzTrigger in quartzTriggers)
                {
                    quartzTrigger.JobDataMap.PutAll(payload);
                    result.Add(quartzTrigger);
                }
            });

            return result;
        }
    }
}