using System;
using System.Runtime.Serialization;

namespace AdvancedStringHandlingInCSharp.Assoc
{
    [Serializable]
    public class AssocArrayException : Exception
    {
        public AssocArrayException(string message)
            : base(message)
        {
        }

        protected AssocArrayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}