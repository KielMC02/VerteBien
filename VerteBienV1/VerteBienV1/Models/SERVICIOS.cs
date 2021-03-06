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
    
    public partial class SERVICIOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SERVICIOS()
        {
            this.CITAS = new HashSet<CITAS>();
            this.PUNTUACION_SERVICIOS = new HashSet<PUNTUACION_SERVICIOS>();
        }
    
        public int id_servicio { get; set; }
        public string id_usuario { get; set; }
        public Nullable<int> id_categoria { get; set; }
        public string nombre_servicio { get; set; }
        public string descripcion { get; set; }
        public Nullable<decimal> precio_servicio { get; set; }
        public Nullable<decimal> tiempo { get; set; }
        public string imagenes { get; set; }
        public string estado { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CATEGORIAS_SERVICIOS CATEGORIAS_SERVICIOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CITAS> CITAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PUNTUACION_SERVICIOS> PUNTUACION_SERVICIOS { get; set; }
    }
}
