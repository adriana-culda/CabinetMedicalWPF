using System.Collections.Generic;
using System.Linq;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;

            //se verifica daca baza de date este goala
            if (!_context.Utilizatori.Any())
            {
                SeedData();
            }
        }

        public Utilizator? Autentificare(string numeUtilizator, string parola)
        {
            return _context.Utilizatori
                .FirstOrDefault(u => u.NumeUtilizator == numeUtilizator
                                  && u.ParolaHash == parola
                                  && u.EsteActiv);
        }

        private void SeedData()
        {
            var utilizatori = new List<Utilizator>
            {
                new Utilizator { NumeUtilizator = "admin", ParolaHash = "admin", Nume = "Admin", Rol = "Admin" },
                new Utilizator { NumeUtilizator = "doctor1", ParolaHash = "doctor1", Nume = "Doctor", Rol = "Angajat" },
                new Utilizator { NumeUtilizator = "client1", ParolaHash = "client1", Nume = "Client", Rol = "Client" }
            };

            _context.Utilizatori.AddRange(utilizatori);
            _context.SaveChanges();
        }
    }
}