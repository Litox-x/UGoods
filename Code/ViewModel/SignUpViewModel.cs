using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using UGoods.Essences;
using UGoods.Model;
using UGoods.View;
using Microsoft.EntityFrameworkCore;

namespace UGoods.ViewModel
{
   
    public class SignUpViewModel : INotifyPropertyChanged
    {
        public static string pacc;
        SignUpWin signup;
        

        public SignUpViewModel(SignUpWin k)
        {
            signup = k;
            _login = "";
            _password = "";
        }

        private string _login;
        private string _password;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public ICommand CAuthorize
        {
            get
            {
                return new DelegateCommand(() => Authorize());
            }
        }

        public void Authorize()
        {
            using (var context = new MyDbContext())
            {
                bool k = false;
                foreach (RegIn user in context.RegIn.Include("PersonalInfo").ToList())
                {
                    if (user.Login == Login && user.Password == ViewModel.MainWindowViewModel.getSHA256(pacc))
                    {
                        User sample = User.getInstance(user.Id, user.PersonalInfo.Name, user.PersonalInfo.Role);

                        k = true;
                        if (user.PersonalInfo.Role == "Manager")
                        {
                            var Mainwin = new MainWindow();
                            Mainwin.MainFrame.Content = new View.Manager.ManagerMenu();
                            App.Current.MainWindow = Mainwin;
                            Mainwin.Show();
                            myMessageBox.Show("Добро пожаловать, " + sample.Name);
                            signup.Close();
                        }
                        else
                        if (user.PersonalInfo.Role == "Cashier")
                        {
                            var Mainwin = new MainWindow();
                            Mainwin.MainFrame.Content = new View.Cashier.CashierMenu();
                            App.Current.MainWindow = Mainwin;
                            Mainwin.Show();
                            myMessageBox.Show("Добро пожаловать, " + sample.Name);

                            signup.Close();
                        }
                    }
                }
                if (!k)
                {
                    myMessageBox.Show("Неверный логин/пароль");
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
