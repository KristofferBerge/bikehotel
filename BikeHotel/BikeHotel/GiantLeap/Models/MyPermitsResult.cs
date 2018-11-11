using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace BikeHotel.GiantLeap.Models
{
    [Preserve(AllMembers = true)]
    public class MyPermitsResult
    {
        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }

        [JsonProperty("permits")]
        public IList<Permit> Permits { get; set; }

    }
}
