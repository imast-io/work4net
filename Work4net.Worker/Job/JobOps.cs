using System;
using System.Collections.Generic;
using Quartz;
using Work4net.Execution;
using Work4net.Model;

namespace Work4net.Worker.Job
{
    /// <summary>
    /// The job operations
    /// </summary>
    public class JobOps
    {
        /// <summary>
        /// Gets value from data map
        /// </summary>
        /// <typeparam name="T">The type of data type</typeparam>
        /// <param name="data">The data source</param>
        /// <param name="key">The key of value</param>
        /// <returns></returns>
        public static T GetValue<T>(JobDataMap data, string key)
        {
            // try get value from data
            if (!data.TryGetValue(key, out var value))
            {
                return default;
            }

            // cast if alright
            if (value is T castValue)
            {
                return castValue;
            }

            return default;
        }

        /// <summary>
        /// Gets value from data map
        /// </summary>
        /// <typeparam name="T">The type of data type</typeparam>
        /// <param name="data">The data source</param>
        /// <param name="key">The key of value</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns></returns>
        public static T GetValueOr<T>(JobDataMap data, string key, T defaultValue)
        {
            // try get value from data
            if (!data.TryGetValue(key, out var value))
            {
                return defaultValue;
            }

            // cast if alright
            if (value is T castValue)
            {
                return castValue;
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the context module with the given key
        /// </summary>
        /// <typeparam name="T">The type of module</typeparam>
        /// <param name="key">The module key</param>
        /// <param name="type">The job type</param>
        /// <param name="executionContext">The execution context</param>
        /// <returns></returns>
        public static T GetContextModule<T>(string key, string type, IJobExecutionContext executionContext) where T : class
        {
            // key is not value
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            // try get context
            var context = executionContext?.Scheduler.Context;

            // check if context is absent
            if (context == null)
            {
                return null;
            }

            // no module dictionary is registered
            if (!context.TryGetValue(JobConstants.JOB_MODULES, out var modules))
            {
                return null;
            }

            // cast to correct type
            var jobModules = modules as IDictionary<string, IDictionary<string, object>>;

            // not correct type or type is not defined
            if (jobModules == null || !jobModules.TryGetValue(type, out var typeModules))
            {
                return null;
            }

            // no module defined in type modules
            if (typeModules == null || !typeModules.TryGetValue(key, out var module))
            {
                return null;
            }

            return module as T;
        }

        /// <summary>
        /// Gets the executor supplier with the given type
        /// </summary>
        /// <param name="type">The type of job</param>
        /// <param name="executionContext">The job execution context</param>
        /// <returns></returns>
        public static Func<IJobExecutorContext, IJobExecutor> GetExecutor(string type, IJobExecutionContext executionContext)
        {
            // nothing to do
            if (string.IsNullOrWhiteSpace(type))
            {
                return null;
            }

            // the scheduler context
            var context = executionContext.Scheduler.Context;

            // no worker factory 
            if (!context.TryGetValue(JobConstants.WORKER_FACTORY, out var factory))
            {
                return null;
            }

            // cast to correct type
            var workerFactory = factory as WorkerFactory;

            // get the executor if available
            return workerFactory?.GetExecutor(type);
        }

        /// <summary>
        /// The job definition identity
        /// </summary>
        /// <param name="definition">The definition</param>
        /// <returns></returns>
        public static string Identity(JobDefinition definition)
        {
            return $"{definition.Code}:{definition.Group}";
        }
    }
}