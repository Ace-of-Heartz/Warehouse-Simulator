using System;

namespace WarehouseSimulator.View
{
    public class MissingVisualElementException : Exception
    {

        public MissingVisualElementException()
        {
            
        }

        public MissingVisualElementException(string message) : base(message)
        {
            
        }
    }
}