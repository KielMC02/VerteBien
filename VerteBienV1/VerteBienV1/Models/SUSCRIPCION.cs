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
    
    public partial class SUSCRIPCION
    {
        public int id_suscripcion { get; set; }
        public string id_usuario { get; set; }
        public string estado { get; set; }
        public string comentario { get; set; }
        public Nullable<System.DateTime> fecha_suscripcion { get; set; }
        public string trasaction_reference { get; set; }
        public string respuesta { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
