using System;

namespace HomeIrrigation.ESFramework.Common.Exceptions
{
    public class EventStreamSubscriptionException : Exception
    {
        public EventStreamSubscriptionException(string subscriptionDropReason, Exception exception) : base(subscriptionDropReason, exception)
        {
        }
    }
}
