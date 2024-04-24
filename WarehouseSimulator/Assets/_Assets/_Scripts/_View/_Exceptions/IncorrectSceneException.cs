using System;

namespace WarehouseSimulator.View
{
    public class IncorrectSceneException : Exception
    {


        public IncorrectSceneException()
        {
            
        }

        public IncorrectSceneException(string message) : base(message)
        {
            
        }
    }
}