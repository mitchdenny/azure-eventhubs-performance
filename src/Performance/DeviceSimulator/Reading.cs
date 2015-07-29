using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    public class Reading
    {
        public Reading()
        {
            this.Taken = DateTimeOffset.Now;
        }

        [JsonProperty("taken")]
        public DateTimeOffset Taken { get; set; }
    }
}
