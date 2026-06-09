using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetMedical.Models
{
    public class Utilizator
    {
        public int Id { get; set; }
        public string NumeUtilizator { get; set; } = string.Empty;
        public string ParolaHash { get; set; } = string.Empty;
        public string Nume { get; set; } = string.Empty;
        public string Prenume { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Rol { get; set; } = "Client";
        public int? DoctorAsociatId { get; set; }
        public DateTime DataInregistrare { get; set; } = DateTime.Now;
        public bool EsteActiv { get; set; } = true;
        public ICollection<Programare> Programari { get; set; } = new List<Programare>();
        public ICollection<Plata> Plati { get; set; } = new List<Plata>();
    }

}
