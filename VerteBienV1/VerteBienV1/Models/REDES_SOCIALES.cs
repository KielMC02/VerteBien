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
    
    public partial class REDES_SOCIALES
    {
        public int id_redes { get; set; }
        public string id_usuario { get; set; }
        public string whatsapp { get; set; }
        public string instagram { get; set; }
        public string facebook { get; set; }
        public string web_app { get; set; }
        public string estado { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
