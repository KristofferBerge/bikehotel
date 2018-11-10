using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeHotel.GiantLeap.Models
{
    public class MyPermitsResult
    {

        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }

        [JsonProperty("permits")]
        public IList<Permit> Permits { get; set; }

    }
}
