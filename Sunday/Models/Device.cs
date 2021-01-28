using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sunday.Models
{
    public class Device
    {
        public Guid ID { get; set; }

        public string ApplicationUserID { get; set; }
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<DeviceData> Data { get; set; }
    }
}
