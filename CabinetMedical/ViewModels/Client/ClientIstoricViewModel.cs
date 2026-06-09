using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Client
{
    public partial class ClientIstoricViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly Utilizator _currentUser;
        private ObservableCollection<Programare> _originalList = new();

        public ObservableCollection<Programare> IstoricProgramari { get; set; } = new();

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { if (SetProperty(ref _searchText, value)) Filtreaza(); }
        }

        public ClientIstoricViewModel(Utilizator user)
        {
            _context = new AppDbContext();
            _currentUser = user;
            LoadIstoric();
        }

        private void LoadIstoric()
        {
            var data = _context.Programari
                .Include(p => p.Serviciu)
                .Include(p => p.Angajat)
                .Where(p => p.ClientId == _currentUser.Id && p.Status == "Finalizata")
                .OrderByDescending(p => p.DataOra)
                .ToList();

            _originalList = new ObservableCollection<Programare>(data);
            Filtreaza();
        }

        private void Filtreaza()
        {
            IstoricProgramari.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _originalList
                : _originalList.Where(p => p.Serviciu.Denumire.ToLower().Contains(SearchText.ToLower()) ||
                                           (p.Observatii != null && p.Observatii.ToLower().Contains(SearchText.ToLower())));

            foreach (var item in filtered) IstoricProgramari.Add(item);
        }
    }
}