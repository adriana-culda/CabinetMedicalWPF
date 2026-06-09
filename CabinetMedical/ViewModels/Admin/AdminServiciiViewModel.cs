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
using Microsoft.EntityFrameworkCore;

namespace CabinetMedical.ViewModels.Admin
{
    public partial class AdminServiciiViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        private ObservableCollection<Serviciu> _servicii = new();
        public ObservableCollection<Serviciu> Servicii
        {
            get => _servicii;
            set => SetProperty(ref _servicii, value);
        }

        private Serviciu? _selectedServiciu;
        public Serviciu? SelectedServiciu
        {
            get => _selectedServiciu;
            set => SetProperty(ref _selectedServiciu, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ServiciiView?.Refresh();
                }
            }
        }

        public ICollectionView? ServiciiView { get; private set; }

      
        public AdminServiciiViewModel()
        {
            _context = new AppDbContext();
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            _context.ChangeTracker.Clear();

            //aduce datele despre doctor
            var lista = _context.Servicii
                .Include(s => s.Doctor)
                .ToList();

            Servicii.Clear();
            foreach (var s in lista)
            {
                Servicii.Add(s);
            }

            if (ServiciiView == null)
            {
                ServiciiView = CollectionViewSource.GetDefaultView(Servicii);
                ServiciiView.Filter = FiltrareServicii;
            }
        }

        private bool FiltrareServicii(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            if (obj is Serviciu s)
            {
                return (s.Denumire != null && s.Denumire.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                       (s.Categorie != null && s.Categorie.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        
        [RelayCommand]
        public void AddServiciu()
        {
            var fereastra = new Views.Admin.ServiciuModalWindow();
            fereastra.DataContext = new ServiciuModalViewModel();
            fereastra.ShowDialog();
            IncarcaDate();
        }

        [RelayCommand]
        public void EditServiciu()
        {
            if (SelectedServiciu == null)
            {
                MessageBox.Show("Te rog să selectezi un serviciu din tabel mai întâi!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var fereastra = new Views.Admin.ServiciuModalWindow();
            fereastra.DataContext = new ServiciuModalViewModel(SelectedServiciu);
            fereastra.ShowDialog();
            IncarcaDate();
        }

        [RelayCommand]
        public void RemoveServiciu()
        {
            if (SelectedServiciu == null)
            {
                MessageBox.Show("Te rog să selectezi un serviciu din tabel mai întâi!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi definitiv serviciul '{SelectedServiciu.Denumire}'?",
                "Confirmare Ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmare == MessageBoxResult.Yes)
            {
                _context.Servicii.Remove(SelectedServiciu);
                _context.SaveChanges();
                IncarcaDate();
            }
        }
    }
}