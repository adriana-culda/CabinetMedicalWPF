using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Admin
{
    public class AdminRapoarteViewModel : ObservableObject
    {
        //stocheaza programari 
        private ObservableCollection<Programare> _istoricProgramari = new();
        public ObservableCollection<Programare> IstoricProgramari
        {
            get => _istoricProgramari;
            set => SetProperty(ref _istoricProgramari, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { if (SetProperty(ref _searchText, value)) IstoricProgramariView?.Refresh(); }
        }

        public ICollectionView? IstoricProgramariView { get; private set; }

        public AdminRapoarteViewModel()
        {
            IncarcaIstoric();
        }

        private void IncarcaIstoric()
        {
            using (var context = new AppDbContext())
            {
                //programari finalizate
                var date = context.Programari
                                   .Include(p => p.Client)
                                   .Include(p => p.Serviciu)
                                   .Where(p => p.Status == "Finalizata")
                                   .OrderByDescending(p => p.DataOra)
                                   .ToList();

                IstoricProgramari = new ObservableCollection<Programare>(date);
                IstoricProgramariView = CollectionViewSource.GetDefaultView(IstoricProgramari);
                IstoricProgramariView.Filter = FiltrareProgramari;
            }
        }

        private bool FiltrareProgramari(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            if (obj is Programare p)
            {
                return (p.Client != null && (p.Client.Nume.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                                           p.Client.Prenume.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)));
            }
            return false;
        }
    }
}