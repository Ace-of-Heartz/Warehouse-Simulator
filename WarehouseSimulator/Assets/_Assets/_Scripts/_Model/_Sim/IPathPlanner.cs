using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using Vector2 = System.Numerics.Vector2;

namespace WarehouseSimulator.Model.Sim
{
    public interface IPathPlanner
    {
        // ReSharper restore Unity.ExpensiveCode
        public Task<List<RobotDoing>> GetPath(Vector2Int start, Vector2Int finish,Direction dir);
        
        
    }
}