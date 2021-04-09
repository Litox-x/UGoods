using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UGoods.ViewModel.Cashier
{
    public class CashierMenuViewModel
    {

        public ICommand ToSignOut
        {
            get
            {
                return new DelegateCommand(() => SignOut());
            }
        }

        public ICommand GoToStoragePage
        {
            get
            {
                return new DelegateCommand(() => StartStoragePage());
            }
        }

        public ICommand ToCashReport
        {
            get
            {
                return new DelegateCommand(() => CashReport());
            }
        }

        private void StartStoragePage()
        {
            App.Current.MainWindow.Content = new View.Cashier.Storage.Storage();
        }

        private void CashReport()
        {
            App.Current.MainWindow.Content = new View.Cashier.CashierReport();
        }

        private void SignOut()
        {
            Model.User.Instance = null;
            new View.SignUpWin().Show();
            App.Current.MainWindow.Close();

        }

    }
}
