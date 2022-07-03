namespace VerteBienV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Agregando_columna_fechas_new2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "fecha_nacimiento_", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "fecha_creacion_", c => c.DateTime(nullable: false));

        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "fecha_creacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "fecha_nacimiento", c => c.DateTime(nullable: false));

        }
    }
}
