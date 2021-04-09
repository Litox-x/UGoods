using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGoods.Essences
{
    public class Goods
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Count { get; set; }
        public string Type { get; set; }

        public virtual ICollection<SellInfo> SellInfos { get; set; }
    }
}
