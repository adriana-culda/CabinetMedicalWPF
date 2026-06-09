using CabinetMedical.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetMedical.ViewModels
{
    public class DashboardViewModel : ObservableObject
    {
        private readonly AppDbContext _context = new AppDbContext();

        public int TotalProgramari => _context.Programari.Count();
        public int TotalPacienti => _context.Utilizatori.Count(u => u.Rol == "Client");
        public decimal TotalIncasari => _context.Plati.Sum(p => p.Suma);

        public DashboardViewModel() { }
    }
}
