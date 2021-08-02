using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleAPI.Models
{
    public class ScheduleContext : DbContext
    {
        public DbSet<ScheduleEvent> ScheduleEvents { get; set; }
        public ScheduleContext(DbContextOptions<ScheduleContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
