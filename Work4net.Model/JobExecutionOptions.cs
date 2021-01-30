using System;

namespace Work4net.Model
{
    /// <summary>
    /// The execution options 
    /// </summary>
    [Serializable]
    public class JobExecutionOptions
    {
        /// <summary>
        /// The option controls reporting iteration results to controller.
        /// In case of silent reporting the iteration success/failure will not be reported.
        /// </summary>
        public bool SilentIterations { get; set; }
    }
}