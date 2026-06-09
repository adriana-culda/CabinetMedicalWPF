using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CabinetMedical.Models;
using CabinetMedical.Data;

namespace CabinetMedical.ViewModels.Admin
{
    public partial class UtilizatorModalViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly bool _esteEditare;

        private Utilizator? _utilizatorCurent;
        public Utilizator UtilizatorCurent
        {
            get => _utilizatorCurent;
            set => SetProperty(ref _utilizatorCurent, value);
        }

        public ObservableCollection<Utilizator> ListaDoctori { get; set; }

        public UtilizatorModalViewModel(Utilizator? utilizator = null)
        {
            _context = new AppDbContext();

            //se incarca doctorii 
            var doctoriDb = _context.Utilizatori
                .Where(u => u.Rol != null && u.Rol == "Doctor")
                .ToList();
            ListaDoctori = new ObservableCollection<Utilizator>(doctoriDb);

            if (utilizator == null)
            {
                UtilizatorCurent = new Utilizator();
                _esteEditare = false;
            }
            else
            {
                UtilizatorCurent = utilizator;
                _esteEditare = true;
            }
        }

        [RelayCommand]
        public void Save(Window fereastra)
        {
            if (string.IsNullOrWhiteSpace(UtilizatorCurent.NumeUtilizator))
            {
                MessageBox.Show("Numele de utilizator este obligatoriu!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //daca se salveaza contul și NU este asistenta se goleste DoctorAsociatId
            if (UtilizatorCurent.Rol != "Asistentă")
            {
                UtilizatorCurent.DoctorAsociatId = null;
            }

            if (!_esteEditare)
            {
                _context.Utilizatori.Add(UtilizatorCurent);
            }
            else
            {
                _context.Utilizatori.Update(UtilizatorCurent);
            }

            _context.SaveChanges();
            fereastra?.Close();
        }

        [RelayCommand]
        public void Cancel(Window fereastra)
        {
            fereastra?.Close();
        }
    }
}