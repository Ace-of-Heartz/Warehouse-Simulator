using System;

namespace WarehouseSimulator.Model
{
    /// <summary>
    /// Custom exception for invalid files
    /// </summary>
    public class InvalidFileException : Exception
    {
        /// <summary>
        /// Yes this is a constructor
        /// </summary>
        /// <param name="msg">The error message</param>
        public InvalidFileException(string msg) : base(msg) { }
    }
}