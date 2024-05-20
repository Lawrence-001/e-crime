using e_crime.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_crime.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //explicitly define the relationship between users(officers) and police station 
            //and what happens if a police station is deleted.
            builder.Entity<ApplicationUser>()
                .HasOne(p => p.PoliceStation)
                .WithMany(au => au.PoliceOfficers)
                .HasForeignKey(p => p.PoliceStationId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<Crime> Crimes { get; set; }
        public DbSet<PoliceStation> PoliceStations { get; set; }

    }
}
