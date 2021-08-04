using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleAPI.Models
{
    public class ScheduleEventResponse
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDescription { get; set; }

        public ScheduleEventResponse(ScheduleEvent scheduleEvent)
        {
            Id = scheduleEvent.Id;
            EventDate = scheduleEvent.EventDate;
            EventDescription = scheduleEvent.EventDescription;
        }
    }
}
