using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using WarehouseSimulator.Model.IO;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model
{
    public class ConfigIO
    {
        
        [SerializeField]
        async public Task<SimulationConfig> ParseFromJson(string jsonContent)
        {
            SimulationConfig simConfig = new SimulationConfig();
            try
            {
                await Task.Run(() => simConfig = JsonUtility.FromJson<SimulationConfig>(jsonContent));
            }
            catch (Exception e)
            {
                Debug.Log("Fatal error occured at JSON parsing!");
                throw e;
            }
            
            
            return simConfig;
        }

        public void ParseToJson(string path)
        {

        }
    }
    
}
