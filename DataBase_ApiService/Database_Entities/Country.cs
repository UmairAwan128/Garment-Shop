using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseEntityClasses
{
    public class Country : IListable
    {
        public int Id { get; set; }

        
        public int? Code { get; set; }

        [Required]
        [Column(TypeName="varchar")]
        [MaxLength(50)]
        public string Name { get; set; }

       

    }
}
