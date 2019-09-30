using System;

namespace HomeIrrigation.Api.DataTransferObjects.Commands
{
    public class IrrigateZoneCommand
    {
        public Guid Id { get; set; }
        public int Zone { get; set; }
    }
}
