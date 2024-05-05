using System;

namespace WarehouseSimulator.Model
{
    public class InvalidFileException : Exception
    {
        /// <summary>
        /// Yes this is a constructor
        /// </summary>
        /// <param name="msg">The error message</param>
        public InvalidFileException(string msg) : base(msg) { }
    }
}