using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace _Assets._Scripts._View._MainMenu
{
    public struct PbInputArgs
    {
        [CanBeNull] public string ConfigFilePath { get; set; }
        
        [CanBeNull] public string EventLogPath { get; set; }

        public bool IsComplete()
        {
            return !(string.IsNullOrEmpty(ConfigFilePath) || string.IsNullOrEmpty(EventLogPath));
        }
    }
}