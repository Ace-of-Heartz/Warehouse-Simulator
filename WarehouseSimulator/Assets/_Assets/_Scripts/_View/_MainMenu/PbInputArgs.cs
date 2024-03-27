using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace _Assets._Scripts._View._MainMenu
{
    public struct PbInputArgs
    {
        public string ConfigFilePath;

        public string EventLogPath;

        public bool IsComplete()
        {
            return !(string.IsNullOrEmpty(ConfigFilePath) || string.IsNullOrEmpty(EventLogPath));
        }
    }
}