using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScheduleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private ScheduleContext _db;
        public EventsController(ScheduleContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleEvent>>> Get()
        {
            return await _db.ScheduleEvents.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleEvent>> Get(int id)
        {
            return await _db.ScheduleEvents.SingleAsync(x => x.Id == id);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ScheduleEvent>>> Post(ScheduleEvent scheduleEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _db.ScheduleEvents.Add(scheduleEvent);
            await _db.SaveChangesAsync();
            return Ok(scheduleEvent);
        }
        [HttpPut]
        public async Task<ActionResult<IEnumerable<ScheduleEvent>>> Put(ScheduleEvent scheduleEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_db.ScheduleEvents.Any(x => x.Id == scheduleEvent.Id))
                return NotFound();
            _db.Update(scheduleEvent);
            await _db.SaveChangesAsync();
            return Ok(scheduleEvent);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<ScheduleEvent>>> Delete(int id)
        {
            var scheduleEvent = _db.ScheduleEvents.FirstOrDefault(x => x.Id == id);
            if (scheduleEvent == null)
                return NotFound();
            _db.ScheduleEvents.Remove(scheduleEvent);
            await _db.SaveChangesAsync();
            return Ok(scheduleEvent);
        }
    }
}
