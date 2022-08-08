using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseEntityClasses.GarmentsShop
{
    public class SubCategory : IListable
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(300)]
        public string ImageUrl { get; set; }
    }
}
