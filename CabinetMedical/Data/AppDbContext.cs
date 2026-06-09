using CabinetMedical.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetMedical.Data
{
    public class AppDbContext : DbContext
    {
        //tabelele 
        public DbSet<Utilizator> Utilizatori { get; set; }
        public DbSet<Serviciu> Servicii { get; set; }
        public DbSet<Programare> Programari { get; set; }
        public DbSet<Plata> Plati { get; set; }
        public DbSet<JurnalAudit> Jurnale { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //creata in acelasi folder
            options.UseSqlite("Data Source=CabinetMedical.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //programare - client -- un client poate avea multe programari
            modelBuilder.Entity<Programare>()
                .HasOne(p => p.Client)
                .WithMany(u => u.Programari)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            //programare - angajat -- un angajat poate avea multe programari
            modelBuilder.Entity<Programare>()
                .HasOne(p => p.Angajat)
                .WithMany()
                .HasForeignKey(p => p.AngajatId)
                .OnDelete(DeleteBehavior.Restrict);

            //programare - serviciu -- un serviciu poate aparea in multe programari
            modelBuilder.Entity<Programare>()
                .HasOne(p => p.Serviciu)
                .WithMany(s => s.Programari)
                .HasForeignKey(p => p.ServiciuId)
                .OnDelete(DeleteBehavior.Restrict);

            //plata - programare -- o programare are o singura plata
            modelBuilder.Entity<Plata>()
                .HasOne(p => p.Programare)
                .WithOne(pr => pr.Plata)
                .HasForeignKey<Plata>(p => p.ProgramareId)
                .OnDelete(DeleteBehavior.Restrict);

            //plata - client -- un client poate avea multe plati
            modelBuilder.Entity<Plata>()
                .HasOne(p => p.Client)
                .WithMany(u => u.Plati)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            //jurnalAudit - utilizator 
            modelBuilder.Entity<JurnalAudit>()
                .HasOne(j => j.Utilizator)
                .WithMany()
                .HasForeignKey(j => j.UtilizatorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        public override int SaveChanges()
        {
            //se uita la schimbari
            var entries = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified)
                             && !(e.Entity is JurnalAudit))
                .ToList();

            foreach (var entry in entries)
            {
                string detaliiExtra = "";

                //extrage date specifice in funcție de tipul obiectului
                if (entry.Entity is Utilizator u)
                {
                    detaliiExtra = $" (Nume: {u.NumeUtilizator}, Rol: {u.Rol})";
                }
                else if (entry.Entity is Serviciu s)
                {
                    detaliiExtra = $" (Denumire: {s.Denumire})";
                }
                

                var log = new JurnalAudit
                {
                    DataOra = DateTime.Now,

                    //tradus in romana
                    Actiune = entry.State == EntityState.Added ? "Adăugare" :
                              entry.State == EntityState.Deleted ? "Ștergere" : "Modificare",
                    Entitate = entry.Entity.GetType().Name,

                    //detaliile extra
                    Detalii = $"Obiect: {entry.Entity.GetType().Name}{detaliiExtra}"
                };

                this.Jurnale.Add(log);
            }

            return base.SaveChanges();
        }
    }
}
