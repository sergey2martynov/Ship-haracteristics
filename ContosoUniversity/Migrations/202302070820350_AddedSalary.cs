namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSalary : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BoxNumber", newName: "Box");
            RenameColumn(table: "dbo.Department", name: "Administrator_ID", newName: "MechanicID");
            RenameIndex(table: "dbo.Department", name: "IX_Administrator_ID", newName: "IX_MechanicID");
            AddColumn("dbo.Contract", "Salary", c => c.Int(nullable: false));
            AlterColumn("dbo.Box", "Number", c => c.Int(nullable: false));
            DropColumn("dbo.Contract", "Grade");
            DropColumn("dbo.Department", "InstructorID");
            AlterStoredProcedure(
                "dbo.Department_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        MechanicID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Department]([Name], [Budget], [StartDate], [MechanicID])
                      VALUES (@Name, @Budget, @StartDate, @MechanicID)
                      
                      DECLARE @DepartmentID int
                      SELECT @DepartmentID = [DepartmentID]
                      FROM [dbo].[Department]
                      WHERE @@ROWCOUNT > 0 AND [DepartmentID] = scope_identity()
                      
                      SELECT t0.[DepartmentID], t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
            AlterStoredProcedure(
                "dbo.Department_Update",
                p => new
                    {
                        DepartmentID = p.Int(),
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        MechanicID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Department]
                      SET [Name] = @Name, [Budget] = @Budget, [StartDate] = @StartDate, [MechanicID] = @MechanicID
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
            AlterStoredProcedure(
                "dbo.Department_Delete",
                p => new
                    {
                        DepartmentID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Department]
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Department", "InstructorID", c => c.Int());
            AddColumn("dbo.Contract", "Grade", c => c.Int());
            AlterColumn("dbo.Box", "Number", c => c.String(maxLength: 50));
            DropColumn("dbo.Contract", "Salary");
            RenameIndex(table: "dbo.Department", name: "IX_MechanicID", newName: "IX_Administrator_ID");
            RenameColumn(table: "dbo.Department", name: "MechanicID", newName: "Administrator_ID");
            RenameTable(name: "dbo.Box", newName: "BoxNumber");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
