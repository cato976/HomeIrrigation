using NUnit.Framework;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESEvents.Common.Events;
using System;
using System.Text;
using System.Text.Json;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HomeIrrigation.ESFramework.Common.Interfaces;
using System.Text.Json.Nodes;

namespace HomeIrrigation.EventStore.Test
{
    [Parallelizable(ParallelScope.Children)]
    public class EventSerializationTests
    {
        public static string EventClrTypeHeader = "EventClrTypeName";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EventSerialization_CreateEventMetadata_InvalidTenantId_ShouldThrow_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new EventMetadata(new Guid(), "testCor", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void EventSerialization_CreateEventMetadata_InvalidCategory_ShouldThrow_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EventMetadata(Guid.NewGuid(), null, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void EventSerialization_CreateEventMetadata_InvalidCorrelationId_ShouldThrow_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EventMetadata(Guid.NewGuid(), "testCategory", Guid.Empty, Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void EventSerialization_SerializeRainFellEvent_ShouldDeserialize()
        {
            var eventMetaData = new EventMetadata(Guid.NewGuid(), "testCat", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var serializableEvent = new RainFell(Guid.NewGuid(), new DateTimeOffset(DateTime.UtcNow), eventMetaData, 2);

            var eventData = EventSerialization.SerializeEvent(serializableEvent);

            var eventDataJson = Encoding.UTF8.GetString(eventData.Data);
            Console.WriteLine(eventDataJson);
            var parsedJObject = JObject.Parse(eventDataJson);
            var metaDataJToken = parsedJObject["Metadata"];

            var eventMetadata = metaDataJToken.ToObject<EventMetadata>();
            var deserializedEventData = DeserializeObject(eventDataJson, eventMetadata.CustomMetadata[EventClrTypeHeader]);
            EventMetadata eventMetadataObject = new EventMetadata(Guid.Parse(deserializedEventData["Metadata"]["TenantId"].ToString())
                    , deserializedEventData["Metadata"]["Category"].ToString()
                    , Guid.Parse(deserializedEventData["Metadata"]["CorrelationId"].ToString())
                    , Guid.Parse(deserializedEventData["Metadata"]["CausationId"].ToString())
                    , Guid.Parse(deserializedEventData["Metadata"]["AccountGuid"].ToString()));
            //RainFell castDeserializedEvent = (RainFell)deserializedEventData;
            RainFell rainFellObject = new RainFell(Guid.Parse(deserializedEventData["AggregateGuid"].ToString()), DateTime.Parse(deserializedEventData["EffectiveDateTime"].ToString()), eventMetadata, 0);

            Assert.IsNotNull(deserializedEventData);
            Assert.AreEqual(serializableEvent.Metadata.AccountGuid, eventMetadataObject.AccountGuid);
            Assert.AreEqual(serializableEvent.AggregateGuid, rainFellObject.AggregateGuid);
            Assert.AreEqual(serializableEvent.EffectiveDateTime, rainFellObject.EffectiveDateTime);
        }

        [Test]
        public void EventSerialization_SerializeDoctorCreatedEvent_ShouldThrow_ArgumentException()
        {
            DateTime date = new DateTime();
            var eventMetaData = new EventMetadata(Guid.NewGuid(), "testCat", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            Assert.Throws<ArgumentException>(() => new RainFell(Guid.NewGuid(), date, eventMetaData, 3));
        }

        [Test]
        public void EventSerialization_SerializeDoctorCreatedEvent_ShouldThrow_ArgumentNullException()
        {
            DateTime date = DateTime.UtcNow;
            Assert.Throws<ArgumentNullException>(() => new RainFell(Guid.NewGuid(), date, null, 3));
        }

        private static JsonObject DeserializeObject(string eventDataJson, string typeName)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<JsonObject>(eventDataJson);
                return obj;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
    }
}
