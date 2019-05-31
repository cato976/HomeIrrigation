using System;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESEvents.Common.Events;

namespace HomeIrrigation.Data.DataAccess.Models
{
    public class RainFall : Aggregate
    {
        private RainFall()
        {

        }

        public RainFall(Guid id, IEventMetadata eventMetadata, double inches)
        {
            ApplyEvent(new RainFell(id, DateTimeOffset.UtcNow, eventMetadata, inches));
        }
    }
}
