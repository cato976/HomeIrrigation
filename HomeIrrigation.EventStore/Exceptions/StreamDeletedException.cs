using System;

namespace HomeIrrigation.EventStore.Exceptions
{
    public class StreamDeletedException : Exception
    {
        public StreamDeletedException(string streamName) : base(streamName)
        { }
    }
}
