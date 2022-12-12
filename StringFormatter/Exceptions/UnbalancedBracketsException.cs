using System.Runtime.Serialization;

namespace StringFormatter.Exceptions
{
    public class UnbalancedBracketsException : Exception
    {
        public UnbalancedBracketsException()
        {
        }

        public UnbalancedBracketsException(string? message) : base(message)
        {
        }

        public UnbalancedBracketsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnbalancedBracketsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
