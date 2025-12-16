using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; }
        public DbSet<AntrenorMusaitlik> AntrenorMusaitlikler { get; set; }


        public DbSet<Uye> Uyeler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Uye>()
                .Property(x => x.IdentityUserId)
                .HasMaxLength(450);
        }


    }
}
