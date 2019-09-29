using NUnit.Framework;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESEvents.Common.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HomeIrrigation.ESFramework.Common.Interfaces;

namespace HomeIrrigation.EventStore.Test
{
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
            var deserializedEventData = DeserializeObject(eventDataJson, eventMetadata.CustomMetadata[EventClrTypeHeader]) as IEvent;
            RainFell castDeserializedEvent = (RainFell)deserializedEventData;

            Assert.IsNotNull(deserializedEventData);
            Assert.AreEqual(serializableEvent.Metadata.AccountGuid, deserializedEventData.Metadata.AccountGuid);
            Assert.AreEqual(serializableEvent.AggregateGuid, deserializedEventData.AggregateGuid);
            Assert.AreEqual(serializableEvent.EffectiveDateTime, deserializedEventData.EffectiveDateTime);
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

        private static object DeserializeObject(string eventDataJson, string typeName)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(eventDataJson, Type.GetType(typeName));
                return obj;
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }
    }
}
