using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetMedical.Models
{
    public class Programare
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ServiciuId { get; set; }
        public int? AngajatId { get; set; }
        public DateTime DataOra { get; set; }
        public string Status { get; set; } = "Asteptare";
        public string Observatii { get; set; } = string.Empty;
        public DateTime DataCreare { get; set; } = DateTime.Now;
        public Utilizator? Client { get; set; }
        public Utilizator? Angajat { get; set; }
        public Serviciu? Serviciu { get; set; }
        public Plata? Plata { get; set; }
    }
}
