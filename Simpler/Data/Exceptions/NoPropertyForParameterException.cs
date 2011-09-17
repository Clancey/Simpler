using System;

namespace Simpler.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when column to build a parameter is missing.
    /// </summary>
    public class NoPropertyForParameterException : Exception
    {
        public NoPropertyForParameterException(string parameterName, string className)
            : base(String.Format("The CommandText contains parameter '{0}' that is not a property of the '{1}' class.", parameterName, className)) { }
    }
}
