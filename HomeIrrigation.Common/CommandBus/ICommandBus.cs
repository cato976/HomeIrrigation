using System;
using HomeIrrigation.Common.Interfaces;

namespace HomeIrrigation.Common.CommandBus
{
    public interface ICommandBus
    {
        void RemoveHandlers();
        void RegisterHandler<T>(Action<T> handler) where T : ICommand;
        void Execute<T>(T @event) where T : ICommand;
    }
}
