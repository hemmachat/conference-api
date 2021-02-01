namespace ConferenceApi.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ResponseTopic
    {
        [JsonProperty("collection")]
        public Collection Collection { get; set; }
    }
}
