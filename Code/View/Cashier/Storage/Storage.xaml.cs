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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UGoods.View.Cashier.Storage
{
    /// <summary>
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Page
    {
        public Storage()
        {
            InitializeComponent();
            Cb1.SelectedItem = Cb1.Items[0];
            Cb2.SelectedItem = Cb2.Items[0];
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cb1.SelectedItem = Cb1.Items[0];
            Cb2.SelectedItem = Cb2.Items[0];
        }
    }
}
