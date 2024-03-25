using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using WarehouseSimulator.Model.Sim;
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

        public async Task<string> GetJsonContent(string path)
        {
            StringBuilder buffer = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    buffer.EnsureCapacity((int)sr.BaseStream.Length);
                    buffer.Append(await sr.ReadToEndAsync());
                }
            }
            catch (Exception e)
            {
                Debug.Log("Fatal error occured at JSON reading!");
                
            }

            return buffer.ToString();
        }
    }
    
}
