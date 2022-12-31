using System;
using System.Text;
using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using System.Text.Json;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net.WebSockets;
using System.Text.Json.Nodes;

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
            //var deserializedEventData = DeserializeObject(eventDataJson, eventMetadata.CustomMetadata[EventClrTypeHeader]) as IEvent;
            var deserializedEventData = DeserializeObject(eventDataJson, eventMetadata.CustomMetadata[EventClrTypeHeader]);
            EventMetadata eventMetadataObject = new EventMetadata(Guid.Parse(deserializedEventData["Metadata"]["TenantId"].ToString())
                , deserializedEventData["Metadata"]["Category"].ToString()
                , Guid.Parse(deserializedEventData["Metadata"]["CorrelationId"].ToString())
                , Guid.Parse(deserializedEventData["Metadata"]["CausationId"].ToString())
                , Guid.Parse(deserializedEventData["Metadata"]["AccountGuid"].ToString()));
            var rainFellObject = new RainFell(Guid.Parse(deserializedEventData["AggregateGuid"].ToString())
                , DateTime.Parse(deserializedEventData["EffectiveDateTime"].ToString())
                , eventMetadataObject, 0);
            RainFell castDeserializedEvent = rainFellObject;

            Assert.IsNotNull(deserializedEventData);
            Assert.AreEqual(serializableEvent.Metadata.AccountGuid, rainFellObject.Metadata.AccountGuid);
            Assert.AreEqual(serializableEvent.AggregateGuid, rainFellObject.AggregateGuid);
            Assert.AreEqual(serializableEvent.EffectiveDateTime, rainFellObject.EffectiveDateTime);
            Assert.AreNotEqual(rainFellObject.Metadata.PublishedDateTime, DateTime.MinValue);
            Assert.Greater(serializableEvent.Metadata.PublishedDateTime.DateTime, DateTime.MinValue);
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
