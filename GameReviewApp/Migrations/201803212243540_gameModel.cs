namespace GameReviewApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gameModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Content = c.String(),
                        GameId = c.Int(nullable: false),
                        NumRating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "GameId", "dbo.Games");
            DropIndex("dbo.Reviews", new[] { "GameId" });
            DropTable("dbo.Reviews");
        }
    }
}
