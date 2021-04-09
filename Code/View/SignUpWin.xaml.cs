using Microsoft.EntityFrameworkCore;
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
using UGoods.Essences;
using UGoods.ViewModel;

namespace UGoods.View
{
    /// <summary>
    /// Логика взаимодействия для SignUpWin.xaml
    /// </summary>
    public partial class SignUpWin : Window
    {
        public SignUpWin()
        {
            InitializeComponent();
            using (var db = new MyDbContext())
            {
                db.Database.Migrate();
            }
            DataContext = new ViewModel.SignUpViewModel(this);
        }
        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            SignUpViewModel.pacc = Password.Password;
            HiddenPass.Text = Password.Password;
            HiddenPass.UpdateLayout();
        }

    }
}
