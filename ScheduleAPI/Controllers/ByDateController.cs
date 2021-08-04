using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleAPI.Controllers
{
    [Route("api/events/[controller]")]
    [ApiController]
    public class ByDateController : ControllerBase
    {
        private ScheduleContext _db;
        public ByDateController(ScheduleContext db)
        {
            _db = db;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ScheduleEventResponse>> Get(DateTime date)
        {
            var scheduleEvent = await _db.ScheduleEvents.Include(x => x.User).SingleOrDefaultAsync(x => x.EventDate == date);
            if (scheduleEvent == null)
                return NotFound();
            if (scheduleEvent.User.Login != User.Identity.Name)
                return Unauthorized("ScheduleEvent belongs to another user");
            return new ScheduleEventResponse(scheduleEvent);
        }
    }
}
