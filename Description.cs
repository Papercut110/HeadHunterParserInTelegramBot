using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhru
{
    //[JsonObject(MemberSerialization.Fields)]
    public class Description
    {
        //[JsonProperty("Id")]
        public decimal Id { get; set; }
        //[JsonProperty("Name")]
        public string Name { get; set; }
        //[JsonProperty("AreaName")]
        public string AreaName { get; set; }
        //[JsonProperty("SnipRequirement")]
        public string SnipRequirement { get; set; }
        //[JsonProperty("Url")]
        public string Url { get; set; }

    }
}
