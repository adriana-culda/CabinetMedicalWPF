using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Angajat
{
    public partial class AngajatPacientiViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        //campul pentru bara de cautare
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FiltreazaPacienti(); //filtreaza automat
            }
        }


        private ObservableCollection<Utilizator> _pacienti = new();
        public ObservableCollection<Utilizator> Pacienti
        {
            get => _pacienti;
            set => SetProperty(ref _pacienti, value);
        }

        private ObservableCollection<Programare> _istoricPacient = new();
        public ObservableCollection<Programare> IstoricPacient
        {
            get => _istoricPacient;
            set => SetProperty(ref _istoricPacient, value);
        }

        //pacientul selectat curent
        private Utilizator? _selectedPacient;
        public Utilizator? SelectedPacient
        {
            get => _selectedPacient;
            set
            {
                SetProperty(ref _selectedPacient, value);
                IncarcaIstoric(); //aduce istoricul
            }
        }

        public AngajatPacientiViewModel()
        {
            _context = new AppDbContext();
            IncarcaPacienti();
        }

        private void IncarcaPacienti()
        {
            _context.ChangeTracker.Clear();

            //se aduc pacientii
            var lista = _context.Utilizatori
                .Where(u => u.Rol == "Client" || u.Rol == "Pacient")
                .OrderBy(u => u.Nume)
                .ToList();

            Pacienti = new ObservableCollection<Utilizator>(lista);
        }

        private void FiltreazaPacienti()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                IncarcaPacienti();
            }
            else
            {
                var filtered = _context.Utilizatori
                    .Where(u => (u.Rol == "Client" || u.Rol == "Pacient") &&
                               (u.Nume.ToLower().Contains(SearchText.ToLower()) ||
                                u.Prenume.ToLower().Contains(SearchText.ToLower())))
                    .ToList();
                Pacienti = new ObservableCollection<Utilizator>(filtered);
            }
        }

        private void IncarcaIstoric()
        {
            IstoricPacient.Clear();

            if (SelectedPacient != null)
            {
                var istoric = _context.Programari
                    .Include(p => p.Serviciu)
                    .Include(p => p.Angajat) 
                    .Where(p => p.ClientId == SelectedPacient.Id)
                    .OrderByDescending(p => p.DataOra)
                    .ToList();

                foreach (var item in istoric)
                {
                    IstoricPacient.Add(item);
                }
            }
        }
    }
}