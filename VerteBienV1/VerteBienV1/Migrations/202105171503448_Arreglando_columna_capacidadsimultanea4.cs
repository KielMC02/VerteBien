namespace VerteBienV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Arreglando_columna_capacidadsimultanea4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "capacidad_simultanea_", c => c.Int(nullable: false));

        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "capacidad_simultanea", c => c.Int(nullable: false));

        }
    }
}
