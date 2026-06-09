using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CabinetMedical.ViewModels;

namespace CabinetMedical.Views
{
    /// <summary>
    /// Interaction logic for MainShellView.xaml
    /// </summary>
    public partial class MainShellView : UserControl
    {
        public MainShellView()
        {
            InitializeComponent();
        }
        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button)?.Tag?.ToString();
            if (DataContext is MainShellViewModel vm)
            {
               vm.Navigate(    tag);
            }
        }
    }
}
