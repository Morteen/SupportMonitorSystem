using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinalRtest.Models
{
    public class TMSDbContext : DbContext
    {
       
        public TMSDbContext(DbContextOptions<TMSDbContext> options) : base(options)
        {

        }
        public DbSet<TMS> TMS { get; set; }
        public DbSet<TF_Services> TF_Services { get; set; }
        public DbSet<ServicesRunningOnTMS> RunningServices { get; set; }

    }

}
