namespace KsiegarniaUKW2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodaniePolaKsiazki : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ksiazka", "OpisS", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ksiazka", "OpisS");
        }
    }
}
