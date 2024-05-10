using System;
using System.IO;
using UnityEngine;
using WarehouseSimulator.Model.Sim;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Helper class for loading simulation configuration from a JSON file.
    /// </summary>
    public static class ConfigIO
    {
        /// <summary>
        /// Loads the simulation configuration from a JSON format string.
        /// </summary>
        /// <param name="jsonContent">The json string</param>
        /// <returns>A SimulationConfig object parsed from the JSON string.</returns>
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

        /// <summary>
        /// Loads the text from a file. Should better be called GetTextFromFile, but it is what it is.
        /// </summary>
        /// <param name="path">The path to the text file</param>
        /// <returns>The contents of the file as a string</returns>
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
