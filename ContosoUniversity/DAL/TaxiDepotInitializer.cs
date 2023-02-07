using System;
using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public class TaxiDepotInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TaxiDepotContext>
    {
        protected override void Seed(TaxiDepotContext context)
        {
            var students = new List<Driver>
            {
            new Driver{FirstMidName="Carson",LastName="Alexander",HireDate=DateTime.Parse("2005-09-01")},
            new Driver{FirstMidName="Meredith",LastName="Alonso",HireDate=DateTime.Parse("2002-09-01")},
            new Driver{FirstMidName="Arturo",LastName="Anand",HireDate=DateTime.Parse("2003-09-01")},
            new Driver{FirstMidName="Gytis",LastName="Barzdukas",HireDate=DateTime.Parse("2002-09-01")},
            new Driver{FirstMidName="Yan",LastName="Li",HireDate=DateTime.Parse("2002-09-01")},
            new Driver{FirstMidName="Peggy",LastName="Justice",HireDate=DateTime.Parse("2001-09-01")},
            new Driver{FirstMidName="Laura",LastName="Norman",HireDate=DateTime.Parse("2003-09-01")},
            new Driver{FirstMidName="Nino",LastName="Olivetto",HireDate=DateTime.Parse("2005-09-01")}
            };

            students.ForEach(s => context.Drivers.Add(s));
            context.SaveChanges();
            var courses = new List<Car>
            {
            new Car{CarID=1050,Title="Ford Focus",Credits=3,},
            new Car{CarID=4022,Title="Toyota Corolla",Credits=3,},
            new Car{CarID=4041,Title="Subaru Outback",Credits=3,},
            new Car{CarID=1045,Title="Volkswagen Passat",Credits=4,},
            new Car{CarID=3141,Title="Mercedes-Benz GLE",Credits=4,},
            new Car{CarID=2021,Title="Mercedes Maybach-S",Credits=3,},
            new Car{CarID=2042,Title="Suzuki Jimmy",Credits=4,}
            };
            courses.ForEach(s => context.Cars.Add(s));
            context.SaveChanges();
            var enrollments = new List<Contract>
            {
            new Contract{DriverID=1,CarID=1050,Salary=1000},
            new Contract{DriverID=1,CarID=4022,Salary=1000},
            new Contract{DriverID=1,CarID=4041,Salary=1000},
            new Contract{DriverID=2,CarID=1045,Salary=1000},
            new Contract{DriverID=2,CarID=3141,Salary=1000},
            new Contract{DriverID=2,CarID=2021,Salary=1000},
            new Contract{DriverID=3,CarID=1050,Salary=1000},
            new Contract{DriverID=4,CarID=1050,Salary=1000},
            new Contract{DriverID=4,CarID=4022,Salary=1000},
            new Contract{DriverID=5,CarID=4041,Salary=1000},
            new Contract{DriverID=6,CarID=1045,Salary=1000},
            new Contract{DriverID=7,CarID=3141,Salary=1000},
            };
            enrollments.ForEach(s => context.Contracts.Add(s));
            context.SaveChanges();
        }
    }
}