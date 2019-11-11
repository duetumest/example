using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using pavlovLab.Models;
using pavlovLab.Storage;
using Serilog;

namespace pavlovLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {
        private static IStorage<LabData> _memCache = new MemCache();

        [HttpGet]
        public ActionResult<IEnumerable<LabData>> Get()
        {
            return Ok(_memCache.All);
        }

        [HttpGet("{id}")]
        public ActionResult<LabData> Get(Guid id)
        {
            Log.Information("Acquiring ID info");
            Log.Error("ID doesn't exist");
            if (!_memCache.Has(id)) 
            {Log.Information($"Acquired ID is {_memCache[id].Id}");
                Log.Debug($"Team {@_memCache[id]} doesn't exist");
                return NotFound("No such");
            }

            return Ok(_memCache[id]);
        }

        [HttpPost]
        public IActionResult Post([FromBody] LabData value)
        {
             Log.Information("Acquiring team info");
            var validationResult = value.Validate();

            if (!validationResult.IsValid) 
            {
                Log.Debug($"Uncorrected data");
                return BadRequest(validationResult.Errors);
            }

            _memCache.Add(value);

            return Ok($"{value.ToString()} has been added");
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] LabData value)
        {
            Log.Information("Acquiring team info");
             Log.Warning("User tried to delete information");
            if (!_memCache.Has(id)) return NotFound("No such");

            var validationResult = value.Validate();

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var previousValue = _memCache[id];
            _memCache[id] = value;
            Log.Debug($"Attempt to put information");

            return Ok($"{previousValue.ToString()} has been updated to {value.ToString()}");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            Log.Information("Acquiring team info");
            Log.Warning("User tried to delete information");
            if (!_memCache.Has(id)) return NotFound("No such");

            var valueToRemove = _memCache[id];
            _memCache.RemoveAt(id);
            Log.Information($"Delete record {DateTime.Now}");
            Log.Debug($"Attempt to delete information");

            return Ok($"{valueToRemove.ToString()} has been removed");
        }
    }
}