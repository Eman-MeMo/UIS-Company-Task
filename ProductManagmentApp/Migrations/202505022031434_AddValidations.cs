namespace ProductManagmentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Products", "Unit", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Unit", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
