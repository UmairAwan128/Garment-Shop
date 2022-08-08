using DataBaseEntityClasses.GarmentsShop;
using DataBaseEntityClasses.UsersMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseEntityClasses
{
    public class Order
    {
        public Order()
        {
            Products = new List<Product>();
            User = new User();
        }

        public int Id { get; set; }

        [Required]

        public virtual User User { get; set; }

        [Required]
        public virtual ICollection<Product> Products { get; set; }
        
    }
}
