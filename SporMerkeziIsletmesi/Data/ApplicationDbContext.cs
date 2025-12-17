using Microsoft.AspNetCore.Identity;
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

            // === UYE ===
            builder.Entity<Uye>(entity =>
            {
                entity.ToTable("Uyeler");

                // AspNetUsers.Id = nvarchar(450) => FK alanı da 450 olmalı
                entity.Property(u => u.IdentityUserId)
                      .IsRequired()
                      .HasMaxLength(450);

                // Navigation yok (modelde IdentityUser property yok) -> generic HasOne ile kur
                entity.HasOne<IdentityUser>()
                      .WithMany()
                      .HasForeignKey(u => u.IdentityUserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // === RANDEVU ===
            builder.Entity<Randevu>(entity =>
            {
                entity.ToTable("Randevular");

                entity.HasOne(r => r.Uye)
                      .WithMany() // Uye içinde Randevular koleksiyonu yoksa böyle kalmalı
                      .HasForeignKey(r => r.UyeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }


    }
}
