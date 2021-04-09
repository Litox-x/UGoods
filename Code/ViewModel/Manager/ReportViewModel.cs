using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UGoods.Essences;

namespace UGoods.ViewModel.Manager
{
    class ReportViewModel
    {
        public List<SellInfo> getList()
        {
            using (var context = new MyDbContext())
            {
                return context.SellInfo.ToList();
            }
        }

            public ICommand CGenerate
            {
                get
                {
                    return new DelegateCommand(() => GenerateAsync());
                }
            }

        public ICommand CGenerateSells
        {
            get
            {
                return new DelegateCommand(() => GenerateAsyncSells());
            }
        }

        public ICommand ToGoBack
        {
            get
            {
                return new DelegateCommand(() => GoBack());
            }
        }

        public void GoBack()
        {
            App.Current.MainWindow.Content = new View.Manager.ManagerMenu();
        }

        public async void GenerateAsyncSells()
        {
            View.myMessageBox.Show("Начата асинхронная генерация отчёта. Можете продолжить работу");
            await Task.Run(() => Service.ReportService.GenerateReportSells(getList()));
        }

        public async void GenerateAsync()
            {
                View.myMessageBox.Show("Начата асинхронная генерация отчёта. Можете продолжить работу");
                await Task.Run(() => Service.ReportService.GenerateReport(getList()));
            }


        
    }
}
