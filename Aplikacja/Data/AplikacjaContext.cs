using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Aplikacja.Models;

namespace Aplikacja.Data
{
    public class AplikacjaContext : DbContext
    {
        // konstruktor pobiera opcje konfiguracji
        public AplikacjaContext (DbContextOptions<AplikacjaContext> options)
            : base(options)
        {
        }

        //obiekty klasy DbSet reprezentujace tabele w bazie danych
        public DbSet<Uzytkownik> Uzytkownicy { get; set; } = default!;
        public DbSet<Warstwa> Warstwy { get; set; } = default!;
        public DbSet<Roslina> Rosliny { get; set; } = default!;
        public DbSet<Wymaganie> Wymagania { get; set; } = default!;
        public DbSet<Zaleznosc> Zaleznosci { get; set; } = default!;
        public DbSet<OgrodUzytkownika> OgrodyUzytkownikow { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // zabezpiecznie przed kaskadowym usuwaniem zaleznosci (self reference)
            modelBuilder.Entity<Zaleznosc>()
                .HasOne(z => z.RoslinaGlowna)
                .WithMany(r => r.ZaleznosciJakoGlowna)
                .HasForeignKey(z => z.RoslinaGlownaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zaleznosc>()
                .HasOne(z => z.RoslinaTowarzyszaca)
                .WithMany(r => r.ZaleznosciJakoTowarzyszaca)
                .HasForeignKey(z => z.RoslinaTowarzyszacaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}