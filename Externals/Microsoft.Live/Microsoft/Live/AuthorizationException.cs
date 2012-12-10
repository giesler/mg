namespace Microsoft.Live
{
    using System;

    public class AuthorizationException : Exception
    {
        internal AuthorizationException()
        {
        }

        internal AuthorizationException(string message) : base(message)
        {
        }

        internal AuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

