namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBoxNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoxNumber", "Number", c => c.String(maxLength: 50));
            DropColumn("dbo.BoxNumber", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BoxNumber", "Location", c => c.String(maxLength: 50));
            DropColumn("dbo.BoxNumber", "Number");
        }
    }
}
