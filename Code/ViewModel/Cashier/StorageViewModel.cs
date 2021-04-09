using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using UGoods.Essences;
using UGoods.Model;

namespace UGoods.ViewModel.Cashier
{
    public class StorageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GoodsInf> StateList = new ObservableCollection<GoodsInf>();
        private ObservableCollection<GoodsInf> SellList = new ObservableCollection<GoodsInf>();
        private ObservableCollection<GoodsInf> SelectedItemList = new ObservableCollection<GoodsInf>();
        public ObservableCollection<GoodsInf> _StateList
        {
            get { return StateList; }
            set { StateList = value; OnPropertyChanged("_Statelist"); }
        }
        public ObservableCollection<GoodsInf> _SellList
        {
            get { return SellList; }
            set { SellList = value; OnPropertyChanged("_SellList"); }
        }
        public ObservableCollection<GoodsInf> _SelectedItemList
        {
            get { return SelectedItemList; }
            set { SelectedItemList = value; OnPropertyChanged("_SelectedItemList"); }
        }


        private GoodsInf selecteditem;
        private TextBlock selectedsort;
        private string nametofind;
        private string counttofind;
        private TextBlock selectedcountsort;
        private string counttoSell;
        public double totalsum = 0;

        public double Totalsum { set { totalsum = value; OnPropertyChanged("Totalsum"); } get => totalsum; }

        private double SumCheck()
        {
            double sum = 0;
            foreach(GoodsInf s in SellList)
            {
                sum += s.Price * s.Count;
            }
            return sum;
        }

        public string CounttoSell { get => counttoSell; set { counttoSell = value; OnPropertyChanged("CounttoSell"); } }
        public GoodsInf SelectedGood { set { selecteditem = value; _SelectedItemList.Clear(); _SelectedItemList.Add(value); OnPropertyChanged("SelectedGood"); } get => selecteditem; }
        public TextBlock SelectedSort { get => selectedsort; set { selectedsort = value; Sortby(); OnPropertyChanged("SelectedSort"); } }
        public string NametoFind { get => nametofind; set { nametofind = value; OnPropertyChanged("NametoFind"); } }
        public string CounttoFind { get => counttofind; set { counttofind = value; OnPropertyChanged("CounttoFind"); } }
        public TextBlock SelectedCountSort { get => selectedcountsort; set { selectedcountsort = value; OnPropertyChanged("SelectedCountSort"); } }

        /*Свойства для ICommand*/
        
        public ICommand ToShowSellList
        {
            get
            {
                return new DelegateCommand(() => ShowSellList());
            }
        }

        public ICommand ToRefreshList
        {
            get
            {
                return new DelegateCommand(() => RefreshList(true));
            }
        }
        
        public ICommand ToCheckandAdd
        { 
            get
            {
                return new DelegateCommand(() => CheckandAdd());
            }
        }

        public ICommand ToSortByName
        {
            get
            {
                return new DelegateCommand(() => SortbyName());
            }
        }

        public ICommand ToSortbyCount
        {
            get
            {
                return new DelegateCommand(() => SortbyCount());
            }
        }

        public ICommand ToSellGoods
        {
            get
            {
                return new DelegateCommand(() => SellGoods());
            }
        }

        public ICommand ToAddtoSellList
        {
            get
            {
                return new DelegateCommand(() => AddtoSellList());
            }
        }

        public ICommand GoBacktoMenu
        {
            get
            {
                return new DelegateCommand(() => BacktoMenu());
            }
        }

        public ICommand ToCancelLastSell
        {
            get
            {
                return new DelegateCommand(() => CancelLastSell());
            }
        }


        /**/

        /*Методы*/
        private void CancelLastSell()
        {
            try
            {
                _SellList.Remove(SellList[SellList.Count - 1]);
            }
            catch
            {
                View.myMessageBox.Show("Список пуст");
            }
        }

        private void ShowSellList()
        {
            new View.Cashier.Storage.CurrentSellList(this).Show();
        }

        private void CheckandAdd()
        {
            if (selecteditem.Type == "шт")
            {
                try
      
          {
                    Convert.ToInt32(CounttoSell);
                }
                catch
                {
                    View.myMessageBox.Show("Введите верное количество в формате (00). Число должно быть целым");
                    return;
                }
            }
            try
            {
               Convert.ToDouble(CounttoSell);
            }
            catch
            {
                View.myMessageBox.Show("Введите верное количество в формате (00.00)");
                return;
            }
            if (Convert.ToDouble(CounttoSell) <= 0)
            {
                View.myMessageBox.Show("Введите количество(больше 0)");
                return;
            }
            if (SelectedGood.Count >= Convert.ToDouble(CounttoSell))
            {
                _SellList.Add(selecteditem);
                _SellList[SellList.Count() - 1].Count = Convert.ToDouble(CounttoSell);
                OnPropertyChanged("_SellList");
                CounttoSell = "";
                Totalsum = SumCheck();
                RefreshList(true);
            }
            else
                View.myMessageBox.Show("Слишком много");
        }

        string texttoprint = "Чек\n\n";
        double sum = 0;

        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(texttoprint, new Font("Times New Roman", 10), Brushes.Black, 0, 0);
        }

        private void SellGoods()
        {
            if (_SellList.Count()==0)
            {
                View.myMessageBox.Show("Список продаж пуст!");
                return;
            }
            texttoprint = "Чек\n\n";
            foreach (GoodsInf good in _SellList)
            {
                texttoprint += good.Name + " (" + good.Count.ToString() + ") " + good.Price.ToString() + "(за шт/кг)\n"+(good.Price*good.Count).ToString()+"\n\n";
                sum+=good.Price*good.Count;
            }
            texttoprint += "Общая сумма чека - "+Math.Round(sum,2).ToString();
            texttoprint += "\n";
            texttoprint += "\n";
            texttoprint += "Кассир - " + User.getInstance().Name;
            texttoprint += "\n" + DateTime.Now.ToString();

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPageHandler;

            PrintDialog print = new PrintDialog();
            
            if (print.ShowDialog() == true)
            {
                printDocument.Print();
                using (var context = new MyDbContext())
                {
                    foreach (GoodsInf k in _SellList)
                    {
                        Goods chg = context.Goods.Find(k.Id);
                        chg.Count -= k.Count;

                        SellInfo selinf = new SellInfo();
                        selinf.Date_of_Sell = DateTime.Now;
                        selinf.Name_of_Sellgoods = k.Name;
                        selinf.Goods_Id = k.Id;
                        selinf.Seller_Id = User.getInstance().Id;
                        selinf.Count_of_Sellgoods = k.Count;

                        context.SellInfo.Add(selinf);
                        context.SaveChanges();
                    }
                    SellList.Clear();
                    Totalsum = 0;
                    RefreshList(true);
                }
            } else
            {
                View.myMessageBox.Show("Ошибка принтера");
                return;
            }
        }
        
        private void AddtoSellList()
        {
            if (SelectedGood == null)
            {
                View.myMessageBox.Show("Выберите товар!");
                return;
            }
            new View.Cashier.Storage.SellListWin(this).Show();
        }

        private void SortbyName()
        {
            if (_StateList.Count == 0)
                RefreshList(false);
            ObservableCollection<GoodsInf> sortedList = new ObservableCollection<GoodsInf>();
            foreach (GoodsInf k in _StateList)
            {
                Regex regex = new Regex(@"" + NametoFind + @"(\w*)");
                MatchCollection matches = regex.Matches(k.Name);
                if (matches.Count > 0)
                {
                    sortedList.Add(k);
                }
            }
            _StateList = sortedList;
        }

        private void Sortby()
        {
            if (_StateList.Count == 0)
                RefreshList(false);
            ObservableCollection<GoodsInf> sortedList = new ObservableCollection<GoodsInf>();

            switch (SelectedSort.Text)
            {
                case ("По наименованию товара"):
                    {

                    
                        var k = from i in _StateList
                                orderby i.Name
                                select i;
                        foreach (GoodsInf sortedel in k)
                        {
                            sortedList.Add(sortedel);
                        }

                    }
                    break;
                case ("По id"):
                    {

                        var k = from i in _StateList
                                orderby i.Id
                                select i;
                        foreach (GoodsInf sortedel in k)
                        {
                            sortedList.Add(sortedel);
                        }
                    }
                    break;
                case ("По количеству"):
                    {

                        var k = from i in _StateList
                                orderby i.Count
                                select i;
                        foreach (GoodsInf sortedel in k)
                        {
                            sortedList.Add(sortedel);
                        }
                    }
                    break;
                case ("По типу"):
                    {
                        var k = from i in _StateList
                                orderby i.Type
                                select i;
                        foreach (GoodsInf sortedel in k)
                        {
                            sortedList.Add(sortedel);
                        }
                    }
                    break;
            }
            _StateList = sortedList;

        }

        private void SortbyCount()
        {
            if (_StateList.Count == 0)
                RefreshList(false);
            ObservableCollection<GoodsInf> sortedList = new ObservableCollection<GoodsInf>();
            try
            {
                Convert.ToDouble(CounttoFind);
            }
            catch
            {
                CounttoFind = "Неверное значение - (00.00)";
                return;
            }
            switch (SelectedCountSort.Text)
            {
                case ("="):
                    {
                        var k = from i in _StateList
                                where i.Count == Convert.ToDouble(CounttoFind)
                                select i;
                        foreach (GoodsInf sortedel in k)
                        {
                            sortedList.Add(sortedel);
                        }
                    }
                    break;
                case (">="):
                    {
                        var k = from i in _StateList
                                where i.Count >= Convert.ToDouble(CounttoFind)
                                select i;
                        foreach (GoodsInf sortedel in k)
                        {
                            sortedList.Add(sortedel);
                        }
                    }
                    break;
                case ("<="):
                    {
                        var k = from i in _StateList
                                where i.Count <= Convert.ToDouble(CounttoFind)
                                select i;
                        foreach (GoodsInf sortedel in k)
                        {
                            sortedList.Add(sortedel);
                        }
                    }
                    break;
            }
            _StateList = sortedList;
        }

        private void RefreshList(bool k)
        {
            if (k)
            {
                NametoFind = "";
                CounttoFind = "";
            }
            StateList.Clear();
            using (var context = new MyDbContext())
            {
                foreach (Goods g in context.Goods.ToList())
                {
                    StateList.Add(new GoodsInf());
                    StateList[StateList.Count() - 1].Name = g.Name;
                    StateList[StateList.Count() - 1].Id = g.Id;
                    StateList[StateList.Count() - 1].Price = g.Price;
                    StateList[StateList.Count() - 1].Type = g.Type;
                    StateList[StateList.Count() - 1].Count = g.Count;
                }
            }

            OnPropertyChanged("StateList");
        }

        public void BacktoMenu()
        {
            if (User.getInstance().Role == "Manager")
            {
                App.Current.MainWindow.Content = new View.Manager.ManagerMenu();
            }
            else 
            App.Current.MainWindow.Content = new View.Cashier.CashierMenu();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
