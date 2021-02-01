using System;
using System.Collections.Generic;
using Work4net.Model.Iterate;

namespace Work4net.Data
{
    /// <summary>
    /// The job iterations repository
    /// </summary>
    public interface IJobIterationRepository
    {
        /// <summary>
        /// Prepare repository preconditions (schema, indexes if needed, other data)
        /// </summary>
        /// <returns>Returns if ready</returns>
        bool Prepare();

        /// <summary>
        /// Gets job iterations by id or null
        /// </summary>
        /// <param name="id">The id of iteration</param>
        /// <returns></returns>
        JobIteration GetById(string id);

        /// <summary>
        /// Gets all job iterations available
        /// </summary>
        /// <returns></returns>
        List<JobIteration> GetAll();

        /// <summary>
        /// Gets page of job iterations ordered by timestamp with filters
        /// </summary>
        /// <param name="jobId">The target job id</param>
        /// <param name="statuses">The target statuses</param>
        /// <param name="page">The page number</param>
        /// <param name="size">The page size</param>
        /// <returns></returns>
        JobIterationResult GetPageByTimestamp(string jobId, List<IterationStatus> statuses, int page, int size);

        /// <summary>
        /// Inserts job definition into the repository
        /// </summary>
        /// <param name="iteration">The iteration to insert</param>
        /// <returns></returns>
        JobIteration Insert(JobIteration iteration);

        /// <summary>
        /// Updates job iteration in the repository
        /// </summary>
        /// <param name="iteration">The iteration to update</param>
        /// <returns></returns>
        JobIteration Update(JobIteration iteration);

        /// <summary>
        /// Deletes job iteration by id
        /// </summary>
        /// <param name="id">The iteration to delete</param>
        /// <returns></returns>
        JobIteration DeleteById(string id);

        /// <summary>
        /// Deletes all job iterations
        /// </summary>
        /// <returns>Returns number of deleted items</returns>
        long DeleteAll();

        /// <summary>
        /// Deletes all job iterations before given time
        /// </summary>
        /// <param name="timestamp">The upper bound to delete</param>
        /// <returns>Returns number of deleted items</returns>
        long DeleteBefore(DateTime timestamp);
    }
}