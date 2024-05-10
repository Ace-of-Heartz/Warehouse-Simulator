namespace WarehouseSimulator.Model.Enums
{
    
    /// <summary>
    /// Search algorithms that can be used for path planning.
    /// </summary>
    public enum SearchAlgorithm
    {
        BFS,
        AStar,
        AStarAsync,
        CoopAStar
    }
}