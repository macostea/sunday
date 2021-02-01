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

        [HttpGet]
        public async Task<IActionResult> GetDevices([FromQuery]string userId)
        {
            var devices = await _dbContext.Devices.Where(d => d.ApplicationUserID.Equals(userId)).ToListAsync();
            return Ok(devices);
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody]Device device)
        {
            try
            {
                _ = await _dbContext.Users.SingleAsync((ApplicationUser arg) => arg.Id.Equals(device.ApplicationUserID));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogDebug("Failed to find user: {0}", e);
                return StatusCode(StatusCodes.Status404NotFound);
            }

            var addedDevice = await _dbContext.Devices.AddAsync(device);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to add entity: {0}", e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(device);
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetData(Guid deviceId, [FromQuery]DateTime startTime, [FromQuery]DateTime endTime)
        {
            try
            {
                _ = await _dbContext.Devices.SingleAsync((Device d) => d.ID.Equals(deviceId));
            }
            catch (InvalidOperationException e)
            {
                _logger.LogDebug("Failed to find device: {0}", e);
                return StatusCode(StatusCodes.Status404NotFound);
            }

            try
            {
                var data = await _dbContext.Data.Where((DeviceData d) => d.DeviceID.Equals(deviceId) && d.Timestamp >= startTime && d.Timestamp <= endTime).OrderBy((DeviceData d) => d.Timestamp).ToListAsync();
                return Ok(data);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to query data for device: {0}", e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
