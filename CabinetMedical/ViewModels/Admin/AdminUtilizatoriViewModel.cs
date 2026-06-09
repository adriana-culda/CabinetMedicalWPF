using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Admin
{
    public partial class AdminUtilizatoriViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        private ObservableCollection<Utilizator> _utilizatori = new();
        public ObservableCollection<Utilizator> Utilizatori
        {
            get => _utilizatori;
            set => SetProperty(ref _utilizatori, value);
        }

        private Utilizator? _selectedUtilizator;
        public Utilizator? SelectedUtilizator
        {
            get => _selectedUtilizator;
            set => SetProperty(ref _selectedUtilizator, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    //cand textul se schimba actualizam
                    UtilizatoriView?.Refresh();
                }
            }
        }

        public ICollectionView? UtilizatoriView { get; private set; }
        public AdminUtilizatoriViewModel()
        {
            _context = new AppDbContext();
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            
            _context.ChangeTracker.Clear();

            var lista = _context.Utilizatori.ToList();

            // se umple baza de date
           
            Utilizatori.Clear();
            foreach (var u in lista)
            {
                Utilizatori.Add(u);
            }

            //filtrare doar prima data
            if (UtilizatoriView == null)
            {
                UtilizatoriView = CollectionViewSource.GetDefaultView(Utilizatori);
                UtilizatoriView.Filter = FiltrareUtilizatori;
            }
        }

        private bool FiltrareUtilizatori(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            if (obj is Utilizator u)
            {
                return u.NumeUtilizator.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                u.Nume.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                u.Rol.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
                return false;
        }

        
        [RelayCommand]
        public void AddUtilizator()
        {
            var fereastra = new Views.Admin.UtilizatorModalWindow();
            fereastra.DataContext = new UtilizatorModalViewModel();
            fereastra.ShowDialog();
            IncarcaDate();
        }

        [RelayCommand]
        public void EditUtilizator()
        {
            if (SelectedUtilizator == null)
            {
                MessageBox.Show("Selectează un utilizator!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var fereastra = new Views.Admin.UtilizatorModalWindow();
            fereastra.DataContext = new UtilizatorModalViewModel(SelectedUtilizator);
            fereastra.ShowDialog();
            IncarcaDate();
        }

        [RelayCommand]
        public void RemoveUtilizator()
        {
            //verificare
            if (SelectedUtilizator == null)
            {
                MessageBox.Show("Te rog să selectezi un utilizator din tabel mai întâi!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //intrebare
            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi definitiv utilizatorul '{SelectedUtilizator.NumeUtilizator}'?",
                "Confirmare Ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            //sterge
            if (confirmare == MessageBoxResult.Yes)
            {
                _context.Utilizatori.Remove(SelectedUtilizator);
                _context.SaveChanges();
                IncarcaDate();
            }
        }
    
    }
}