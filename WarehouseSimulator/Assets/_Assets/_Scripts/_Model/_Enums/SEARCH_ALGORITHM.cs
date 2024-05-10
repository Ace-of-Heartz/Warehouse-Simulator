namespace WarehouseSimulator.Model.Enums
{
    
    /// <summary>
    /// Search algorithms that can be used for path planning.
    /// </summary>
    public enum SEARCH_ALGORITHM
    {
        BFS,
        A_STAR,
        A_STAR_ASYNC,
        COOP_A_STAR
    }
}