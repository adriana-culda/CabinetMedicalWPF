using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetMedical.Models
{
    public class JurnalAudit
    {
        public int Id { get; set; }
        public int? UtilizatorId { get; set; }
        public string Actiune { get; set; } = string.Empty;
        public string Entitate { get; set; } = string.Empty;
        public string Detalii { get; set; } = string.Empty;
        public DateTime DataOra { get; set; } = DateTime.Now;
        public string AdresaIP { get; set; } = string.Empty;

        public Utilizator? Utilizator { get; set; }
    }

}
