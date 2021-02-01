using System;
using System.Collections.Generic;
using System.Linq;
using Work4net.Channel;
using Work4net.Channel.Notify;
using Work4net.Data;
using Work4net.Model;
using Work4net.Model.Agent;
using Work4net.Model.Exchange;
using Work4net.Model.Iterate;

namespace Work4net.Controller
{
    /// <summary>
    /// The scheduler controller 
    /// </summary>
    public class SchedulerController : ISchedulerChannel
    {
        /// <summary>
        /// The job definitions repository
        /// </summary>
        protected readonly IJobDefinitionRepository definitions;

        /// <summary>
        /// The job iterations repository
        /// </summary>
        protected readonly IJobIterationRepository iterations;

        /// <summary>
        /// The agent definitions repository
        /// </summary>
        protected readonly IAgentDefinitionRepository agents;

        /// <summary>
        /// The set of worker publishers
        /// </summary>
        protected readonly List<IWorkerPublisher> publishers;

        /// <summary>
        /// Creates new instance of scheduler controller
        /// </summary>
        /// <param name="definitions">The job definitions repository</param>
        /// <param name="iterations">The job iterations repository</param>
        /// <param name="agents">The agent definitions repository</param>
        /// <param name="publishers">The set of worker publishers</param>
        public SchedulerController(IJobDefinitionRepository definitions, IJobIterationRepository iterations, IAgentDefinitionRepository agents, List<IWorkerPublisher> publishers)
        {
            this.definitions = definitions;
            this.iterations = iterations;
            this.agents = agents;
            this.publishers = publishers;
        }

        /// <summary>
        /// Initializes current instance of controller
        /// </summary>
        /// <returns></returns>
        public SchedulerController Initialize()
        {
            // prepare agents
            this.agents.Prepare();

            // prepare jobs
            this.definitions.Prepare();

            // prepare iterations
            this.iterations.Prepare();

            return this;
        }

        /// <summary>
        /// Pull the scheduler job metadata
        /// </summary>
        /// <param name="request">The metadata request</param>
        /// <returns></returns>
        public virtual JobMetadataResponse Metadata(JobMetadataRequest request)
        {
            // the queried cluster
            var cluster = request.Cluster;

            // gets all groups
            var groups = this.definitions.GetAllGroups(cluster);

            // get all types
            var types = this.definitions.GetAllTypes(cluster);

            return new JobMetadataResponse
            {
                Cluster = cluster,
                Groups = groups,
                Types = types
            };
        }

        /// <summary>
        /// Exchange current state with modified entries
        /// </summary>
        /// <param name="status">The status request</param>
        /// <returns></returns>
        public virtual JobStatusExchangeResponse StatusExchange(JobStatusExchangeRequest status)
        {
            // get all active jobs
            var all = this.GetAllActive(status.Group, status.Type, status.Cluster).Jobs;

            // all unique keys
            var allKeys = all.Select(j => j.Code).ToHashSet();

            // all the new jobs
            var newJobs = new Dictionary<string, JobDefinition>();

            // all the updated jobs
            var updatedJobs = new Dictionary<string, JobDefinition>();

            // evaluate all the active jobs to decide if new or updated
            foreach (var job in all)
            {
                // try get existing job status
                var exists = status.State.TryGetValue(job.Code, out var existing);

                // check if does not exist in current state, then consider as new
                if (!exists)
                {
                    newJobs[job.Code] = job;
                    continue;
                }

                // if job modification time is still the same consider as non-modified
                if (existing.ToUniversalTime().Equals(job.Modified))
                {
                    continue;
                }

                // we have fresh version of the existing job
                if (job.Modified.ToUniversalTime() > existing.ToUniversalTime())
                {
                    updatedJobs[job.Code] = job;
                }
            }

            // get all keys that are present in current state but not in "all active jobs" list and consider as deleted
            var deleted = status.State.Keys.Where(statusJob => !allKeys.Contains(statusJob)).ToList();

            // construct and return result
            return new JobStatusExchangeResponse
            {
                Group = status.Group,
                Type = status.Type,
                Added = newJobs,
                Updated = updatedJobs,
                Removed = deleted
            };
        }

        /// <summary>
        /// Gets all jobs by type if given
        /// </summary>
        /// <param name="type">The type of job</param>
        /// <returns></returns>
        public List<JobDefinition> GetAllJobs(string type)
        {
            return string.IsNullOrWhiteSpace(type) ? this.definitions.GetAll() : this.definitions.GetByType(type);
        }

        /// <summary>
        /// Gets job definitions page
        /// </summary>
        /// <param name="page">The page number</param>
        /// <param name="size">The page size</param>
        /// <returns></returns>
        public JobRequestResult GetJobsPage(int page, int size)
        {
            return this.definitions.GetPageByCode(page, size);
        }

        /// <summary>
        /// Gets all the active job definitions
        /// </summary>
        /// <param name="group">The target group</param>
        /// <param name="type">The target type</param>
        /// <param name="cluster">The target cluster</param>
        /// <returns></returns>
        protected JobRequestResult GetAllActive(string group, string type, string cluster)
        {
            // get all active jobs in cluster
            var jobs = this.definitions.GetByStatusIn(type, group, cluster, new List<JobStatus> { JobStatus.ACTIVE });

            return new JobRequestResult
            {
                Jobs = jobs,
                Total = jobs.Count
            };
        }

        /// <summary>
        /// Gets job definition by id
        /// </summary>
        /// <param name="id">The id of job</param>
        /// <returns></returns>
        public JobDefinition GetJob(string id)
        {
            return this.definitions.GetById(id);
        }

        /// <summary>
        /// Adds the job definition to scheduler
        /// </summary>
        /// <param name="definition">The definition to add</param>
        /// <returns></returns>
        public JobDefinition AddJob(JobDefinition definition)
        {
            // add job to storage
            var inserted = this.definitions.Insert(definition);

            // if inserted publish change
            if (inserted != null)
            {
                this.PublishWorkerUpdate(new WorkerUpdateMessage
                {
                    Operation = UpdateOperation.ADD,
                    Code = inserted.Code,
                    Group = inserted.Group,
                    Definition = inserted
                });
            }

            return inserted;
        }

        /// <summary>
        /// Change job status to the given one
        /// </summary>
        /// <param name="id">The identifier of the job</param>
        /// <param name="status">The status of job definition</param>
        /// <returns></returns>
        public virtual JobDefinition MarkAs(string id, JobStatus status)
        {
            // get job if exist
            var existing = this.GetJob(id);

            // check if exit
            if (existing == null)
            {
                return null;
            }

            // clone to update
            var clone = existing.ShallowClone();

            // update status and save
            clone.Status = status;

            return this.UpdateJob(clone);
        }
        

        /// <summary>
        /// Updates existing job definition
        /// </summary>
        /// <param name="definition">The definition to save</param>
        /// <returns></returns>
        public virtual JobDefinition UpdateJob(JobDefinition definition)
        {
            // get existing
            var existing = this.GetJob(definition.Id);

            // make sure exists
            if (existing == null)
            {
                return null;
            }

            // update and get result
            var updated = this.definitions.Update(definition);

            // not updated properly
            if (updated == null)
            {
                return null;
            }

            // previous status of job before saving
            var prevStatus = existing.Status;

            // the status after saving
            var newStatus = updated.Status;

            // if was inactive and remains inactive do nothing
            if (prevStatus != JobStatus.ACTIVE && newStatus != JobStatus.ACTIVE)
            {
                return updated;
            }

            // if remains active (other fields are updated)
            if (prevStatus == JobStatus.ACTIVE && newStatus == JobStatus.ACTIVE)
            {
                this.PublishWorkerUpdate(new WorkerUpdateMessage
                {
                    Operation = UpdateOperation.UPDATE,
                    Code = updated.Code,
                    Group = updated.Group,
                    Definition = updated
                });
            }

            // if job was active and became inactive
            if (prevStatus == JobStatus.ACTIVE && newStatus != JobStatus.ACTIVE)
            {
                this.PublishWorkerUpdate(new WorkerUpdateMessage
                {
                    Operation = UpdateOperation.REMOVE,
                    Code = updated.Code,
                    Group = updated.Group,
                    Definition = null
                });
            }

            // if job was active and became inactive 
            if (prevStatus != JobStatus.ACTIVE && newStatus == JobStatus.ACTIVE)
            {
                this.PublishWorkerUpdate(new WorkerUpdateMessage
                {
                    Operation = UpdateOperation.ADD,
                    Code = updated.Code,
                    Group = updated.Group,
                    Definition = updated
                });
            }

            return updated;
        }

        /// <summary>
        /// Deletes job from scheduler
        /// </summary>
        /// <param name="id">The job identifier</param>
        /// <returns></returns>
        public virtual JobDefinition DeleteJob(string id)
        {
            // deleted if exists
            var deleted = this.definitions.DeleteById(id);

            // if deleted publish change
            if (deleted != null)
            {
                this.PublishWorkerUpdate(new WorkerUpdateMessage
                {
                    Operation = UpdateOperation.REMOVE,
                    Code = deleted.Code,
                    Group = deleted.Group,
                });
            }

            return deleted;
        }

        /// <summary>
        /// Gets job iterations page for the job target job id
        /// </summary>
        /// <param name="jobId">The job identifier</param>
        /// <param name="status">The job status to filter</param>
        /// <param name="page">The page number</param>
        /// <param name="size">The size of page</param>
        /// <returns></returns>
        public virtual JobIterationResult GetIterations(string jobId, IterationStatus? status, int page, int size)
        {
            // the status filter
            var statuses = status.HasValue ? new List<IterationStatus> {status.Value} : null;

            return this.iterations.GetPageByTimestamp(jobId, statuses, page, size);
        }

        /// <summary>
        /// Adds iteration information to scheduler
        /// </summary>
        /// <param name="iteration">The iteration to add</param>
        /// <returns></returns>
        public virtual JobIteration Iterate(JobIteration iteration)
        {
            return this.iterations.Insert(iteration);
        }

        /// <summary>
        /// Cleanup job iterations before the specified timestamp
        /// </summary>
        /// <param name="upper">The upper bound to cleanup</param>
        /// <returns></returns>
        public virtual long CleanupIterations(DateTime upper)
        {
            return this.iterations.DeleteBefore(upper);
        }

        /// <summary>
        /// Gets all agent definitions
        /// </summary>
        /// <returns></returns>
        public virtual List<AgentDefinition> GetAgents()
        {
            return this.agents.GetAll();
        }

        /// <summary>
        /// Gets agent definition by id
        /// </summary>
        /// <param name="id">The agent identifier</param>
        /// <returns></returns>
        public AgentDefinition GetAgent(string id)
        {
            return this.agents.GetById(id);
        }

        /// <summary>
        /// Registers agent definition into the system
        /// </summary>
        /// <param name="agent">The agent to register</param>
        /// <returns></returns>
        public virtual AgentDefinition Registration(AgentDefinition agent)
        {
            return this.agents.Insert(agent);
        }

        /// <summary>
        /// Send a heartbeat signal to scheduler for agent
        /// </summary>
        /// <param name="id">The agent identifier</param>
        /// <param name="health">The new health of agent</param>
        /// <returns></returns>
        public AgentDefinition Heartbeat(string id, AgentHealth health)
        {
            // try get existing agent
            var existing = this.GetAgent(id);

            // nothing to do
            if (existing == null)
            {
                return null;
            }

            // clone agent
            var clone = existing.ShallowClone();

            // update health
            clone.Health = health;

            return this.agents.Update(clone);
        }

        /// <summary>
        /// Deletes agent definition by id
        /// </summary>
        /// <param name="id">The agent identifier</param>
        /// <returns></returns>
        public virtual AgentDefinition DeleteAgent(string id)
        {
            return this.agents.DeleteById(id);
        }

        /// <summary>
        /// Publish message to subscribed worker
        /// </summary>
        /// <param name="message">The message to publish</param>
        protected virtual void PublishWorkerUpdate(WorkerUpdateMessage message)
        {
            foreach (var publisher in this.publishers)
            {
                publisher.Publish(message);
            }
        }
    }
}
