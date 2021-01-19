using System;
using System.Collections.Generic;

namespace Sunday.Models
{
    public class Device
    {
        public Guid ID { get; set; }

        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Name { get; set; }

        public ICollection<DeviceData> Data { get; set; }
    }
}
