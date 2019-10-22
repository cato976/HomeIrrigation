using System;

namespace HomeIrrigation.Sprinkler.Service.DataTransferObjects.Commands.Irrigation
{
    public class StartIrrigationCommand
    {
        public Guid TenantId { get; set; }
        public Guid Zone { get; set; }
        public int HowLongToIrrigate { get; set; }
    }
}
