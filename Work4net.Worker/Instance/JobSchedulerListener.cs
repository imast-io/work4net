using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Work4net.Channel;
using Work4net.Common.Log;
using Work4net.Model;

namespace Work4net.Worker.Instance
{
    /// <summary>
    /// The job scheduler listener
    /// </summary>
    public class JobSchedulerListener : ISchedulerListener
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
        /// Creates new instance of every job scheduler listener
        /// </summary>
        /// <param name="schedulerChannel">The scheduler channel</param>
        public JobSchedulerListener(ISchedulerChannel schedulerChannel)
        {
            this.schedulerChannel = schedulerChannel;
        }

        /// <summary>
        /// The job is scheduled indication
        /// </summary>
        /// <param name="trigger">The trigger</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job is unscheduled
        /// </summary>
        /// <param name="triggerKey">The trigger key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The trigger finalized
        /// </summary>
        /// <param name="trigger">The trigger</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            // report completed job as trigger finalized
            var result = this.schedulerChannel.MarkAs(trigger.JobKey.Name, JobStatus.COMPLETED);

            // report if any
            if (result == null)
            {
                log.Warn("JobSchedulerListener: Job completion is not updated");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// The trigger paused
        /// </summary>
        /// <param name="triggerKey">The trigger key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The trigger group paused
        /// </summary>
        /// <param name="triggerGroup">The trigger group</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The trigger resumed
        /// </summary>
        /// <param name="triggerKey">The trigger key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The trigger group resumed
        /// </summary>
        /// <param name="triggerGroup">The trigger group</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job added indication
        /// </summary>
        /// <param name="jobDetail">The job details</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job deleted indication
        /// </summary>
        /// <param name="jobKey">The job key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job paused indication
        /// </summary>
        /// <param name="jobKey">The job key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job interrupted indication
        /// </summary>
        /// <param name="jobKey">The job key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job resumed indication
        /// </summary>
        /// <param name="jobKey">The job key</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job paused indication
        /// </summary>
        /// <param name="jobGroup">The job group</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The job resumed indication
        /// </summary>
        /// <param name="jobGroup">The job group</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The scheduler error indication
        /// </summary>
        /// <param name="msg">The error message</param>
        /// <param name="cause">The error cause</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Scheduler in standby mode
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The scheduler started
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task SchedulerStarted(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The scheduler is starting
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task SchedulerStarting(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The scheduler shuts down
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task SchedulerShutdown(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The scheduler is shutting down
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task SchedulerShuttingdown(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The scheduling data cleared
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task SchedulingDataCleared(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }
    }
}