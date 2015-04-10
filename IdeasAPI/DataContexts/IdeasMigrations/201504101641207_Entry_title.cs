namespace IdeasAPI.DataContexts.IdeasMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Entry_title : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Entries", "Title");
        }
    }
}
