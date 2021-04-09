using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGoods.Essences
{
    public class PersonalInfo
    {
        [Key]
        [ForeignKey("RegIn")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }
        

        public virtual RegIn RegIn { get; set; }
        public virtual ICollection<SellInfo> SellInfos { get; set; }
    }
}
