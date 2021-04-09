using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UGoods.Essences;
using UGoods.Model;

namespace UGoods.ViewModel.Manager
{
    public class StorageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GoodsInf> StateList = new ObservableCollection<GoodsInf>();

        public ObservableCollection<GoodsInf> _StateList
        {
            get { return StateList; }
            set { StateList = value; OnPropertyChanged("_Statelist"); }
        }

        private GoodsInf selecteditem;
        private TextBlock selectedsort;
        private string nametofind;
        private string counttofind;
        private TextBlock selectedcountsort;
        private string prCh;

        public string PrCh { get => prCh; set => prCh = value; }
        public GoodsInf SelectedGood { set => selecteditem = value; get => selecteditem; }
        public TextBlock SelectedSort { get => selectedsort; set { selectedsort = value; Sortby(); OnPropertyChanged("SelectedSort"); } }
        public string NametoFind { get => nametofind; set { nametofind = value; OnPropertyChanged("NametoFind"); } }
        public string CounttoFind { get => counttofind; set { counttofind = value; OnPropertyChanged("CounttoFind"); } }
        public TextBlock SelectedCountSort { get => selectedcountsort; set { selectedcountsort = value; OnPropertyChanged("SelectedCountSort"); } }

        /*Свойства для ICommand*/
        public ICommand ToRefreshList
        {
            get
            {
                return new DelegateCommand(() => RefreshList(true));
            }
        }

        public ICommand ToAddNewUser
        {
            get
            {
                return new DelegateCommand(() => AddNewUser());
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

        public ICommand ToChangePrice
        {
            get
            {
                return new DelegateCommand(() => ChangePrice());
            }
        }

        public ICommand GoBacktoMenu
        {
            get
            {
                return new DelegateCommand(() => BacktoMenu());
            }
        }

        public ICommand ToChangeRole
        {
            get
            {
                return new DelegateCommand(() => ChangeRole());
            }
        }




        /**/

        /*Методы*/
        private void ChangeRole()
        {
            App.Current.MainWindow.Content = new View.Cashier.Storage.Storage();
        }

        private void ChangePrice()
        {
            if (SelectedGood == null)
            {
                View.myMessageBox.Show("Выберите товар!");
                return;
            }
            if (PrCh == String.Empty )
            {
                View.myMessageBox.Show("Поле сверху не может быть пустым. Введите цену.");
                return;
            }
            try
            {
               if(Convert.ToDouble(PrCh) == 0)
                {
                    View.myMessageBox.Show("Цена не может быть равна нулю.");
                    return;
                }
            }
            catch
            {
                View.myMessageBox.Show("неверный формат ввода цены (00,00)");
                return;
            }
           using( var context = new MyDbContext())
            {
                Goods PriceChange =  context.Goods.Find(SelectedGood.Id);
                PriceChange.Price = Convert.ToDouble(PrCh);
                context.SaveChanges();
            }
            RefreshList(true);
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

        private void AddNewUser()
        {
            new View.Manager.Storage.NewGoodsAdd().Show();
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
            App.Current.MainWindow.Content = new View.Manager.ManagerMenu();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
