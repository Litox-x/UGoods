using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using UGoods.View;

namespace UGoods.ViewModel.Manager
{
    class ManagerMenuViewModel : INotifyPropertyChanged
    {
        public ICommand ToSignOut
        {
            get
            {
                return new DelegateCommand(() => SignOut());
            }
        }

        public ICommand ToGetReport
        {
            get
            {
                return new DelegateCommand(()=>GetReport());
            }
        }

        public ICommand GoToStatePage
        {
            get
            {
                return new DelegateCommand(() => StartStatePage());
            }
        }

        public ICommand GoToStoragePage
        {
            get
            {
                return new DelegateCommand(() => StartStoragePage());
            }
        }

        private void StartStatePage()
        {
           App.Current.MainWindow.Content = new View.Manager.State();
        }

        private void StartStoragePage()
        {
            App.Current.MainWindow.Content = new View.Manager.Storage.Storage();
        }

        private void GetReport()
        {
            App.Current.MainWindow.Content = new View.Manager.Report.Report();
        }

        private void SignOut()
        {
            Model.User.Instance = null;
            new View.SignUpWin().Show();
            App.Current.MainWindow.Close();
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        

    }
}
