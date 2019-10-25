using System;

namespace HomeIrrigation.Sprinkler.Service.DataTransferObjects.Commands.Irrigation
{
    public class StopIrrigationCommand
    {
        public Guid TenantId { get; set; }
        public Guid Zone { get; set; }
    }
}
