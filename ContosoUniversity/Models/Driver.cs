using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Driver : Person
    {
        public virtual ICollection<Contract> Contracts { get; set; }
    }
}