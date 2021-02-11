using System.Collections.Generic;

namespace Work4net.Execution
{
    /// <summary>
    /// The job executor context interface to access existing context
    /// </summary>
    public interface IJobExecutorContext
    {
        /// <summary>
        /// Gets the job code
        /// </summary>
        /// <returns></returns>
        string GetCode();

        /// <summary>
        /// Gets the job group
        /// </summary>
        /// <returns></returns>
        string GetGroup();

        /// <summary>
        /// Gets the job type
        /// </summary>
        /// <returns></returns>
        string GetJobType();

        /// <summary>
        /// Gets registered module with given key and type
        /// </summary>
        /// <typeparam name="T">The module type</typeparam>
        /// <param name="key">The key of module</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        T GetModuleOr<T>(string key, T defaultValue) where T : class;

        /// <summary>
        /// Gets job value with given key and type
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="key">The key of value</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        T GetJobValueOr<T>(string key, T defaultValue);

        /// <summary>
        /// Gets trigger value with given key and type
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="key">The key of value</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        T GetTriggerValueOr<T>(string key, T defaultValue);

        /// <summary>
        /// Gets value (lookup in trigger, then in job) with given key and type
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="key">The key of value</param>
        /// <param name="defaultValue">The default value if does not exist</param>
        /// <returns></returns>
        T GetValue<T>(string key, T defaultValue);

        /// <summary>
        /// Sets the output payload
        /// </summary>
        /// <param name="payload">The payload of output</param>
        void SetOutput(IDictionary<string, object> payload);
    }
}