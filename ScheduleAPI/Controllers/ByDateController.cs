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
        public async Task<ActionResult<ScheduleEvent>> Get(DateTime date)
        {
            var scheduleEvent = await _db.ScheduleEvents.SingleOrDefaultAsync(x => x.EventDate == date);
            if (scheduleEvent == null)
                return NotFound();
            return scheduleEvent;
        }
    }
}
