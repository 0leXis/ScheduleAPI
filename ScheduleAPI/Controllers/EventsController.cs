using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleEventResponse>>> Get()
        {
            return await _db.ScheduleEvents.Where(x => x.User.Login == User.Identity.Name).Select(x => new ScheduleEventResponse(x)).ToListAsync();
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleEventResponse>> Get(int id)
        {
            var scheduleEvent = await _db.ScheduleEvents.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id);
            if (scheduleEvent == null)
                return NotFound();
            if (scheduleEvent.User.Login != User.Identity.Name)
                return Unauthorized("ScheduleEvent belongs to another user");
            return new ScheduleEventResponse(scheduleEvent);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ScheduleEventResponse>> Post(ScheduleEvent scheduleEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Login == User.Identity.Name);
            if(user == null)
                return NotFound("User not found");
            scheduleEvent.User = user;
            _db.ScheduleEvents.Add(scheduleEvent);
            await _db.SaveChangesAsync();
            return new ScheduleEventResponse(scheduleEvent);
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ScheduleEventResponse>> Put(ScheduleEvent scheduleEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dbEvent = await _db.ScheduleEvents.SingleOrDefaultAsync(x => x.Id == scheduleEvent.Id);
            if (dbEvent == null)
                return NotFound();
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Login == User.Identity.Name);
            if (user == null)
                return NotFound("User not found");
            if(user.Login != dbEvent.User.Login)
                return Unauthorized("ScheduleEvent belongs to another user");
            scheduleEvent.User = user;
            _db.Update(scheduleEvent);
            await _db.SaveChangesAsync();
            return new ScheduleEventResponse(scheduleEvent);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ScheduleEventResponse>> Delete(int id)
        {
            var scheduleEvent = _db.ScheduleEvents.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            if (scheduleEvent == null)
                return NotFound();
            if (scheduleEvent.User.Login != User.Identity.Name)
                return Unauthorized("ScheduleEvent belongs to another user");
            _db.ScheduleEvents.Remove(scheduleEvent);
            await _db.SaveChangesAsync();
            return new ScheduleEventResponse(scheduleEvent);
        }
    }
}
