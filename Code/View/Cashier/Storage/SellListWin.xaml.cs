using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UGoods.ViewModel.Cashier;

namespace UGoods.View.Cashier.Storage
{
    /// <summary>
    /// Логика взаимодействия для SellListWin.xaml
    /// </summary>
    public partial class SellListWin : Window
    {
        public SellListWin(StorageViewModel baseVm)
        {
            InitializeComponent();
            DataContext = baseVm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
