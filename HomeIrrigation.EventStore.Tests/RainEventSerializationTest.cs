using System;
using System.Text;
using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace HomeIrrigation.EventStore.Test
{
    public class RainEventSerializationTest
    {
        public static string EventClrTypeHeader = "EventClrTypeName";

        [Test]
        public void EventSerialization_SerializeRainFellEvent_ShouldDeserialize()
        {
            var eventMetaData = new EventMetadata(Guid.NewGuid(), "testCat", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var serializableEvent = new RainFell(Guid.NewGuid(), new DateTimeOffset(DateTime.UtcNow), eventMetaData, 4);

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
            Assert.AreNotEqual(deserializedEventData.Metadata.PublishedDateTime, DateTime.MinValue);
            Assert.Greater(serializableEvent.Metadata.PublishedDateTime.DateTime, DateTime.MinValue);
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
