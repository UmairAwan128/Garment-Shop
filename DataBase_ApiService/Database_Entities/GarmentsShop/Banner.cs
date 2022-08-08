using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseEntityClasses.GarmentsShop
{
    public class Banner
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string Heading { get; set; }
        
        [MaxLength(200)]
        [Column(TypeName = "varchar")]
        public string SubHeading { get; set; }
        
        [Required]
        [MaxLength(300)]
        [Column(TypeName = "varchar")]
        public string ImageUrl { get; set; }

    }
}
