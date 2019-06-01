using System;
using HomeIrrigation.Common.Interfaces;

namespace HomeIrrigation.Api.DataTransferObjects.Commands.Rain
{
    public class RainFallCommand : ICommand
    {
        public RainFallCommand(double inches)
        {
            Inches = inches;
        }

        public Guid Id { get; private set; }

        public double Inches { get; set; }
        
    }
}
