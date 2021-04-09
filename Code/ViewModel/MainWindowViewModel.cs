using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UGoods.ViewModel
{
    public class MainWindowViewModel
    {
        public static string getSHA256(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            SHA256Managed hashString = new SHA256Managed();
            byte[] hash = hashString.ComputeHash(bytes);
            string outHash = String.Empty;
            foreach (byte x in hash)
            {
                outHash += String.Format("{0:x2}", x);
            }

            return outHash;
        }
    }
}
