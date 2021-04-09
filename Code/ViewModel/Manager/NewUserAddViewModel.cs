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
    
    class NewUserAddViewModel : INotifyPropertyChanged,IDataErrorInfo
    {
        private string firstname="";
        private string surname=""; //фамилия
        private string middlename=""; //отчество

        private bool rolemanag;
        private bool rolecashier=true;

        private string login="";
        private string password="";

        

        public string Firstname { get => firstname; set => firstname = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Middlename { get => middlename; set => middlename = value; }

        public bool Rolemanag { get => rolemanag; set => rolemanag = value; }
        public bool Rolecashier { get => rolecashier; set => rolecashier = value; }

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }


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
            using( var context = new MyDbContext())
            {
                foreach (RegIn item in context.RegIn.ToList())
                {
                    if (item.Login == column)
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
            else return null;

        }



        public ICommand ToRegisterNewUser
        {
            get
            {
                return new DelegateCommand(() => Register());
            }
        }


        public void Register()
        {
            using (var context = new MyDbContext())
            {
                    RegIn NewUser = new RegIn();
                    NewUser.Login = Login;
                    NewUser.Password = ViewModel.MainWindowViewModel.getSHA256(Password);
                    context.RegIn.Add(NewUser);
                    context.SaveChanges();
                    PersonalInfo NewUserInfo = new PersonalInfo();
                    NewUserInfo.Id = NewUser.Id;
                    NewUserInfo.Name = Surname + " " + Firstname + " " + Middlename;
                    if (Rolecashier)
                        NewUserInfo.Role = "Cashier";
                    else if (Rolemanag)
                        NewUserInfo.Role = "Manager";
                    context.PersonalInfo.Add(NewUserInfo);
                    context.SaveChanges();
                    View.myMessageBox.Show("Добавление прошло успешно");
                    foreach (Window window in App.Current.Windows)
                    {
                    if (window is NewUserAdd)
                        window.Close();
                    }
                
            }
        }



        //PropertyChanged,Valid
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

    }
}
