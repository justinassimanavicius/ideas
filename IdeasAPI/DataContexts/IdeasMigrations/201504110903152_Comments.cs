namespace IdeasAPI.DataContexts.IdeasMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Author = c.String(),
                        Created = c.DateTime(nullable: false),
                        Edited = c.DateTime(),
                        Entry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entries", t => t.Entry_Id)
                .Index(t => t.Entry_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Entry_Id", "dbo.Entries");
            DropIndex("dbo.Comments", new[] { "Entry_Id" });
            DropTable("dbo.Comments");
        }
    }
}
