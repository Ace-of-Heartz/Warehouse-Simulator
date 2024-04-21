using System;
using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model
{
    public static class ConfigIO
    {
        
        public static SimulationConfig ParseFromJson(string jsonContent)
        {
            SimulationConfig simConfig;
            try
            {
                simConfig = JsonUtility.FromJson<SimulationConfig>(jsonContent);
            }
            catch (Exception)
            {
                Debug.Log("Fatal error occured at JSON parsing!");
                throw;
            }
            return simConfig;
        }

        public static string GetJsonContent(string path)
        {
            using StreamReader reader = new(path);
            string json;
            try {
                json = reader.ReadToEnd();
            }
            catch (Exception)
            {
                Debug.Log("Fatal error occured at JSON parsing!");
                throw;
            }
            return json;
        }
    }
    
}
