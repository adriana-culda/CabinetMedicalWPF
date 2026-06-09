using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Angajat
{
    public partial class AngajatProgramariViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly Utilizator _currentUser;

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); IncarcaProgramari(); }
        }

        private bool _isObservatiiVisible = false;
        public bool IsObservatiiVisible
        {
            get => _isObservatiiVisible;
            set => SetProperty(ref _isObservatiiVisible, value);
        }

        private string _observatiiText = string.Empty;
        public string ObservatiiText
        {
            get => _observatiiText;
            set => SetProperty(ref _observatiiText, value);
        }


        private ObservableCollection<Programare> _programari = new();
        public ObservableCollection<Programare> Programari
        {
            get => _programari;
            set => SetProperty(ref _programari, value);
        }

        private Programare? _selectedProgramare;
        public Programare? SelectedProgramare
        {
            get => _selectedProgramare;
            set => SetProperty(ref _selectedProgramare, value);
        }

        public AngajatProgramariViewModel(Utilizator currentUser)
        {
            _context = new AppDbContext();
            _currentUser = currentUser;
            IncarcaProgramari();
        }

        private void IncarcaProgramari()
        {
            try
            {
                _context.ChangeTracker.Clear();

                int idFiltru = (_currentUser.Rol == "Asistentă" && _currentUser.DoctorAsociatId.HasValue)
                                ? _currentUser.DoctorAsociatId.Value
                                : _currentUser.Id;

                
                var query = _context.Programari
                    .Include(p => p.Client)
                    .Include(p => p.Serviciu)
                    .Where(p => p.AngajatId == idFiltru); 

                //cautarea 
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    string text = SearchText.ToLower();
                    query = query.Where(p =>
                        (p.Client != null && (p.Client.Nume.ToLower().Contains(text) || p.Client.Prenume.ToLower().Contains(text))) ||
                        (p.Serviciu != null && p.Serviciu.Denumire.ToLower().Contains(text)) ||
                        p.Status.ToLower().Contains(text)
                    );
                }

                var lista = query.OrderByDescending(p => p.DataOra).ToList();

                Programari.Clear();
                foreach (var p in lista) { Programari.Add(p); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcare: {ex.Message}");
            }
        }

    
        [RelayCommand]
        public void FinalizareProgramare()
        {
            if (SelectedProgramare == null)
            {
                MessageBox.Show("Te rog să selectezi o programare din tabel!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //se iau observatiile vechi 
            ObservatiiText = SelectedProgramare.Observatii ?? string.Empty;
            IsObservatiiVisible = true;
        }

        //se salveaza observatiile si schimba statusul
        [RelayCommand]
        public void SalveazaObservatii()
        {
            try
            {
                if (SelectedProgramare != null)
                {
                    var programareDb = _context.Programari.Find(SelectedProgramare.Id);
                    if (programareDb != null)
                    {
                        programareDb.Status = "Finalizata";
                        programareDb.Observatii = ObservatiiText;
                        _context.SaveChanges();
                        IncarcaProgramari();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare: {ex.Message}");
            }
            finally
            {
                IsObservatiiVisible = false; 
            }
        }

        //inchide fara sa se salveze
        [RelayCommand]
        public void InchideObservatii()
        {
            IsObservatiiVisible = false;
        }


        //partea de anulare
        [RelayCommand]
        public void AnulareProgramare()
        {
            if (SelectedProgramare == null) return;
            var confirmare = MessageBox.Show($"Ești sigur că vrei să anulezi programarea?", "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmare == MessageBoxResult.Yes)
            {
                var programareDb = _context.Programari.Find(SelectedProgramare.Id);
                if (programareDb != null)
                {
                    programareDb.Status = "Anulata";
                    _context.SaveChanges();
                    IncarcaProgramari();
                }
            }
        }
    }
}