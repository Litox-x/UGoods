using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGoods.Essences
{
    public class RegIn
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
      
        
         public virtual PersonalInfo PersonalInfo { get; set; }
    }
}
