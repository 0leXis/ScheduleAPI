using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleAPI.Models
{
    public class ScheduleEvent
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        [Required]
        public string EventDescription { get; set; }
    }
}
