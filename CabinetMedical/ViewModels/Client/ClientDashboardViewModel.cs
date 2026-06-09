using System;
using System.Linq;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CabinetMedical.Models;
using CabinetMedical.Data;
using Microsoft.EntityFrameworkCore;

namespace CabinetMedical.ViewModels.Client
{
    public partial class ClientDashboardViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly Utilizator _currentUser;
        private int _programariActive;
        public int ProgramariActive
        {
            get => _programariActive;
            set => SetProperty(ref _programariActive, value);
        }

        private string _urmatoareaProgramare = "Nicio programare";
        public string UrmatoareaProgramare
        {
            get => _urmatoareaProgramare;
            set => SetProperty(ref _urmatoareaProgramare, value);
        }
        private ObservableCollection<Programare> _programariViitoare = new();
        public ObservableCollection<Programare> ProgramariViitoare
        {
            get => _programariViitoare;
            set => SetProperty(ref _programariViitoare, value);
        }

        public ClientDashboardViewModel(Utilizator user)
        {
            _context = new AppDbContext();
            _currentUser = user;
            IncarcaStatistici();
        }

        private void IncarcaStatistici()
        {
            
            _context.ChangeTracker.Clear();

            //se extrag programarile 
            var programari = _context.Programari
                .Include(p => p.Serviciu)
                .Where(p => p.ClientId == _currentUser.Id && p.Status != "Anulata")
                .OrderBy(p => p.DataOra)
                .ToList();

            //se calculeaza cate sunt active
            var listaViitoare = programari.Where(p => p.DataOra >= DateTime.Now).ToList();
            ProgramariActive = listaViitoare.Count;

            //urmatoarea programare
            var viitoare = listaViitoare.FirstOrDefault();

            if (viitoare != null)
            {
                UrmatoareaProgramare = viitoare.DataOra.ToString("dd MMM, HH:mm");
            }
            else
            {
                UrmatoareaProgramare = "Nu aveți programări viitoare";
            }

            //se populeaza tabelul
            ProgramariViitoare = new ObservableCollection<Programare>(listaViitoare);
        }
    }
}