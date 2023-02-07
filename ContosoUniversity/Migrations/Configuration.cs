namespace ContosoUniversity.Migrations
{
    using ContosoUniversity.Models;
    using ContosoUniversity.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TaxiDepotContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaxiDepotContext context)
        {
            var students = new List<Driver>
            {
                new Driver { FirstMidName = "Carson",   LastName = "Alexander",
                    HireDate = DateTime.Parse("2010-09-01") },
                new Driver { FirstMidName = "Meredith", LastName = "Alonso",
                    HireDate = DateTime.Parse("2012-09-01") },
                new Driver { FirstMidName = "Arturo",   LastName = "Anand",
                    HireDate = DateTime.Parse("2013-09-01") },
                new Driver { FirstMidName = "Gytis",    LastName = "Barzdukas",
                    HireDate = DateTime.Parse("2012-09-01") },
                new Driver { FirstMidName = "Yan",      LastName = "Li",
                    HireDate = DateTime.Parse("2012-09-01") },
                new Driver { FirstMidName = "Peggy",    LastName = "Justice",
                    HireDate = DateTime.Parse("2011-09-01") },
                new Driver { FirstMidName = "Laura",    LastName = "Norman",
                    HireDate = DateTime.Parse("2013-09-01") },
                new Driver { FirstMidName = "Nino",     LastName = "Olivetto",
                    HireDate = DateTime.Parse("2005-09-01") }
            };

            students.ForEach(s => context.Drivers.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();

            var instructors = new List<Mechanic>
            {
                new Mechanic { FirstMidName = "Kim",     LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11") },
                new Mechanic { FirstMidName = "Fadi",    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06") },
                new Mechanic { FirstMidName = "Roger",   LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01") },
                new Mechanic { FirstMidName = "Candace", LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15") },
                new Mechanic { FirstMidName = "Roger",   LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12") }
            };
            instructors.ForEach(s => context.Mechanics.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();

            var departments = new List<Department>
            {
                new Department { Name = "North",     Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    MechanicID  = instructors.Single( i => i.LastName == "Abercrombie").ID },
                new Department { Name = "East", Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    MechanicID  = instructors.Single( i => i.LastName == "Fakhouri").ID },
                new Department { Name = "West", Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    MechanicID  = instructors.Single( i => i.LastName == "Harui").ID },
                new Department { Name = "South",   Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    MechanicID  = instructors.Single( i => i.LastName == "Kapoor").ID }
            };
            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var courses = new List<Car>
            {
                new Car {CarID = 1050, Title = "Ford Focus",      Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "North").DepartmentID,
                  Mechanics = new List<Mechanic>()
                },
                new Car {CarID = 4022, Title = "Toyota Corolla", Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "West").DepartmentID,
                  Mechanics = new List<Mechanic>()
                },
                new Car {CarID = 4041, Title = "Subaru Outback", Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "East").DepartmentID,
                  Mechanics = new List<Mechanic>()
                },
                new Car {CarID = 1045, Title = "Volkswagen Passat",       Credits = 4,
                  DepartmentID = departments.Single( s => s.Name == "South").DepartmentID,
                  Mechanics = new List<Mechanic>()
                },
                new Car {CarID = 3141, Title = "Mercedes-Benz GLE",   Credits = 4,
                  DepartmentID = departments.Single( s => s.Name == "North").DepartmentID,
                  Mechanics = new List<Mechanic>()
                },
                new Car {CarID = 2021, Title = "Mercedes Maybach-S",    Credits = 3,
                  DepartmentID = departments.Single( s => s.Name == "South").DepartmentID,
                  Mechanics = new List<Mechanic>()
                },
                new Car {CarID = 2042, Title = "Suzuki Jimmy",     Credits = 4,
                  DepartmentID = departments.Single( s => s.Name == "North").DepartmentID,
                  Mechanics = new List<Mechanic>()
                },
            };

            courses.ForEach(s => context.Cars.AddOrUpdate(p => p.CarID, s));
            context.SaveChanges();

            var officeAssignments = new List<Box>
            {
                new Box {
                    MechanicID = instructors.Single( i => i.LastName == "Fakhouri").ID,
                    Number = 1 },
                new Box {
                    MechanicID = instructors.Single( i => i.LastName == "Harui").ID,
                    Number = 2 },
                new Box {
                    MechanicID = instructors.Single( i => i.LastName == "Kapoor").ID,
                    Number = 3 },
            };

            officeAssignments.ForEach(s => context.BoxNumbers.AddOrUpdate(p => p.MechanicID, s));
            context.SaveChanges();

            AddOrUpdateMechanic(context, "Ford Focus", "Kapoor");
            AddOrUpdateMechanic(context, "Ford Focus", "Harui");
            AddOrUpdateMechanic(context, "Toyota Corolla", "Zheng");
            AddOrUpdateMechanic(context, "Toyota Corolla", "Zheng");

            AddOrUpdateMechanic(context, "Subaru Outback", "Fakhouri");
            AddOrUpdateMechanic(context, "Volkswagen Passat", "Harui");
            AddOrUpdateMechanic(context, "Mercedes-Benz GLE", "Abercrombie");
            AddOrUpdateMechanic(context, "Suzuki Jimmy", "Abercrombie");

            context.SaveChanges();

            var enrollments = new List<Contract>
            {
                new Contract {
                    DriverID = students.Single(s => s.LastName == "Alexander").ID,
                    CarID = courses.Single(c => c.Title == "Ford Focus" ).CarID,
                    Salary = 3000
                },
                 new Contract {
                    DriverID = students.Single(s => s.LastName == "Alexander").ID,
                    CarID = courses.Single(c => c.Title == "Toyota Corolla" ).CarID,
                    Salary = 1000
                 },
                 new Contract {
                    DriverID = students.Single(s => s.LastName == "Alexander").ID,
                    CarID = courses.Single(c => c.Title == "Subaru Outback" ).CarID,
                    Salary = 500
                 },
                 new Contract {
                     DriverID = students.Single(s => s.LastName == "Alonso").ID,
                    CarID = courses.Single(c => c.Title == "Volkswagen Passat" ).CarID,
                    Salary = 5000
                 },
                 new Contract {
                     DriverID = students.Single(s => s.LastName == "Alonso").ID,
                    CarID = courses.Single(c => c.Title == "Mercedes-Benz GLE" ).CarID,
                    Salary = 5664
                 },
                 new Contract {
                    DriverID = students.Single(s => s.LastName == "Alonso").ID,
                    CarID = courses.Single(c => c.Title == "Mercedes Maybach-S" ).CarID,
                    Salary = 5420
                 },
                 new Contract {
                    DriverID = students.Single(s => s.LastName == "Anand").ID,
                    CarID = courses.Single(c => c.Title == "Suzuki Jimmy" ).CarID,
                    Salary = 1523
                 },
                 new Contract {
                    DriverID = students.Single(s => s.LastName == "Anand").ID,
                    CarID = courses.Single(c => c.Title == "Volkswagen Passat").CarID,
                    Salary = 3020
                 },
                new Contract {
                    DriverID = students.Single(s => s.LastName == "Barzdukas").ID,
                    CarID = courses.Single(c => c.Title == "Suzuki Jimmy").CarID,
                    Salary = 6523
                 },
                 new Contract {
                    DriverID = students.Single(s => s.LastName == "Li").ID,
                    CarID = courses.Single(c => c.Title == "Mercedes Maybach-S").CarID,
                    Salary = 7945
                 },
                 new Contract {
                    DriverID = students.Single(s => s.LastName == "Justice").ID,
                    CarID = courses.Single(c => c.Title == "Toyota Corolla").CarID,
                    Salary = 6958
                 }
            };

            foreach (Contract e in enrollments)
            {
                var enrollmentInDataBase = context.Contracts.Where(
                    s =>
                         s.Driver.ID == e.DriverID &&
                         s.Car.CarID == e.CarID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Contracts.Add(e);
                }
            }
            context.SaveChanges();
        }

        void AddOrUpdateMechanic(TaxiDepotContext context, string carTitle, string mechanicName)
        {
            var crs = context.Cars.SingleOrDefault(c => c.Title == carTitle);
            var inst = crs.Mechanics.SingleOrDefault(i => i.LastName == mechanicName);
            if (inst == null)
                crs.Mechanics.Add(context.Mechanics.Single(i => i.LastName == mechanicName));
        }
    }
}
