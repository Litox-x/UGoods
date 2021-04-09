using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGoods.Model
{
    public class PInf
    {

        private int Id;

        private string name;

        private string role;

        public PInf()
        {

        }
        public PInf( int id,string name, string role)
        {
            ID = id;
            Name = name;
            Role = role;
        }
        
        public int ID { get => Id; set => Id = value; }
        public string Name { get => name; set => name = value; }
        public string Role { get => role; set => role = value; }
    }
}
