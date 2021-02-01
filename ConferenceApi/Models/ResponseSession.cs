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
        Speaker,
        Timeslot,
        Title
    }
}
