using CabinetMedical.Data;
using CabinetMedical.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CabinetMedical
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            using (var context = new CabinetMedical.Data.AppDbContext())
            {
                context.Database.EnsureCreated(); //verifica dacă fisierul bazei de date exista
            }
            //cum se va desena graficul
            LiveCharts.Configure(config =>
                config.AddSkiaSharp().AddDefaultMappers().AddLightTheme());
        }
    }

}
