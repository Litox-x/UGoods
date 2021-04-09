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
using System.Windows.Controls;
using System.Windows.Input;
using UGoods.Essences;
using UGoods.View;

namespace UGoods.ViewModel.Manager
{
    public class NewGoodsAddViewModel :INotifyPropertyChanged,IDataErrorInfo
    {
        private string name = "";
        private string count ="";
        private string price ="";
        private TextBlock selectedtype;
        
        public string Name { get => name; set { name = value; OnPropertyChanged("Name");} }
        public string Count { get => count; set { count = value; OnPropertyChanged("Count"); } }
        public string Price { get => price; set { price = value; OnPropertyChanged("Price"); } }
        public TextBlock Selectedtype { get => selectedtype; set { selectedtype = value; OnPropertyChanged("Selectedtype"); OnPropertyChanged("Count"); } }





        private string CheckValidName(string column)
        {
            if (Regex.Match(column, @"[^а-яА-яa-zA-Z ]").Success == true)
                return "Поле должно содержать только кирилицу, латиницу";
            if (!column.Take(1).All(Char.IsUpper))
                return "Первая буква должна быть заглавной";
            if (string.IsNullOrEmpty(column))
                return "Поле не может быть пустым";
            if (column.Length < 2)
                return "Имя слишком короткое, либо поле пусто";
            return null;

        }

        public ICommand ToSubmitAdd
        {
            get
            {
                return new DelegateCommand(() => SubmitAdd());
            }
        }

        private void SubmitAdd()
        {
            using (var context = new MyDbContext())
            {
                Goods newg = new Goods();
                newg.Name = Name;
                newg.Price = Convert.ToDouble(Price);
                newg.Count = Convert.ToDouble(Count);
                if (Selectedtype.Text == "Штука")
                    newg.Type = "шт";
                else newg.Type = "кг";
                bool flag=false;
                foreach(Goods k in context.Goods)
                {
                    if(k.Name==newg.Name && k.Type==newg.Type)
                    {

                        flag = true;
                        var mes =  MessageBox.Show("Обнаружены совпадения по типу и названию продукта.\n При нажатии кнопкі Yes количество товара будет совмещено." +
                            "\n при нажатии No вы вернетесь к окну добавления товара.", "Совпадение", MessageBoxButton.YesNo);
                        if (mes == MessageBoxResult.Yes)
                        {
                            newg = context.Goods.Find(k.Id);
                            newg.Count += Convert.ToDouble(Count);
                            break;
                            
                        }
                    }
                }
                if (!flag)
                {
                    context.Goods.Add(newg);
                    context.SaveChanges();
                    myMessageBox.Show("Новый продукт добавлен");
                    foreach(Window window in App.Current.Windows)
                    {
                        if (window is View.Manager.Storage.NewGoodsAdd)
                            window.Close();
                    }
                }
                else
                {
                    context.SaveChanges();
                    myMessageBox.Show("Количество найденного продукта увеличено(цена сохранена старой)");
                    foreach (Window window in App.Current.Windows)
                    {
                        if (window is View.Manager.Storage.NewGoodsAdd)
                            window.Close();
                    }
                }

            }

        }

        public string this[string columnName]
        {
            get
            {
                string result = String.Empty;
                switch (columnName)
                {
                    case ("Name"):
                        if (CheckValidName(Name) != null)
                            result = CheckValidName(Name);
                        break;
                    case ("Price"):
                        if (Price == "")
                            result = "Строка не может быть пустой";
                        try {
                            Convert.ToDouble(Price);
                        }
                        catch (Exception m) {
                            result = "Не верный ввод формата цены (00,00)";
                            return result;
                        }
                        if (Convert.ToDouble(Price) <= 0)
                        {
                            result = "Цена не может быть отрицательной, либо нулем";
                            return result;
                            
                        }
                        break;
                    case ("Count"):
                        
                        if (Selectedtype == null)
                        result = "Выберите тип";
                        else
                        if (Selectedtype.Text == "Штука")
                        {
                            try
                            {
                                Convert.ToInt32(Count);
                            }
                            catch (Exception m)
                            {
                                result = "Количество - целое число";
                                return result;
                            }
                            if (Convert.ToInt32(Count) > 100000)
                                result = ("Слишком большое количество(>100000)");
                            if (Convert.ToInt32(Count) <= 0)
                                result = ("Количество не может быть отрицательным, либо нулем");
                            if (Count == String.Empty)
                                result = "Строка не может быть пустой";
                        }
                         else 
                            if (Selectedtype.Text == "Киллограмм")
                            {
                                try
                                {
                                    Convert.ToDouble(Count);
                                }
                                catch (Exception m)
                                {
                                    result = "Не верный ввод формата количества (00,00)";
                                    return result;
                                }
                                if (Convert.ToDouble(Count) > 100000)
                                    result = ("Слишком большое количество(>100000)");
                            if (Count == String.Empty)
                                result = "Строка не может быть пустой";
                        }
                        


                        break;

                    case ("Selectedtype"):
                        if (Selectedtype == null)
                            result = "Выберите тип";
                        break;
                }
                return result;
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
