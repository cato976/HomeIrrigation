using HomeIrrigation.Common.CommandBus;
using HomeIrrigation.Api.DataTransferObjects.Commands.Rain;

namespace HomeIrrigation.Api.Handlers
{
    public static class CommandHandlerRegistration
    {
        public static void RegisterCommandHandler()
        {
            var rainHandlers = new RainCommandHandlers();
            var commandBus = CommandBus.Instance;
            commandBus.RemoveHandlers();
            commandBus.RegisterHandler<RainFallCommand>(rainHandlers.Handle);;
        }
    }
}
