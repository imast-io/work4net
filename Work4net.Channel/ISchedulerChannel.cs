using Work4net.Model;
using Work4net.Model.Agent;
using Work4net.Model.Exchange;
using Work4net.Model.Iterate;

namespace Work4net.Channel
{
    /// <summary>
    /// The scheduler communication channel
    /// </summary>
    public interface ISchedulerChannel
    {
        /// <summary>
        /// Pull the scheduler job metadata
        /// </summary>
        /// <param name="request">The metadata request</param>
        /// <returns></returns>
        JobMetadataResponse Metadata(JobMetadataRequest request);

        /// <summary>
        /// Exchange current state with modified entries
        /// </summary>
        /// <param name="status">The status request</param>
        /// <returns></returns>
        JobStatusExchangeResponse StatusExchange(JobStatusExchangeRequest status);

        /// <summary>
        /// Adds iteration information to scheduler
        /// </summary>
        /// <param name="iteration">The iteration to add</param>
        /// <returns></returns>
        JobIteration Iterate(JobIteration iteration);
        
        /// <summary>
        /// Change job status to the given one
        /// </summary>
        /// <param name="id">The identifier of the job</param>
        /// <param name="status">The status of job definition</param>
        /// <returns></returns>
        JobDefinition MarkAs(string id, JobStatus status);

        /// <summary>
        /// Registers agent definition into the system
        /// </summary>
        /// <param name="agent">The agent to register</param>
        /// <returns></returns>
        AgentDefinition Registration(AgentDefinition agent);

        /// <summary>
        /// Send a heartbeat signal to scheduler for agent
        /// </summary>
        /// <param name="id">The agent identifier</param>
        /// <param name="health">The new health of agent</param>
        /// <returns></returns>
        AgentDefinition Heartbeat(string id, AgentHealth health);
    }
}