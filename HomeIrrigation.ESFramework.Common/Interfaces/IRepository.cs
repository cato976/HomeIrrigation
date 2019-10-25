using HomeIrrigation.ESFramework.Common.Base;

namespace HomeIrrigation.ESFramework.Common.Interfaces
{
    public interface IRepository<T> where T : IAggregate, new()
    {
        T GetById(CompositeAggregateId aggregateId);
    }
}
