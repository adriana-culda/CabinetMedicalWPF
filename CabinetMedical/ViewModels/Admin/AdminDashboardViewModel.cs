using CabinetMedical.Data;
using CabinetMedical.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CabinetMedical.ViewModels.Admin
{
    public partial class AdminDashboardViewModel : ObservableObject
    {
        private readonly AppDbContext _context; //conexiunea la BD

        public int TotalPacienti { get; set; }
        public int TotalProgramari { get; set; }
        public decimal TotalIncasari { get; set; }

        private ISeries[] _series = Array.Empty<ISeries>();
        public ISeries[] Series { get => _series; set => SetProperty(ref _series, value); }

        private Axis[] _xAxes = Array.Empty<Axis>();
        public Axis[] XAxes { get => _xAxes; set => SetProperty(ref _xAxes, value); }

        public AdminDashboardViewModel(Utilizator user)
        {
            _context = new AppDbContext();
            IncarcaStatistici();
        }

        private void IncarcaStatistici()
        {
            //partea de grafic
            var azi = DateTime.Today;
            int zileInLuna = DateTime.DaysInMonth(azi.Year, azi.Month);

            //axa X cu zilele lunii
            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = Enumerable.Range(1, zileInLuna).Select(d => d.ToString()).ToArray()
                }
            };

            double[] valoriProgramari = new double[zileInLuna];
            double[] valoriIncasari = new double[zileInLuna];

            //spun ce sa imi aduca
            var dateLuna = _context.Programari
                .Include(p => p.Serviciu)
                .Where(p => p.DataOra.Month == azi.Month && p.DataOra.Year == azi.Year)
                .ToList();

            for (int i = 0; i < zileInLuna; i++)
            {
                int zi = i + 1;
                //se numara valorile
                valoriProgramari[i] = dateLuna.Count(p => p.DataOra.Day == zi);
                valoriIncasari[i] = (double)dateLuna.Where(p => p.DataOra.Day == zi && p.Status == "Finalizata")
                                                   .Sum(p => p.Serviciu?.Pret ?? 0);
            }

            TotalPacienti = _context.Utilizatori.Count(u => u.Rol == "Pacient" || u.Rol == "Client");
            TotalProgramari = dateLuna.Count;
            TotalIncasari = (decimal)valoriIncasari.Sum();

            Series = new ISeries[]
            {
                new LineSeries<double> { Values = valoriProgramari, Name = "Programări" },
                new LineSeries<double> { Values = valoriIncasari, Name = "Încasări" }
            };
        }
    }
}