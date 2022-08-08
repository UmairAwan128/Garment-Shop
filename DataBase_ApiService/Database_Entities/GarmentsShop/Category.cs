using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseEntityClasses.GarmentsShop
{
    //[Table("Table1")]
    public class Category : IListable
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Column("Col1",TypeName="varchar")]
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public virtual Department Department { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(300)]
        public string ImageUrl { get; set; }
        
    }
}
