using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Box
    {
        [Key]
        [ForeignKey("Mechanic")]
        public int MechanicID { get; set; }
        [Display(Name = "Box Number")]
        public int Number { get; set; }

        public virtual Mechanic Mechanic { get; set; }
    }
}