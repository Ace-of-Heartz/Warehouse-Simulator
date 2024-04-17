using System;

namespace WarehouseSimulator.Model
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException(string msg) : base(msg) { }
    }
}