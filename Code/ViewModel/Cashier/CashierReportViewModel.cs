using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UGoods.Essences;
using UGoods.Model;
using System.Windows.Input;

namespace UGoods.ViewModel.Cashier
{
    class CashierReportViewModel : INotifyPropertyChanged
    {
        public ICommand ToGoBack
        {
            get
            {
                return new DelegateCommand(() => GoBack());
            }
        }

        public void GoBack()
        {
            App.Current.MainWindow.Content = new View.Cashier.CashierMenu();
        }

        public SeriesCollection ColumnValues { get; set; }

        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
       
        public CashierReportViewModel()
        {
            DateTime _date = DateTime.Now;
            ColumnValues = new SeriesCollection();
             double sum = 0;
            using (var context = new MyDbContext())
            {
                foreach (SellInfo selinf in context.SellInfo.ToList())
                {
                    if (selinf.Seller_Id == User.getInstance().Id)
                    {
                        Console.WriteLine(selinf.Name_of_Sellgoods);
                        ColumnValues.Add(new ColumnSeries
                        {
                            Title = selinf.Name_of_Sellgoods,
                            Values = new ChartValues<double> { selinf.Count_of_Sellgoods }
                        });
                        sum += selinf.Count_of_Sellgoods;
                    }
                }
            }

            Labels = new[] { "Промежуток времени" };
            Formatter = value => value.ToString("N");
            View.myMessageBox.Show("Общее количество проданных товаров: "+sum.ToString());
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
