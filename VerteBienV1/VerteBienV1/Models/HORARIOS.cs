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
    
    public partial class HORARIOS
    {
        public int id_horario { get; set; }
        public string id_usuario { get; set; }
        public Nullable<decimal> semanales_inicio { get; set; }
        public Nullable<decimal> semanales_cierre { get; set; }
        public Nullable<decimal> sabados_inicio { get; set; }
        public Nullable<decimal> sabados_cierre { get; set; }
        public Nullable<decimal> domingo_inicio { get; set; }
        public Nullable<decimal> domingo_cierre { get; set; }
        public string estado { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
