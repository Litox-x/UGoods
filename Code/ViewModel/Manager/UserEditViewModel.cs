using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UGoods.Essences;
using UGoods.Model;
using UGoods.View.Manager;

namespace UGoods.ViewModel.Manager
{
    public class UserEditViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private PInf selecteduser = StateViewModel.selecteduser;
        public PInf SelectedUser { get => selecteduser; set { selecteduser = value; OnPropertyChanged("SelectedUser"); } }

        private string firstname = " ";
        private string surname = ""; //фамилия
        private string middlename = ""; //отчество

        private bool rolemanag;
        private bool rolecashier = true;

        private string login = "";
        private string password = "";


        public string Firstname { get => firstname; set { firstname = value; OnPropertyChanged("Firstname"); } }
        public string Surname { get => surname; set { surname = value; OnPropertyChanged("Surname"); } }
        public string Middlename { get => middlename; set { middlename = value; OnPropertyChanged("Middlename"); } }

        public bool Rolemanag { get => rolemanag; set { rolemanag = value; OnPropertyChanged("Rolemanag"); } }
        public bool Rolecashier { get => rolecashier; set { rolecashier = value; OnPropertyChanged("Rolecashier"); } }

        public string Login { get => login; set { login = value; OnPropertyChanged("Login"); } }
        public string Password { get => password; set { password = value; OnPropertyChanged("Password"); } }
        
        public UserEditViewModel()
        {
            SelectedUser = ViewModel.Manager.StateViewModel.selecteduser;
          
        }

        public string this[string columnName]
        {
            get
            {
                string result = String.Empty;
                switch (columnName)
                {

                    case ("Firstname"):
                        if (CheckValid(Firstname) != null)
                            result = CheckValid(Firstname);
                        break;
                    case ("Surname"):
                        if (CheckValid(Surname) != null)
                            result = CheckValid(Surname);
                        break;
                    case ("Middlename"):
                        if (CheckValid(Middlename) != null)
                            result = CheckValid(Middlename);
                        break;
                    case ("Login"):
                        if (CheckValidLog(Login) != null)
                            result = CheckValidLog(Login);
                        break;
                    case ("Password"):
                        if (CheckValidPass(Password) != null)
                            result = CheckValidPass(Password);
                        break;
                }
                return result;
            }

        }

        private string CheckValid(string column)
        {
            if (Regex.Match(column, @"[^а-яА-Я]").Success)
                return "Поле должно содержать только кирилицу";
            if (!column.Take(1).All(Char.IsUpper))
                return "Первая буква должна быть заглавной";
            if (column.Split(new char[] { ' ' }).Count() > 1)
                return "В поле должно находиться только одно слово(либо присутствует пробел после слова)";
            if (string.IsNullOrEmpty(column))
                return "Поле не может быть пустым";
            if (column.Length < 2)
                return "Имя слишком короткое, либо поле пусто";
            return null;

        }

        private string CheckValidLog(string column)
        {
            if (Regex.Match(column, @"[^A-Za-z0-9_]").Success)
                return "Поле должно содержать только латинские буквы и цифры и _";
            if (column.Split(new char[] { ' ' }).Count() > 1)
                return "В поле должно находиться только одно слово(либо присутствует пробел после слова)";
            if (string.IsNullOrEmpty(column))
                return "Поле не может быть пустым";
            if (column.Length < 4)
                return "Login слишком короткий(4+ символа), либо поле пусто";
            using (var context = new MyDbContext())
            {
                foreach (RegIn item in context.RegIn.ToList())
                {
                    if (item.Login == column && item.Id!=User.getInstance().Id)
                        return "Логин занят";
                }
            }
            return null;

        }

        private string CheckValidPass(string column)
        {
            if (Regex.Match(column, @"[^A-Za-z0-9]").Success == true)
                return "Поле должно содержать только латинские буквы и цифры";
            if (column.Split(new char[] { ' ' }).Count() > 1)
                return "В поле должно находиться только одно слово(либо присутствует пробел после слова)";
            if (string.IsNullOrEmpty(column))
                return "Поле не может быть пустым";
            if (column.Length < 6)
                return "Пароль слишком короткий(6+ символов), либо поле пусто";
            return null;

        }



        public ICommand ToFindUser
        {
            get
            {
                return new DelegateCommand(() => UserFind());
            }
        }

        public ICommand ToSaveChanges
        {
            get
            {
                return new DelegateCommand(() => SaveChanges());
            }
        }

        public void SaveChanges()
        {
            using (var context = new MyDbContext())
            {
                RegIn user = context.RegIn.Find(SelectedUser.ID);

                user.PersonalInfo.Name = Surname + " " + Firstname + " " + Middlename;
                user.Login = Login;
                user.Password = ViewModel.MainWindowViewModel.getSHA256(Password);
                if (Rolecashier)
                    user.PersonalInfo.Role = "Cashier";
                else
                    user.PersonalInfo.Role = "Manager";

                context.SaveChanges();

                foreach (Window window in App.Current.Windows)
                    if (window is UserEdit)
                        window.Close();
            }
        }

        public void UserFind()
        {
            using(var context = new MyDbContext())
            {
                try
                {
                    
                    RegIn user = context.RegIn.Find(SelectedUser.ID);

                    Surname = context.PersonalInfo.Find(SelectedUser.ID).Name.Split(new[] { ' ' })[0];
                    Firstname = context.PersonalInfo.Find(SelectedUser.ID).Name.Split(new[] { ' ' })[1];
                    Middlename = context.PersonalInfo.Find(SelectedUser.ID).Name.Split(new[] { ' ' })[2];

                    Login = user.Login;
                    Password = user.Password;
                    if (context.PersonalInfo.Find(SelectedUser.ID).Role == "Manager")
                    {
                        Rolecashier = false;
                        Rolemanag = true;
                    }
                }
                catch(Exception m)
                {
                    MessageBox.Show(m.Message);
                }
                
            }
        }


        public string Error => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
