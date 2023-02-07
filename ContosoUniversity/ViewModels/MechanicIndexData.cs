using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.ViewModels
{
    public class MechanicIndexData
    {
        public IEnumerable<Mechanic> Mechanics { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
}