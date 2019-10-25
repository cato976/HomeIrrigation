using System;
using HomeIrrigation.Api.Handlers;
using HomeIrrigation.Common.EventBus;
using Moq;
using NUnit.Framework;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.Common.Interfaces;
using HomeIrrigation.Common.CommandBus;
using HomeIrrigation.Api.DataTransferObjects.Commands.Rain;

namespace HomeIrrigation.Api.Test
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class HandlerTest
    {
        Mock<IEventStore> moqEventStore;
        IEventMetadata eventMetadata;

        [SetUp]
        public void Setup()
        {
            moqEventStore = new Mock<IEventStore>();
            eventMetadata = new EventMetadata(Guid.NewGuid(), "TestCategory", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        }

        [Test]
        public void Should_Handle_Rain_Fell_Event()
        {
            PassEventToEventBus(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow,
                eventMetadata, 56));
        }

        [Test]
        public void Should_Handle_Rain_Fell_Command()
        {
            PassCommandToCommandBus(new RainFallCommand(70));
        }

        [Test]
        public void Should_Get_Weekly_Rainfall()
        {
            PassEventToEventBus(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow,
                eventMetadata, 56));
            
        }

        private void PassEventToEventBus(IEvent handledEvent)
        {
            EventStoreHandlerRegistration.RegisterEventHandler(moqEventStore.Object);
            var eventBus = EventBus.Instance;
            eventBus.Execute(handledEvent);
        }

        private void PassCommandToCommandBus(ICommand handleCommand)
        {
            CommandHandlerRegistration.RegisterCommandHandler();
            var commandBus = CommandBus.Instance;
            commandBus.Execute(handleCommand);
        }
    }
}
