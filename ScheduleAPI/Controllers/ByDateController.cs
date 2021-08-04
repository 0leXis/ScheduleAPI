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
        public async Task<ActionResult<IEnumerable<ScheduleEventResponse>>> Get(DateTime date)
        {
            var scheduleEvents = await _db.ScheduleEvents
                .Include(x => x.User)
                .Where(x => x.EventDate.Date == date && x.User.Login != User.Identity.Name)
                .Select(x => new ScheduleEventResponse(x))
                .ToListAsync();
            return scheduleEvents;
        }
    }
}
