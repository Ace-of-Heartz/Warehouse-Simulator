using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WarehouseSimulator.Model.Enums;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model.Sim
{
    public class BFS_PathPlanner : IPathPlanner
    {
        private Map m_map;
        private Dictionary<(Vector2Int,Direction),((Vector2Int,Direction),RobotDoing)> m_pathDict;

        private Queue<(Vector2Int,Direction)> m_queue;
        
        public BFS_PathPlanner(Map map)
        {
            m_map = map;
        } 
            
        public async Task<List<RobotDoing>> GetPath(Vector2Int start, Vector2Int finish, Direction dir)
        {

            return GetInstructions(start,finish,dir);
        }

        private List<RobotDoing> GetInstructions(Vector2Int start, Vector2Int finish, Direction facing)
        {
            bool is_finish_found = false;
            
            List<RobotDoing> instructions = new();
            m_queue = new();
            m_queue.Enqueue((start,facing));

            (var currentNode,var currentDir) = (Vector2Int.zero, Direction.North);
            //Find finish
            while (m_queue.Count > 0)
            {
                (currentNode,currentDir) = m_queue.Dequeue();
                //TODO: Dict stuff
                if (currentNode == finish)
                {
                    is_finish_found = true;
                    break;
                }
                foreach((var node,var dir) in GetNeighbouringNodes(currentNode,currentDir))
                {
                    m_queue.Enqueue((node, dir));
                }
            }

            if (!is_finish_found)
            {
                return instructions; //Could not find finish -> don't do anything
            }
            
            //Traceback path
            

            return instructions;
        }

        private IEnumerable<(Vector2Int, Direction)> GetNeighbouringNodes(Vector2Int currentNode, Direction facing)
        {
            (Vector2Int forwardNode,Direction leftNode,Direction rightNode)  = GetNextNodes(currentNode, facing);
            yield return (forwardNode, facing);
            yield return (currentNode, leftNode);
            yield return (currentNode, rightNode);
        }

        private (Vector2Int,Direction, Direction) GetNextNodes(Vector2Int currentNode, Direction facing)
        {
            Vector2Int forwardNode;
            Direction leftNode, rightNode;
            
            switch (facing)
            {
                case Direction.North:
                    forwardNode = currentNode + Vector2Int.up;
                    leftNode    = Direction.West;
                    rightNode   = Direction.East;
                    break;
                case Direction.South:
                    forwardNode = currentNode + Vector2Int.down;
                    leftNode    = Direction.East;
                    rightNode   = Direction.West;
                    break;    
                case Direction.East:
                    forwardNode = currentNode + Vector2Int.left;
                    leftNode    = Direction.North;
                    rightNode   = Direction.South;
                    break;
                case Direction.West:
                    forwardNode = currentNode + Vector2Int.right;
                    leftNode    = Direction.South;
                    rightNode   = Direction.North;
                    break;
                default:
                    forwardNode = currentNode;
                    leftNode    = facing;
                    rightNode   = facing;
                    break;
            }

            return (forwardNode,leftNode,rightNode);
        }
        




    }
}