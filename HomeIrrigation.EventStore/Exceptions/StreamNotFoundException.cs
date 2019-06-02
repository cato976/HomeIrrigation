using System;

namespace HomeIrrigation.EventStore.Exceptions
{
    public class StreamNotFoundException : Exception
    {
        public StreamNotFoundException(string streamName) : base(streamName)
        { }
    }
}
