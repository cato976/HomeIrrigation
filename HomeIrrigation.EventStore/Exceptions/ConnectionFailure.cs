using System;

namespace HomeIrrigation.EventStore.Exceptions
{
    public class ConnectionFailure : Exception
    {
        public ConnectionFailure(string message) : base(message)
        {

        }
    }
}
