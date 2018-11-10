using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace BikeHotel.GiantLeap.Models
{
    [Preserve(AllMembers = true)]
    public class Permit
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("categoryType")]
        public string PermitType { get; set; }

        [JsonProperty("scope")]
        public string Location { get; set; }

        [JsonProperty("name")]
        public string PermitTypeName { get; set; }


        public override string ToString()
        {
            return Location;
        }

    }
}
