namespace VerteBienV1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Agregando_colimnas_usuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "nombre", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "apellido", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "fecha_nacimiento", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "fecha_creacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "ciudad", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "sector", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "calle", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "telefono", c => c.String(maxLength: 20));
            AddColumn("dbo.AspNetUsers", "capacidad_simultanea", c => c.Int(nullable: true));
            AddColumn("dbo.AspNetUsers", "latitud", c => c.String(maxLength: 40));
            AddColumn("dbo.AspNetUsers", "longitud", c => c.String(maxLength: 40));
            AddColumn("dbo.AspNetUsers", "nombre_peluqueria", c => c.String(maxLength: 60));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "nombre_peluqueria");
            DropColumn("dbo.AspNetUsers", "longitud");
            DropColumn("dbo.AspNetUsers", "latitud");
            DropColumn("dbo.AspNetUsers", "capacidad_simultanea");
            DropColumn("dbo.AspNetUsers", "telefono");
            DropColumn("dbo.AspNetUsers", "calle");
            DropColumn("dbo.AspNetUsers", "sector");
            DropColumn("dbo.AspNetUsers", "ciudad");
            DropColumn("dbo.AspNetUsers", "fecha_creacion");
            DropColumn("dbo.AspNetUsers", "fecha_nacimiento");
            DropColumn("dbo.AspNetUsers", "apellido");
            DropColumn("dbo.AspNetUsers", "nombre");
        }
    }
}
