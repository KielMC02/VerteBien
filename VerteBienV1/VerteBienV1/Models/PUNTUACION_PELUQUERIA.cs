//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VerteBienV1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PUNTUACION_PELUQUERIA
    {
        public int id_puntuacion_peluqueria { get; set; }
        public string id_usuario { get; set; }
        public Nullable<int> id_cita { get; set; }
        public string comentario { get; set; }
        public Nullable<int> estrellas { get; set; }
        public Nullable<System.DateTime> fecha_creacion { get; set; }
        public string estado { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CITAS CITAS { get; set; }
    }
}
