using e_crime.mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_crime.mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //explicitly define the relationship, 1:m,  between police station and users(officers)
            //and what happens if a police station is deleted.
            builder.Entity<ApplicationUser>()
                .HasOne(p => p.PoliceStation)
                .WithMany(au => au.PoliceOfficers)
                .HasForeignKey(p => p.PoliceStationId)
                .OnDelete(DeleteBehavior.SetNull);

            //1:m relationship between user and crime

            builder.Entity<Crime>()
                .HasOne(au => au.User)
                .WithMany(c => c.Crimes)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            //1:m relationship between user(officer) and crime
            builder.Entity<Crime>()
               .HasOne(ao => ao.AssignedOfficer)
               .WithMany(ac => ac.AssignedCrimes)
               .HasForeignKey(fk => fk.AssignedTo)
               .OnDelete(DeleteBehavior.NoAction);

            ////m:m relationship between application user and crime
            //builder.Entity<UserCrime>()
            //    .HasKey(x => new { x.UserId, x.CrimeId });

            //builder.Entity<UserCrime>()
            //    .HasOne(au => au.ApplicationUser)
            //    .WithMany(uc => uc.UserCrimes)
            //    .HasForeignKey(fk => fk.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<UserCrime>()
            //    .HasOne(c => c.Crime)
            //    .WithMany(uc => uc.UserCrimes)
            //    .HasForeignKey(fk => fk.CrimeId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Crime> Crimes { get; set; }
        public DbSet<PoliceStation> PoliceStations { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }

    }
}
