using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGoods.Model
{
    [Serializable]
    public class User
    {
        private static User instance;

        public static User getInstance(int id, string name, string role)
        {
            if (Instance == null)
            {
                Instance = new User(id, name, role);
            }
            return Instance;
        }
        public static User getInstance()
        {
            return Instance;
        }

        private User(int id, string name, string role)
        {
            Name = name;
            Role = role;
            Id = id;
        }

        private int id;
        private string name;
        private string role;

        public string Name { get => name; set => name = value; }
        public string Role { get => role; set => role = value; }
        public int Id { get => id; set => id = value; }
        public static User Instance { get => instance; set => instance = value; }

        public string Info()
        {
            return (name + " " + role + " " + Convert.ToString(Id));
        }
    }
}
