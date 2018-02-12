namespace eCMS.BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Case", "Comments", c => c.String());
            AddColumn("dbo.CaseGoal", "Comments", c => c.String());
            DropColumn("dbo.CaseGoal", "QualityOfLifeCategoryNames");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CaseGoal", "QualityOfLifeCategoryNames", c => c.String());
            DropColumn("dbo.CaseGoal", "Comments");
            DropColumn("dbo.Case", "Comments");
        }
    }
}
