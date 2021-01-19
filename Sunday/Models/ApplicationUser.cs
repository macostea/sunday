using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Device> Devices { get; set; }
    }
}
