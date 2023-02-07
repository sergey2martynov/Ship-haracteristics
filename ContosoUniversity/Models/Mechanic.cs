using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Mechanic : Person
    {
        public virtual ICollection<Car> Cars { get; set; }
        public virtual Box Box { get; set; }
    }
}