using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using System.Windows;
using UGoods.Essences;

namespace UGoods.Service
{
    public static class ReportService
    {
        public static void GenerateReport(List<SellInfo> selllist)
        {

            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add();
            wordApp.Visible = true;
            doc.Content.SetRange(0, 0);
            Word.Paragraph parag1 = doc.Content.Paragraphs.Add();
            object styleHeading1 = "Заголовок 1";
            parag1.Range.set_Style(styleHeading1);
            doc.Content.Text = "Отчёт по доходам " + Environment.NewLine;
            Word.Paragraph parag2 = doc.Content.Paragraphs.Add();
            parag2.Range.set_Style("Без интервала");

            double TotalProfit = 0;
            double UserProfit = 0;

            string output = "";
            using (var context = new MyDbContext())
            {
                foreach (PersonalInfo user in context.PersonalInfo)
                {
                    UserProfit = 0;

                    foreach(SellInfo workday in selllist) {

                        if (workday.Seller_Id == user.Id)
                            UserProfit = UserProfit + (workday.Count_of_Sellgoods * GetPrice(workday.Goods_Id));
                    }

                    output +="Роль: "+ user.Role +"; Имя " + user.Name + "; Прибыль: " + UserProfit + Environment.NewLine;
                    TotalProfit += UserProfit;
                }
            }
            output += "Общая прибыль:" + TotalProfit;
            doc.Content.Text += output;
        }
        public static void GenerateReportSells(List<SellInfo> selllist)
        {

            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add();
            wordApp.Visible = true;
            doc.Content.SetRange(0, 0);
            Word.Paragraph parag1 = doc.Content.Paragraphs.Add();
            object styleHeading1 = "Заголовок 1";
            parag1.Range.set_Style(styleHeading1);
            doc.Content.Text = "Отчёт по продажам " + Environment.NewLine;
            Word.Paragraph parag2 = doc.Content.Paragraphs.Add();
            parag2.Range.set_Style("Без интервала");

            string output = "";
            using (var context = new MyDbContext())
            {
                foreach (PersonalInfo user in context.PersonalInfo)
                {
                    
                    foreach (SellInfo workday in selllist)
                    {
                        if(workday.Seller_Id == user.Id)

                            output += "Роль: " + user.Role + "; Имя: " + user.Name + "; Дата продажи: "+workday.Date_of_Sell+"; Проданный товар: "+workday.Name_of_Sellgoods + "; Количество: " + workday.Count_of_Sellgoods + Environment.NewLine;

                    }

                 }
            }
            doc.Content.Text += output;
        }
        private static double GetPrice(int id)
        {
            using (var context = new MyDbContext())
            {
                return context.Goods.Find(id).Price;
            }
        }

    }
}
