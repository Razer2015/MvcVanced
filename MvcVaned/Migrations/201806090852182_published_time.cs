namespace MvcVanced.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class published_time : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APKs", "Published", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.APKs", "Published");
        }
    }
}
