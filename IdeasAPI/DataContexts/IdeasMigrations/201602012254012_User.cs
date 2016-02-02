namespace IdeasAPI.DataContexts.IdeasMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortName = c.String(),
                        Email = c.String(),
                        Thumbnail = c.Binary(),
                        BirthDay = c.DateTime(),
                        DomainName = c.String(),
                        IsModerator = c.Boolean(nullable: false),
                        LastLoginDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Comments", "UserId", c => c.Int());
            AddColumn("dbo.Entries", "UserId", c => c.Int());
            AddColumn("dbo.Votes", "UserId", c => c.Int());
            CreateIndex("dbo.Comments", "UserId");
            CreateIndex("dbo.Entries", "UserId");
            CreateIndex("dbo.Votes", "UserId");
            AddForeignKey("dbo.Entries", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Votes", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Comments", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Votes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Entries", "UserId", "dbo.Users");
            DropIndex("dbo.Votes", new[] { "UserId" });
            DropIndex("dbo.Entries", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropColumn("dbo.Votes", "UserId");
            DropColumn("dbo.Entries", "UserId");
            DropColumn("dbo.Comments", "UserId");
            DropTable("dbo.Users");
        }
    }
}
