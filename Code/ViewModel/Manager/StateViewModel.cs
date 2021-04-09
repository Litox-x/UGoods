using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UGoods.Essences;
using UGoods.Model;
using UGoods.View;
using UGoods.View.Manager;

namespace UGoods.ViewModel.Manager
{
    
    public class StateViewModel : INotifyPropertyChanged
    {
        private BindingList<PInf> StateList = new BindingList<PInf>();
        public static PInf selecteduser = new PInf();
        
        public PInf SelectedUser { get=>selecteduser; set=>selecteduser=value; }
        
        public BindingList<PInf> _StateList
        {
            get { return StateList; }
        }

        public ICommand ToRefreshList
        {
            get
            {
                return new DelegateCommand(()=>RefreshList());
            }
        }

        private void RefreshList()
        {
            StateList.Clear();
            using (var context = new MyDbContext())
            {              
                foreach (PersonalInfo user in context.PersonalInfo.ToList())
                {
                    StateList.Add(new PInf());
                    StateList[StateList.Count() - 1].Name = user.Name;
                    StateList[StateList.Count() - 1].ID = user.Id;
                    StateList[StateList.Count() - 1].Role = user.Role;
                }
            }

            OnPropertyChanged("_StateList");
        }

        public ICommand ToDeleteUser
        {
            get
            {
                return new DelegateCommand(() => DeleteUserCom());
            }
        }

        public async void DeleteUserCom()
        {
            await Task.Run(() => DeleteUser());
        }

        public void DeleteUser()
        {
            if (selecteduser.Role == null)
            {
                View.myMessageBox.Show("Сперва выберите пользователя");
                RefreshList();
            }
            else
            if ((selecteduser.Role == "Manager" && selecteduser.ID!=User.getInstance().Id && User.getInstance().Id!=1) || SelectedUser.ID==1)
            {
                View.myMessageBox.Show("Недостаточно прав");
            }
            else
            {

                var k = MessageBox.Show("Вы уверены?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (k == MessageBoxResult.Yes)
                {
                    using (var context = new MyDbContext())
                    {
                        foreach(SellInfo sellInfo in context.SellInfo.ToList())
                        {
                            if (sellInfo.Seller_Id == SelectedUser.ID)
                                context.SellInfo.Remove(sellInfo);
                        }
                        context.RegIn.Remove(context.RegIn.Find(selecteduser.ID));
                        context.PersonalInfo.Remove(context.PersonalInfo.Find(selecteduser.ID));
                        context.SaveChanges();
                    }

                }


            }
        }

        public ICommand ToEditUser
        {
            get
            {
                return new DelegateCommand(() => EditUser());
            }
        }

        public void EditUser()
        {
            if (selecteduser.Role == null)
            {
                View.myMessageBox.Show("Сперва выберите пользователя");
                RefreshList();
            }

            else
            
            if (selecteduser.Role == "Manager" && selecteduser.ID != User.getInstance().Id && User.getInstance().Id != 1)
            {
                View.myMessageBox.Show("Недостаточно прав");
            }

            else
                new UserEdit().Show();
        }

        public ICommand GoBacktoMenu
        {
            get
            {
                return new DelegateCommand(() => BacktoMenu());
            }
        }

        public void BacktoMenu()
        {
            App.Current.MainWindow.Content = new View.Manager.ManagerMenu();
        }

        public ICommand ToAddNewUser
        {
            get
            {
                return new DelegateCommand(() => AddNewUser());
            }
        }

        public void AddNewUser()
        {
            new NewUserAdd().Show();            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
