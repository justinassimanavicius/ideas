namespace IdeasAPI.DataContexts.IdeasMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Votes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsPositive = c.Boolean(nullable: false),
                        Author = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Entry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entries", t => t.Entry_Id)
                .Index(t => t.Entry_Id);
            
            AddColumn("dbo.Comments", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comments", "UpdateDate", c => c.DateTime());
            DropColumn("dbo.Comments", "Created");
            DropColumn("dbo.Comments", "Edited");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Edited", c => c.DateTime());
            AddColumn("dbo.Comments", "Created", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Votes", "Entry_Id", "dbo.Entries");
            DropIndex("dbo.Votes", new[] { "Entry_Id" });
            DropColumn("dbo.Comments", "UpdateDate");
            DropColumn("dbo.Comments", "CreateDate");
            DropTable("dbo.Votes");
        }
    }
}
