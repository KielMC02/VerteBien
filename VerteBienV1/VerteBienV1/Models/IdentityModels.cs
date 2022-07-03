using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VerteBienV1.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string nombre { get; set; }
        [StringLength(50)]
        public string apellido { get; set; }
        public DateTime fecha_nacimiento_ { get; set; }
        public DateTime fecha_creacion_ { get; set; }
        [StringLength(50)]
        public string ciudad { get; set; }
        [StringLength(50)]
        public string sector { get; set; }
        [StringLength(50)]
        public string calle { get; set; }
        [StringLength(20)]
        public string telefono { get; set; }
        public int capacidad_simultanea_ { get; set; }
        [StringLength(40)]
        public string latitud { get; set; }
        [StringLength(40)]
        public string longitud { get; set; }
        [StringLength(60)]
        public string estado { get; set; }
        [StringLength(60)]
        public string nombre_peluqueria { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}