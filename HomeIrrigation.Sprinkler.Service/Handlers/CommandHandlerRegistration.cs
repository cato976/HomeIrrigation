using HomeIrrigation.Common.CommandBus;
using Irrigation.Common.Commands;

namespace HomeIrrigation.Sprinkler.Service.Handlers
{
    public static class CommandHandlerRegistration
    {
        public static void RegisterCommandHandler()
        {
            var irrigationHandlers = new IrrigationCommandHandlers();
            var commandBus = CommandBus.Instance;
            commandBus.RemoveHandlers();
            commandBus.RegisterHandler<StartIrrigation>(irrigationHandlers.Handle);
            commandBus.RegisterHandler<StopIrrigation>(irrigationHandlers.Handle);
        }
    }
    
}
