using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetMedical.Models
{
    public class Plata
    {
        public int Id { get; set; }
        public int ProgramareId { get; set; }
        public int ClientId { get; set; }
        public decimal Suma { get; set; }
        public string MetodaPlata { get; set; } = "Numerar";
        public string Status { get; set; } = "Neplata";
        public DateTime? DataPlata { get; set; }
        public string NumarFactura { get; set; } = string.Empty;
        public Programare? Programare { get; set; }
        public Utilizator? Client { get; set; }
    }
}
