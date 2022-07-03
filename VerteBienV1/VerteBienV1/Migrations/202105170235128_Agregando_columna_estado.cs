namespace VerteBienV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Agregando_columna_estado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "estado", c => c.String(maxLength: 60));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "estado");
        }
    }
}
