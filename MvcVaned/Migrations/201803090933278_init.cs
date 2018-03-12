namespace MvcVanced.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.APKs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Version = c.String(),
                        Architecture = c.String(),
                        MinimumAPI = c.String(),
                        DPI = c.String(),
                        Size = c.Long(nullable: false),
                        Type = c.Int(nullable: false),
                        Downloads = c.Int(nullable: false),
                        FileID = c.String(),
                        Public = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.APKs");
        }
    }
}
