using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UGoods.View
{
    partial class myMessageBox
    {
        public static void Show(string Message)
        {
            Window window = new View.myMessageBox(Message);
            window.Show();
        }
    }
}
