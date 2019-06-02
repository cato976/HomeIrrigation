using HomeIrrigation.ESFramework.Common.Interfaces;
using System.Collections.Generic;

namespace HomeIrrigation.ESFramework.Common.Interfaces
{
    public interface IAggregate
    {
        IEnumerable<IEvent> GetUncommittedEvents();
    }
}
