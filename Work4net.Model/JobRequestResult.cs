using System.Collections.Generic;

namespace Work4net.Model
{
    /// <summary>
    /// The job request result
    /// </summary>
    public class JobRequestResult
    {
        /// <summary>
        /// The set of result jobs
        /// </summary>
        public List<JobDefinition> Jobs { get; set; }

        /// <summary>
        /// The number of jobs in total
        /// </summary>
        public long Total { get; set; }
    }
}