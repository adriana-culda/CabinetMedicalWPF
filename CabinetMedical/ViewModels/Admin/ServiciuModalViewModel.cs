using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CabinetMedical.Models;
using CabinetMedical.Data;
using Microsoft.EntityFrameworkCore;

namespace CabinetMedical.ViewModels.Admin
{
    public partial class ServiciuModalViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly bool _esteEditare;

        private Serviciu _serviciuCurent;
        public Serviciu ServiciuCurent
        {
            get => _serviciuCurent;
            set => SetProperty(ref _serviciuCurent, value);
        }

        public ObservableCollection<Utilizator> ListaDoctori { get; set; }

        private Utilizator? _doctorSelectat;
        public Utilizator? DoctorSelectat
        {
            get => _doctorSelectat;
            set => SetProperty(ref _doctorSelectat, value);
        }

        public ServiciuModalViewModel(Serviciu? serviciu = null)
        {
            _context = new AppDbContext();

            var doctoriDb = _context.Utilizatori
                .Where(u => u.Rol != null && u.Rol.ToLower().Contains("doctor"))
                .ToList();

            ListaDoctori = new ObservableCollection<Utilizator>(doctoriDb);

            if (serviciu == null)
            {
                ServiciuCurent = new Serviciu { EsteActiv = true, DurataMInute = 30 };
                _esteEditare = false;
            }
            else
            {
                //se incarca serviciul proaspat 
                ServiciuCurent = _context.Servicii.Find(serviciu.Id) ?? serviciu;
                _esteEditare = true;

                if (ServiciuCurent.DoctorId != null)
                {
                    DoctorSelectat = ListaDoctori.FirstOrDefault(d => d.Id == ServiciuCurent.DoctorId);
                }
            }
        }

        [RelayCommand]
        public void Save(Window fereastra)
        {
            if (string.IsNullOrWhiteSpace(ServiciuCurent.Denumire) ||
                string.IsNullOrWhiteSpace(ServiciuCurent.Categorie) ||
                DoctorSelectat == null)
            {
                MessageBox.Show("Te rog completează toate câmpurile obligatorii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ServiciuCurent.DoctorId = DoctorSelectat.Id;

            if (!_esteEditare)
            {
                _context.Servicii.Add(ServiciuCurent);
            }
            else
            {
                
                _context.Entry(ServiciuCurent).State = EntityState.Modified;
            }

            _context.SaveChanges();
            fereastra?.Close();
        }

        [RelayCommand]
        public void Cancel(Window fereastra) => fereastra?.Close();
    }
}