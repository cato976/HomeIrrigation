using HomeIrrigation.ESFramework.Common.Base;
using System.Collections.Generic;

namespace HomeIrrigation.ESFramework.Common.Interfaces
{
    public interface IAggregate
    {
        IEnumerable<Event> GetUncommittedEvents();
    }
}
