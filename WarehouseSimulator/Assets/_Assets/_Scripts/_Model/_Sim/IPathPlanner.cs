using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using Vector2 = System.Numerics.Vector2;

namespace WarehouseSimulator.Model.Sim
{
    
    /// <summary>
    /// Interface for path planners
    /// Implementations should be able to calculate the shortest path of all robots to their goals
    /// </summary>
    public interface IPathPlanner
    {
        // ReSharper restore Unity.ExpensiveCode
        /// <summary>
        /// Gets the shortest path from a starting position to a finish position, with the initial direction in mind.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="dir"></param>
        /// <param name="disallowedPosition"></param>
        /// <returns></returns>
        Dictionary<SimRobot,RobotDoing> GetNextSteps(List<SimRobot> robots);
        /// <summary>
        /// Set map for path planner
        /// </summary>
        /// <param name="map"></param>
        void SetMap(Map map);
    }
}