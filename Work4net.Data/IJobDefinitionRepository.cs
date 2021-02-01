using System.Collections.Generic;
using Work4net.Model;

namespace Work4net.Data
{
    /// <summary>
    /// The job definitions repository
    /// </summary>
    public interface IJobDefinitionRepository
    {
        /// <summary>
        /// Prepare repository preconditions (schema, indexes if needed, other data)
        /// </summary>
        /// <returns>Returns if ready</returns>
        bool Prepare();

        /// <summary>
        /// Gets job definition by id or null
        /// </summary>
        /// <param name="id">The id of job</param>
        /// <returns></returns>
        JobDefinition GetById(string id);

        /// <summary>
        /// Gets all job definitions available
        /// </summary>
        /// <returns></returns>
        List<JobDefinition> GetAll();

        /// <summary>
        /// Gets page of job definitions ordered by code 
        /// </summary>
        /// <param name="page">The page number</param>
        /// <param name="size">The page size</param>
        /// <returns></returns>
        JobRequestResult GetPageByCode(int page, int size);

        /// <summary>
        /// Gets all job definitions by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<JobDefinition> GetByType(string type);

        /// <summary>
        /// Gets job definitions by status in given set
        /// </summary>
        /// <param name="type">The type to filter</param>
        /// <param name="group">The group to filter</param>
        /// <param name="cluster">The cluster to filter</param>
        /// <param name="statuses">The statuses to filter</param>
        /// <returns></returns>
        List<JobDefinition> GetByStatusIn(string type, string group, string cluster, List<JobStatus> statuses);

        /// <summary>
        /// Gets job definitions by status not in given set
        /// </summary>
        /// <param name="type">The type to filter</param>
        /// <param name="group">The group to filter</param>
        /// <param name="cluster">The cluster to filter</param>
        /// <param name="statuses">The statuses to filter</param>
        /// <returns></returns>
        List<JobDefinition> GetByStatusNotIn(string type, string group, string cluster, List<JobStatus> statuses);

        /// <summary>
        /// Gets all job types in cluster
        /// </summary>
        /// <param name="cluster">The target cluster</param>
        /// <returns></returns>
        List<string> GetAllTypes(string cluster);

        /// <summary>
        /// Gets all job groups in cluster
        /// </summary>
        /// <param name="cluster">The target cluster</param>
        /// <returns></returns>
        List<string> GetAllGroups(string cluster);

        /// <summary>
        /// Inserts job definition into the repository
        /// </summary>
        /// <param name="job">The job to insert</param>
        /// <returns></returns>
        JobDefinition Insert(JobDefinition job);

        /// <summary>
        /// Updates job definition in the repository
        /// </summary>
        /// <param name="job">The job to update</param>
        /// <returns></returns>
        JobDefinition Update(JobDefinition job);

        /// <summary>
        /// Deletes job definition by id
        /// </summary>
        /// <param name="id">The identifier to delete</param>
        /// <returns></returns>
        JobDefinition DeleteById(string id);

        /// <summary>
        /// Deletes all job definitions
        /// </summary>
        /// <returns>Returns number of deleted items</returns>
        long DeleteAll();
    }
}