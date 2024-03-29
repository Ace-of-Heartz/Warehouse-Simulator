using UnityEngine;
using WarehouseSimulator.Model.Sim;
using WarehouseSimulator.View.MainMenu;

namespace WarehouseSimulator.View.Sim
{
    public class UnitySimulationManager : MonoBehaviour
    {
        [SerializeField]
        private UnityMap unityMap;
    
        private SimulationManager simulationManager;
        
        void Start()
        {
            simulationManager = new SimulationManager();
            simulationManager.Setup(MainMenuManager.simInputArgs);

            unityMap.AssignMap(simulationManager.Map);
            unityMap.GenerateMap();
        }

        void Update()
        {
        
        }
    }   
}
