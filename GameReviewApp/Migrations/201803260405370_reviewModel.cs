namespace GameReviewApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reviews", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Content", c => c.String());
        }
    }
}
