using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions; 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CabinetMedical.Models;
using CabinetMedical.Data;
using Microsoft.EntityFrameworkCore;

namespace CabinetMedical.ViewModels.Client
{
    public partial class ClientProgramareNouaViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly Utilizator _currentUser;

        public ObservableCollection<Serviciu> Servicii { get; set; }

        private Serviciu? _selectedServiciu;
        public Serviciu? SelectedServiciu { get => _selectedServiciu; set => SetProperty(ref _selectedServiciu, value); }

        private DateTime _dataProgramare = DateTime.Today;
        public DateTime DataProgramare { get => _dataProgramare; set => SetProperty(ref _dataProgramare, value); }

        private string _oraProgramare = "10:00";
        public string OraProgramare { get => _oraProgramare; set => SetProperty(ref _oraProgramare, value); }

        public ClientProgramareNouaViewModel(Utilizator user)
        {
            _context = new AppDbContext();
            _context.ChangeTracker.Clear();
            _currentUser = user;
            Servicii = new ObservableCollection<Serviciu>(_context.Servicii.ToList());
        }

        [RelayCommand]
        public void SalveazaProgramare()
        {
            //validare serviciu
            if (SelectedServiciu == null)
            {
                MessageBox.Show("Te rugăm să alegi un serviciu!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //validare format ora
            if (!Regex.IsMatch(OraProgramare, @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$"))
            {
                MessageBox.Show("Ora trebuie scrisă în formatul HH:mm (ex: 08:30).", "Format Incorect", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //validare interval orar (08:00 - 16:00)
            TimeSpan ora = TimeSpan.Parse(OraProgramare);
            TimeSpan inceputProgram = new TimeSpan(8, 0, 0);
            TimeSpan sfarsitProgram = new TimeSpan(16, 0, 0);

            if (ora < inceputProgram || ora >= sfarsitProgram)
            {
                MessageBox.Show("Programul cabinetului este între orele 08:00 și 16:00. Vă rugăm alegeți o oră validă.", "Ora Invalida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime startNou = DataProgramare.Date.Add(ora);
            DateTime endNou = startNou.AddMinutes(SelectedServiciu.DurataMInute);

            //verificare suprapunere
            var programariDinZiuaAleasa = _context.Programari
                .Include(p => p.Serviciu)
                .Where(p => p.DataOra.Date == startNou.Date && p.Status != "Anulata")
                .ToList();

            bool existaSuprapunere = false;
            foreach (var programareExistenta in programariDinZiuaAleasa)
            {
                DateTime startExistent = programareExistenta.DataOra;
                int durataExistenta = programareExistenta.Serviciu?.DurataMInute ?? 30;
                DateTime endExistent = startExistent.AddMinutes(durataExistenta);

                if (startNou < endExistent && startExistent < endNou)
                {
                    existaSuprapunere = true;
                    break;
                }
            }

            if (existaSuprapunere)
            {
                MessageBox.Show("Acest interval orar se suprapune cu o altă programare!", "Interval Indisponibil", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //salvare
            var programare = new Programare
            {
                ClientId = _currentUser.Id,
                ServiciuId = SelectedServiciu.Id,
                AngajatId = SelectedServiciu.DoctorId,
                DataOra = startNou,
                Status = "În așteptare"
            };

            _context.Programari.Add(programare);
            _context.SaveChanges();

            MessageBox.Show("Programarea ta a fost salvată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}