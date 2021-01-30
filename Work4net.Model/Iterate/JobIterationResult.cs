using System;
using System.Collections.Generic;

namespace Work4net.Model.Iterate
{
    /// <summary>
    /// The job iteration result
    /// </summary>
    [Serializable]
    public class JobIterationResult
    {
        /// <summary>
        /// The set of iterations
        /// </summary>
        public List<JobIteration> Results { get; set; }

        /// <summary>
        /// The total number of iterations
        /// </summary>
        public long Total { get; set; }
    }
}