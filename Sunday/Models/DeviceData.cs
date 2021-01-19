using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sunday.Models
{
    public class DeviceData
    {
        [Key]   
        public DateTime Timestamp { get; set; }
        public string Label { get; set; }
        public double Value { get; set; }

        public Guid DeviceID { get; set; }
        public Device Device { get; set; }
    }
}
