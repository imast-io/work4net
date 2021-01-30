using System;

namespace Work4net.Model.Iterate
{
    /// <summary>
    /// The iteration status
    /// </summary>
    [Serializable]
    public enum IterationStatus
    {
        /// <summary>
        /// The iteration success indicator
        /// </summary>
        SUCCESS,

        /// <summary>
        /// The iteration failure indicator
        /// </summary>
        FAILURE
    }
}