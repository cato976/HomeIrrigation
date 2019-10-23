using HomeIrrigation.Common.Interfaces;
using HomeIrrigation.ESFramework.Common.Interfaces;
using System;

namespace Irrigation.Common.Commands
{
    public class StartIrrigation : ICommand
    {
        public StartIrrigation(Guid tenantId, Guid id, int howLongToIrrigate, IEventStore eventStore)
        {
            TenantId = tenantId;
            Id = id;
            HowLongToIrrigate = howLongToIrrigate;
            EventStore = eventStore;
        }
        
        public Guid TenantId { get; private set; }
        public Guid Id { get; private set; }
        public int HowLongToIrrigate { get; private set; }
        public IEventStore EventStore { get; private set; }
    }
    
    public class StopIrrigation : ICommand
    {
        public StopIrrigation(Guid tenantId, Guid id, IEventStore eventStore)
        {
            TenantId = tenantId;
            Id = id;
            EventStore = eventStore;
        }
        
        public Guid TenantId { get; private set; }
        public Guid Id { get; private set; }
        public IEventStore EventStore { get; private set; }
    }
}
