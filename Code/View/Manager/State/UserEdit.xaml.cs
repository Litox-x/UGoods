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
using UGoods.Model;

namespace UGoods.View.Manager
{
    /// <summary>
    /// Логика взаимодействия для UserEdit.xaml
    /// </summary>
    public partial class UserEdit : Window
    {
        public UserEdit()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContinueButton.Visibility = Visibility.Hidden;
        }
    }
}
