using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.Sprinkler.Service.Domain;
using Irrigation.Common.Commands;
using System;

namespace HomeIrrigation.Sprinkler.Service.Handlers
{
    public class IrrigationCommandHandlers
    {
        public IrrigationCommandHandlers()
        {
        }

        public void Handle(StartIrrigation message)
        {
            var eventMetadata = new EventMetadata(message.TenantId, "IRRIGATION", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            
            // Validation 
            if (Guid.Empty == message.Id)
            {
                throw new ArgumentException(nameof(message.Id));
            }

            // Process
            var zone = new Zone(message.Id);
            zone.IrrigateZone(eventMetadata, message.EventStore);
        }
    }
}
