using System.Collections.Generic;
using Work4net.Model.Agent;

namespace Work4net.Data
{
    /// <summary>
    /// The agent definition repository interface
    /// </summary>
    public interface IAgentDefinitionRepository
    {
        /// <summary>
        /// Prepare repository preconditions (schema, indexes if needed, other data)
        /// </summary>
        /// <returns>Returns if ready</returns>
        bool Prepare();

        /// <summary>
        /// Gets agent definition by id or null
        /// </summary>
        /// <param name="id">The id of agent</param>
        /// <returns></returns>
        AgentDefinition GetById(string id);

        /// <summary>
        /// Gets all agent definitions available
        /// </summary>
        /// <returns></returns>
        List<AgentDefinition> GetAll();

        /// <summary>
        /// Inserts agent definition into the repository
        /// </summary>
        /// <param name="agent">The agent to insert</param>
        /// <returns></returns>
        AgentDefinition Insert(AgentDefinition agent);

        /// <summary>
        /// Updates agent definition in the repository
        /// </summary>
        /// <param name="agent">The agent to update</param>
        /// <returns></returns>
        AgentDefinition Update(AgentDefinition agent);

        /// <summary>
        /// Deletes agent definition by id
        /// </summary>
        /// <param name="id">The identifier to delete</param>
        /// <returns></returns>
        AgentDefinition DeleteById(string id);

        /// <summary>
        /// Deletes all agent definitions
        /// </summary>
        /// <returns>Returns number of deleted items</returns>
        long DeleteAll();

    }
}
