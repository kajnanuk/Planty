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
        // Konstruktor pobiera opcje kontekstu bazy danych, dzięki którym 
        // możliwa będzie wygodna komunikacja i utrwalanie obiektów
        public AplikacjaContext (DbContextOptions<AplikacjaContext> options)
            : base(options)
        {
        }

        // Definicje obiektów klasy DbSet, przy pomocy których będziemy 
        // mieli dostęp do poszczególnych tabel w bazie
        public DbSet<Uzytkownik> Uzytkownicy { get; set; } = default!;
        public DbSet<Warstwa> Warstwy { get; set; } = default!;
        public DbSet<Roslina> Rosliny { get; set; } = default!;
        public DbSet<Wymaganie> Wymagania { get; set; } = default!;
        public DbSet<Zaleznosc> Zaleznosci { get; set; } = default!;
        public DbSet<OgrodUzytkownika> OgrodyUzytkownikow { get; set; } = default!;

        // Konfiguracja specyficznych zachowań modeli podczas tworzenia bazy
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Zabezpieczenie przed pętlą kaskadowego usuwania dla self-referencing w tabeli Zaleznosci
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