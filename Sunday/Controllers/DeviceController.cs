using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sunday.Data;
using Sunday.Models;

namespace Sunday.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public DeviceController(ILogger<DeviceController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDevices(string userId)
        {
            var devices = await _dbContext.Devices.Where(d => d.ApplicationUserID.Equals(userId)).ToListAsync();
            return Ok(devices);
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody]Device device)
        {
            var user = await _dbContext.Users.SingleAsync((ApplicationUser arg) => arg.Id.Equals(device.ApplicationUserID));
            if (user == null)
            {
                return NotFound();
            }
            var addedDevice = await _dbContext.Devices.AddAsync(device);

            if (addedDevice == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            await _dbContext.SaveChangesAsync();

            return Ok(device);
        }
    }
}
