using CabinetMedical.Data;
using CabinetMedical.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CabinetMedical.ViewModels.Angajat
{
    public partial class AngajatDashboardViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly Utilizator _currentUser;
        private string _dataAzi = string.Empty;
        public string DataAzi
        {
            get => _dataAzi;
            set => SetProperty(ref _dataAzi, value);
        }

        private int _totalProgramariAzi;
        public int TotalProgramariAzi
        {
            get => _totalProgramariAzi;
            set => SetProperty(ref _totalProgramariAzi, value);
        }

        private ObservableCollection<dynamic> _programariAzi = new();
        public ObservableCollection<dynamic> ProgramariAzi
        {
            get => _programariAzi;
            set => SetProperty(ref _programariAzi, value);
        }

        public AngajatDashboardViewModel(Utilizator currentUser)
        {
            _context = new AppDbContext();
            _currentUser = currentUser;
            DataAzi = DateTime.Now.ToString("dd MMMM yyyy");

            IncarcaDate();
        }

        private void IncarcaDate()
        {
            _context.ChangeTracker.Clear();

            var azi = DateTime.Today;
            var maine = azi.AddDays(1);

            //toate programarile de azi
            var query = _context.Programari
                .Include(p => p.Client)
                .Include(p => p.Serviciu)
                .Where(p => p.DataOra >= azi && p.DataOra < maine && p.Status != "Anulata");

           
            //daca este asistenta vede tot.
            if (_currentUser.Rol == "Doctor")
            {
                query = query.Where(p => p.AngajatId == _currentUser.Id);
            }

            var programariReale = query.OrderBy(p => p.DataOra).ToList();

            ProgramariAzi.Clear();

            foreach (var p in programariReale)
            {
                ProgramariAzi.Add(new
                {
                    Ora = p.DataOra.ToString("HH:mm"),
                    NumePacient = p.Client != null ? $"{p.Client.Nume} {p.Client.Prenume}" : "Fără nume",
                    NumeServiciu = p.Serviciu != null ? p.Serviciu.Denumire : "Fără serviciu",
                    Status = p.Status
                });
            }

            TotalProgramariAzi = programariReale.Count;
        }
    }
}