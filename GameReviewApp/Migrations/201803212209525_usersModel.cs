namespace GameReviewApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usersModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FavoriteGenre", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "FavoriteGame", c => c.String());
            AddColumn("dbo.AspNetUsers", "ReviewCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ReviewCount");
            DropColumn("dbo.AspNetUsers", "FavoriteGame");
            DropColumn("dbo.AspNetUsers", "FavoriteGenre");
        }
    }
}
