using System;
using System.Diagnostics;
using Newtonsoft.Json;
using PinnacleWrapper.Enums;

namespace PinnacleWrapper.Data
{
    [DebuggerDisplay("{Home} vs {Away}")]
    public class FixturesEvent
    {
        [JsonProperty(PropertyName = "id")] 
        public long Id;

        [JsonProperty(PropertyName = "starts")]
        public DateTime Start;

        [JsonProperty(PropertyName = "home")] 
        public string Home;

        [JsonProperty(PropertyName = "away")] 
        public string Away;

        [JsonProperty(PropertyName = "rotNum")]
        public string RotationNumber;

        [JsonProperty(PropertyName = "liveStatus")]
        public LiveStatus LiveStatus;

        [JsonProperty(PropertyName = "status")]
        public EventStatus EventStatus;

        [JsonProperty(PropertyName = "parlayRestriction")]
        public ParlayRestriction ParlayRestriction;

        [JsonProperty(PropertyName = "parentId")]
        public long ParentId;
    }
}