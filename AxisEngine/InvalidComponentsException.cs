using System;

namespace AxisEngine
{
    class InvalidComponentsException : Exception
    {
        public InvalidComponentsException() : base() { }
        public InvalidComponentsException(string message) : base(message) { }
        public InvalidComponentsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
