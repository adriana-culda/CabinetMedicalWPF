using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Client
{
    public partial class ClientServiciiViewModel : ObservableObject
    {
        private ObservableCollection<Serviciu> _toateServiciile = new();
        public ObservableCollection<Serviciu> ListaServicii { get; set; } = new();

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    Filtreaza();
                }
            }
        }

        public ClientServiciiViewModel()
        {
            using (var context = new AppDbContext())
            {
                _toateServiciile = new ObservableCollection<Serviciu>(context.Servicii.ToList());
            }
            Filtreaza();
        }

        private void Filtreaza()
        {
            ListaServicii.Clear();

            //se salveaza textul cautat
            string cautare = SearchText?.ToLower() ?? "";

            var filtered = string.IsNullOrWhiteSpace(cautare)
                ? _toateServiciile
                : _toateServiciile.Where(s =>
                    (s.Denumire != null && s.Denumire.ToLower().Contains(cautare)) ||
                    (s.Categorie != null && s.Categorie.ToLower().Contains(cautare))
                );

            foreach (var item in filtered)
            {
                ListaServicii.Add(item);
            }
        }
    }
}