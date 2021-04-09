using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGoods.Essences
{
    public class SellInfo
    {
        [Key]
        public int Id { get; set; }

        public string Name_of_Sellgoods { get; set; }

        public double Count_of_Sellgoods { get; set; }

        public DateTime Date_of_Sell { get; set; }

        [ForeignKey("PersonalInfo")]
        public int Seller_Id { get; set; }
        
        [ForeignKey("Goods")]
        public int Goods_Id { get; set; }

        public virtual Goods Goods { get; set; }
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}
