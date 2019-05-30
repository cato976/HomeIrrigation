using System;

namespace HomeIrrigation.ESFramework.Common.Base
{
    public class CompositeAggregateId
    {
        public CompositeAggregateId(Guid tenantId,Guid aggregateId, string category)
        {
            TenantId = tenantId;
            AggregateId = aggregateId;
            Category = category;
        }

        public Guid TenantId { get; }
        public Guid AggregateId { get; }
        public string Category { get; }
        public string CompositeId => Category + "-" + TenantId + "-" + AggregateId;
    }
}
