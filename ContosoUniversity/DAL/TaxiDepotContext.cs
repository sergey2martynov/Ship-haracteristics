using ContosoUniversity.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace ContosoUniversity.DAL
{
    public class TaxiDepotContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Box> BoxNumbers { get; set; }
        public DbSet<Person> People { get; set; }

        /*public SchoolContext()
        {
            SqlConnection.ClearAllPools();
            Database.Delete();
            Database.Create();
        }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Car>()
                .HasMany(c => c.Mechanics).WithMany(i => i.Cars)
                .Map(t => t.MapLeftKey("CarID")
                    .MapRightKey("MechanicID")
                    .ToTable("CarMechanic"));
            modelBuilder.Entity<Department>().MapToStoredProcedures();
        }
    }
}