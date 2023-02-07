namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoxNumber",
                c => new
                    {
                        MechanicID = c.Int(nullable: false),
                        Location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MechanicID)
                .ForeignKey("dbo.Person", t => t.MechanicID)
                .Index(t => t.MechanicID);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        HireDate = c.DateTime(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Car",
                c => new
                    {
                        CarID = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        Credits = c.Int(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarID)
                .ForeignKey("dbo.Department", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        ContractID = c.Int(nullable: false, identity: true),
                        CarID = c.Int(nullable: false),
                        DriverID = c.Int(nullable: false),
                        Grade = c.Int(),
                    })
                .PrimaryKey(t => t.ContractID)
                .ForeignKey("dbo.Car", t => t.CarID, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.DriverID, cascadeDelete: true)
                .Index(t => t.CarID)
                .Index(t => t.DriverID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Budget = c.Decimal(nullable: false, storeType: "money"),
                        StartDate = c.DateTime(nullable: false),
                        InstructorID = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Administrator_ID = c.Int(),
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.Person", t => t.Administrator_ID)
                .Index(t => t.Administrator_ID);
            
            CreateTable(
                "dbo.CarMechanic",
                c => new
                    {
                        CarID = c.Int(nullable: false),
                        MechanicID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CarID, t.MechanicID })
                .ForeignKey("dbo.Car", t => t.CarID, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.MechanicID, cascadeDelete: true)
                .Index(t => t.CarID)
                .Index(t => t.MechanicID);
            
            CreateStoredProcedure(
                "dbo.Department_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        InstructorID = p.Int(),
                        Administrator_ID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Department]([Name], [Budget], [StartDate], [InstructorID], [Administrator_ID])
                      VALUES (@Name, @Budget, @StartDate, @InstructorID, @Administrator_ID)
                      
                      DECLARE @DepartmentID int
                      SELECT @DepartmentID = [DepartmentID]
                      FROM [dbo].[Department]
                      WHERE @@ROWCOUNT > 0 AND [DepartmentID] = scope_identity()
                      
                      SELECT t0.[DepartmentID], t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
            CreateStoredProcedure(
                "dbo.Department_Update",
                p => new
                    {
                        DepartmentID = p.Int(),
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        InstructorID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                        Administrator_ID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Department]
                      SET [Name] = @Name, [Budget] = @Budget, [StartDate] = @StartDate, [InstructorID] = @InstructorID, [Administrator_ID] = @Administrator_ID
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
            CreateStoredProcedure(
                "dbo.Department_Delete",
                p => new
                    {
                        DepartmentID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                        Administrator_ID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Department]
                      WHERE ((([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL))) AND (([Administrator_ID] = @Administrator_ID) OR ([Administrator_ID] IS NULL AND @Administrator_ID IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Department_Delete");
            DropStoredProcedure("dbo.Department_Update");
            DropStoredProcedure("dbo.Department_Insert");
            DropForeignKey("dbo.BoxNumber", "MechanicID", "dbo.Person");
            DropForeignKey("dbo.CarMechanic", "MechanicID", "dbo.Person");
            DropForeignKey("dbo.CarMechanic", "CarID", "dbo.Car");
            DropForeignKey("dbo.Car", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Department", "Administrator_ID", "dbo.Person");
            DropForeignKey("dbo.Contract", "DriverID", "dbo.Person");
            DropForeignKey("dbo.Contract", "CarID", "dbo.Car");
            DropIndex("dbo.CarMechanic", new[] { "MechanicID" });
            DropIndex("dbo.CarMechanic", new[] { "CarID" });
            DropIndex("dbo.Department", new[] { "Administrator_ID" });
            DropIndex("dbo.Contract", new[] { "DriverID" });
            DropIndex("dbo.Contract", new[] { "CarID" });
            DropIndex("dbo.Car", new[] { "DepartmentID" });
            DropIndex("dbo.BoxNumber", new[] { "MechanicID" });
            DropTable("dbo.CarMechanic");
            DropTable("dbo.Department");
            DropTable("dbo.Contract");
            DropTable("dbo.Car");
            DropTable("dbo.Person");
            DropTable("dbo.BoxNumber");
        }
    }
}
