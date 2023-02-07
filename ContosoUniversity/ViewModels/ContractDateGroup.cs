using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.ViewModels
{
    public class ContractDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public int DriverCount { get; set; }
    }
}