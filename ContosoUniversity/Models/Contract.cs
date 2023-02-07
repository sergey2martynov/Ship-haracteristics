using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{

    public class Contract
    {
        public int ContractID { get; set; }
        public int CarID { get; set; }
        public int DriverID { get; set; }
        public int Salary { get; set; }

        public virtual Car Car { get; set; }
        public virtual Driver Driver { get; set; }
    }
}