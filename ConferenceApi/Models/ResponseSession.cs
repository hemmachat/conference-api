using System.Runtime.Serialization;

namespace ConferenceApi.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ResponseSession
    {
        [JsonProperty("collection")]
        public Collection Collection { get; set; }
    }

    public partial class Collection
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("links")]
        public object[] Links { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("queries")]
        public object[] Queries { get; set; }

        [JsonProperty("template")]
        public Template Template { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("data")]
        public Datum[] Data { get; set; }

        [JsonProperty("links")]
        public Link[] Links { get; set; }

        public string Title { get; set; }

        public string Timeslot { get; set; }

        public string Speaker { get; set; }

        public Item[] Topics { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("name")]
        [JsonConverter(typeof(NameConverter))]
        public Name Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Link
    {
        [JsonProperty("rel")]
        public Uri Rel { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public partial class Template
    {
        [JsonProperty("data")]
        public object[] Data { get; set; }
    }

    public enum Name
    {
        //[EnumMember(Value = "Speaker")]
        Speaker, 
        
        //[EnumMember(Value = "Timeslot")]
        Timeslot, 
        
        //[EnumMember(Value = "Title")]
        Title
    };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                NameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Name) || t == typeof(Name?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Speaker":
                    return Name.Speaker;
                case "Timeslot":
                    return Name.Timeslot;
                case "Title":
                    return Name.Title;
            }
            throw new Exception("Cannot unmarshal type Name");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Name)untypedValue;
            switch (value)
            {
                case Name.Speaker:
                    serializer.Serialize(writer, "Speaker");
                    return;
                case Name.Timeslot:
                    serializer.Serialize(writer, "Timeslot");
                    return;
                case Name.Title:
                    serializer.Serialize(writer, "Title");
                    return;
            }
            throw new Exception("Cannot marshal type Name");
        }

        public static readonly NameConverter Singleton = new NameConverter();
    }
}
