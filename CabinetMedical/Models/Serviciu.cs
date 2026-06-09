using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetMedical.Models
{
    public class Serviciu
    {
        public int Id { get; set; }
        public int? DoctorId { get; set; }
        public virtual Utilizator? Doctor { get; set; } 
        public string Denumire { get; set; } = string.Empty;
        public string Descriere { get; set; } = string.Empty;
        public decimal Pret { get; set; }
        public int DurataMInute { get; set; } = 5;
        public string Categorie { get; set; } = string.Empty;
        public bool EsteActiv { get; set; } = true;
        public ICollection<Programare> Programari { get; set; } = new List<Programare>();
    }
}