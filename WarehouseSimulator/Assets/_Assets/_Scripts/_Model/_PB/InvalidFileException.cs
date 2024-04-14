using System;

namespace WarehouseSimulator.Model.PB
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException(string msg) : base(msg) { }
    }
}