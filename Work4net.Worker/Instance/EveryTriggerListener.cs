using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Work4net.Channel;

namespace Work4net.Worker.Instance
{
    /// <summary>
    /// The trigger listener
    /// </summary>
    public class EveryTriggerListener : ITriggerListener
    {
        /// <summary>
        /// The scheduler channel instance
        /// </summary>
        protected readonly ISchedulerChannel schedulerChannel;

        /// <summary>
        /// Creates new instance of every trigger listener
        /// </summary>
        /// <param name="schedulerChannel">The scheduler channel</param>
        public EveryTriggerListener(ISchedulerChannel schedulerChannel)
        {
            this.schedulerChannel = schedulerChannel;
        }

        /// <summary>
        /// The name of listener
        /// </summary>
        public string Name => "WORK4NET_TRIGGER_LISTENER";

        /// <summary>
        /// The trigger fired indication
        /// </summary>
        /// <param name="trigger">The trigger</param>
        /// <param name="context">The execution context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Decide whether to veto or not the job trigger instance
        /// </summary>
        /// <param name="trigger">The trigger</param>
        /// <param name="context">The execution context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// The trigger misfired indication
        /// </summary>
        /// <param name="trigger">The trigger</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The trigger completion indication
        /// </summary>
        /// <param name="trigger">The trigger</param>
        /// <param name="context">The execution context</param>
        /// <param name="triggerInstructionCode">The instruction code</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }
    }
}