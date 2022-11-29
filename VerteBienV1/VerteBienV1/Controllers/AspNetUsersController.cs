using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VerteBienV1.Models;

namespace VerteBienV1.Controllers
{

    public class AspNetUsersController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();


 
        public ActionResult CambiarEstado(string estado, string idusuario) 
        {
            AspNetUsers usuario = db.AspNetUsers.Find(idusuario);

            usuario.estado = estado;
            db.SaveChanges();
            //Correo para la peluqueria
            string from = "kielcuentas@gmail.com"; //example:- sourabh9303@gmail.com

            if(estado == "activo") { 
                MailMessage mailUser = new MailMessage(from, usuario.Email);
                {

                    mailUser.Subject = "Suscripcion Actulizada";
                    mailUser.Body = "Su suscripcion en Verte Bien ha sido actulizada";
                    mailUser.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from, "20175376Octubre0210*");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mailUser);
                    ViewBag.Message = "Enviado con exito";
                }
            }

            if(estado == "suspendido")
            {
                MailMessage mailUser = new MailMessage(from, usuario.Email);
                {

                    mailUser.Subject = "Suscripcion Suspendida";
                    mailUser.Body = "Su suscripcion en Verte Bien ha sido suspendida por falta de pago.";
                    mailUser.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from, "20175376Octubre0210*");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mailUser);
                    ViewBag.Message = "Enviado con exito";
                }
            }
            return RedirectToAction("Index","AspNetUsers");
        }
        public string borrarImagen(string imagenParaBorrar)
        {
            string confirmado;
            String imagenBorra = Path.Combine(HttpContext.Server.MapPath("~/imagenes_local/"), imagenParaBorrar);
            if (System.IO.File.Exists(imagenBorra))
            {
                System.IO.File.Delete(imagenBorra);
                confirmado = "Borrado Exitoso";
            }
            else
            {
                confirmado = "No existe la imagen";
            }
            return confirmado;
        }
        [Authorize(Roles = "administrador")]
        // GET: AspNetUsers
        public ActionResult Index(string email)
        {
            List<AspNetUsers> usuarios = new List<AspNetUsers> ();

            if(email != null)
            {
                usuarios = (from busqueda in db.AspNetUsers where busqueda.Email.Contains(email) select busqueda).ToList();
                return View(usuarios);
            }



            return View(db.AspNetUsers.ToList());
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }

            //Separamos los nombres de las imagenes y guardamos en una lista
            List<String> imagenes = (aspNetUsers.fotos_local.Split(';')).ToList();
            //Enviamos la lista a la vista
            ViewData["imagenes_s"] = imagenes;

            return View(aspNetUsers);
        }
        [Authorize(Roles = "administrador")]
        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,nombre,apellido,ciudad,sector,calle,telefono,latitud,longitud,nombre_peluqueria,estado,fecha_nacimiento_,fecha_creacion_,capacidad_simultanea_")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUsers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUsers);
        }


        [Authorize(Roles = "expres,preferencial,vip,administrador")]
        // GET: AspNetUsers/Edit/5
        public ActionResult Edit()
        {
            SERVICIOSController validar = new SERVICIOSController();
            string idUser = User.Identity.GetUserId();
            var estatus = validar.VerificarUser(idUser);

            if (estatus == "activo" || estatus == "no fotos") 
            { 
                var id = "vacio";
                var estaAutenticado = User.Identity.IsAuthenticated;
                if (estaAutenticado)
                {
                    id = User.Identity.GetUserId();
                }
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
                if (aspNetUsers == null)
                {
                    return HttpNotFound();
                }
                //Listado de sectores
                #region
                List<string> sectores = new List<string>();
                sectores.Add("5 esquinas");
                sectores.Add("Alangasí");
                sectores.Add("Atucucho");
                sectores.Add("Bellavista");
                sectores.Add("Carcelén");
                sectores.Add("Caupichu");
                sectores.Add("Centro Histórico");
                sectores.Add("Chilibulo");
                sectores.Add("Chillogallo");
                sectores.Add("Chimbacalle");
                sectores.Add("Ciudadela del Ejército");
                sectores.Add("Ciudadela Ibarra");
                sectores.Add("Comité del Pueblo");
                sectores.Add("Conocoto");
                sectores.Add("Cornejo");
                sectores.Add("Cumbayá");
                sectores.Add("El Batán");
                sectores.Add("El Beaterio");
                sectores.Add("El Calzado");
                sectores.Add("El Camal");
                sectores.Add("El Condado");
                sectores.Add("El Dorado");
                sectores.Add("El Ejido");
                sectores.Add("El Inca");
                sectores.Add("El Panecillo");
                sectores.Add("El Pintado");
                sectores.Add("El Tejar");
                sectores.Add("El Troje");
                sectores.Add("Guajalo");
                sectores.Add("Guamaní");
                sectores.Add("Guápulo");
                sectores.Add("Iñaquito");
                sectores.Add("Kennedy");
                sectores.Add("La Argelia");
                sectores.Add("La Bota");
                sectores.Add("La Ecuatoriana");
                sectores.Add("La Ferroviaria");
                sectores.Add("La Floresta");
                sectores.Add("La Florida");
                sectores.Add("La Forestal");
                sectores.Add("La González Suárez");
                sectores.Add("La Guaragua");
                sectores.Add("La Libertad");
                sectores.Add("La Loma Grande");
                sectores.Add("La Magdalena");
                sectores.Add("La Marín");
                sectores.Add("La Mariscal");
                sectores.Add("La Mena");
                sectores.Add("La Ronda");
                sectores.Add("La Tola");
                sectores.Add("La Vicentina");
                sectores.Add("La Victoria");
                sectores.Add("Las Casas");
                sectores.Add("Lucha de los Pobres");
                sectores.Add("Luluncoto");
                sectores.Add("Manuelita Saenz");
                sectores.Add("Mena de Hierro");
                sectores.Add("Miraflores");
                sectores.Add("Monjas");
                sectores.Add("Nueva Aurora");
                sectores.Add("Oriente Quiteño");
                sectores.Add("Pifo");
                sectores.Add("Ponceano");
                sectores.Add("Puembo");
                sectores.Add("Puengasí");
                sectores.Add("Quito Norte");
                sectores.Add("Quito Sur");
                sectores.Add("Quito Tennis");
                sectores.Add("Quitumbe");
                sectores.Add("Reino de Quito");
                sectores.Add("Rumiñahui");
                sectores.Add("San");
                sectores.Add("San Carlos");
                sectores.Add("San Diego");
                sectores.Add("San Juan");
                sectores.Add("San Marcos");
                sectores.Add("San Matin");
                sectores.Add("San Rafael");
                sectores.Add("Santa Rita");
                sectores.Add("Solanda");
                sectores.Add("Tababela");
                sectores.Add("Toctiuco");
                sectores.Add("Tumbaco");
                sectores.Add("Turubamba");
                sectores.Add("Villaflora");
                #endregion
                ViewBag.fecha_nacimiento = aspNetUsers.fecha_nacimiento_;
                ViewBag.sectores = new SelectList(sectores, aspNetUsers.sector);

                //Separamos los nombres de las imagenes y guardamos en una lista

                if (aspNetUsers.fotos_local != null) 
                {
                    List<String> imagenes = (aspNetUsers.fotos_local.Split(';')).ToList();
                    //Enviamos la lista a la vista
                    ViewData["imagenes_s"] = imagenes;
                }

                return View(aspNetUsers);
            }
            else
            {
                ViewBag.respuesta = estatus;
                return RedirectToAction("pagoRequerido","SUSCRIPCIONs");
            }
        }

        // POST: AspNetUsers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, HttpPostedFileBase img5, HttpPostedFileBase img6,[Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,nombre,apellido,ciudad,sector,calle,telefono,latitud,longitud,nombre_peluqueria,estado,fecha_nacimiento_,fecha_creacion_,capacidad_simultanea_")] AspNetUsers aspNetUsers, string sectores)
        {
            AspNetUsers usuarioEdit = db.AspNetUsers.Find(aspNetUsers.Id);

            ////Aqui se rellenan los datos no modificables desde el lado del Backend por seguridad
            usuarioEdit.Email = usuarioEdit.Email;
            usuarioEdit.EmailConfirmed = usuarioEdit.EmailConfirmed;
            usuarioEdit.PasswordHash = usuarioEdit.PasswordHash;
            usuarioEdit.SecurityStamp = usuarioEdit.SecurityStamp;
            usuarioEdit.PhoneNumber = usuarioEdit.PhoneNumber;
            usuarioEdit.PhoneNumberConfirmed = usuarioEdit.PhoneNumberConfirmed;
            usuarioEdit.TwoFactorEnabled = usuarioEdit.TwoFactorEnabled;
            usuarioEdit.LockoutEnabled = usuarioEdit.LockoutEnabled;
            usuarioEdit.AccessFailedCount = usuarioEdit.AccessFailedCount;
            usuarioEdit.UserName = usuarioEdit.UserName;
            usuarioEdit.estado = usuarioEdit.estado;
            usuarioEdit.fecha_creacion_ = usuarioEdit.fecha_creacion_;
            usuarioEdit.capacidad_simultanea_ = usuarioEdit.capacidad_simultanea_;
            //////---FIN DATOS NO MODIFICABLES
            ///
            ///Datos si modificables
            usuarioEdit.nombre = aspNetUsers.nombre;
            usuarioEdit.apellido = aspNetUsers.apellido;
            usuarioEdit.ciudad = aspNetUsers.ciudad;
            usuarioEdit.sector = sectores;
            usuarioEdit.calle = aspNetUsers.calle;
            usuarioEdit.telefono = aspNetUsers.telefono;
            usuarioEdit.latitud = aspNetUsers.latitud;
            usuarioEdit.longitud = aspNetUsers.longitud;
            usuarioEdit.nombre_peluqueria = aspNetUsers.nombre_peluqueria;
            usuarioEdit.fecha_nacimiento_ = aspNetUsers.fecha_nacimiento_;

            //Guardado y edicion de imagenes.//
            //Variable para generar numeros ramdon para el nombre de las imagenes.
            Random rnd = new Random();

        if(usuarioEdit.fotos_local == null) 
        { 
            //Establecemos la ruta donde se guardaran las imagenes
            if (img1 != null)
            {

                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + aspNetUsers.Id + img1.FileName;
                    String ruta_img1 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                    img1.SaveAs(ruta_img1);
                    aspNetUsers.fotos_local = nuevaImagen;
                }
                if (img2 != null)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + aspNetUsers.Id + img2.FileName;
                    String ruta_img2 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                    img2.SaveAs(ruta_img2);
                    aspNetUsers.fotos_local += ";" + nuevaImagen;
                }
                if (img3 != null)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + aspNetUsers.Id + img3.FileName;
                    String ruta_img3 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                    img3.SaveAs(ruta_img3);
                    aspNetUsers.fotos_local += ";" + nuevaImagen;
                }
                if (img4 != null)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + aspNetUsers.Id + img4.FileName;
                    String ruta_img4 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                    img4.SaveAs(ruta_img4);
                    aspNetUsers.fotos_local += ";" + nuevaImagen;
                }
                if (img5 != null)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + aspNetUsers.Id + img5.FileName;
                    String ruta_img5 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                    img5.SaveAs(ruta_img5);
                    aspNetUsers.fotos_local += ";" + nuevaImagen;

                }
                if (img6 != null)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + aspNetUsers.Id + img6.FileName;
                    String ruta_img6 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img6.SaveAs(ruta_img6);
                    aspNetUsers.fotos_local += ";" + nuevaImagen;
                }
                //Rellenar campo Fotos
                usuarioEdit.fotos_local = aspNetUsers.fotos_local;
            }
            else 
            {
                //Separamos los nombres de las imagenes y guardamos en una lista
                List<String> imagenes = (usuarioEdit.fotos_local.Split(';')).ToList();
                //Establecemos la ruta donde se guardaran las imagenes
                if (img1 != null)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + aspNetUsers.Id + img1.FileName;
                    usuarioEdit.fotos_local = usuarioEdit.fotos_local.Replace(imagenes[0], nuevaImagen);
                    ViewBag.comprobar = usuarioEdit.fotos_local;
                    String ruta_img1 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                    img1.SaveAs(ruta_img1);
                    var borrar = borrarImagen(imagenes[0]);
                }
                if (img2 != null)
                {
                    if (imagenes.Count < 2)
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img2.FileName;
                        String ruta_img2 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img2.SaveAs(ruta_img2);
                        usuarioEdit.fotos_local += ";" + nuevaImagen;
                    }
                    else
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img2.FileName;
                        usuarioEdit.fotos_local = usuarioEdit.fotos_local.Replace(imagenes[1], nuevaImagen);
                        ViewBag.comprobar = usuarioEdit.fotos_local;
                        String ruta_img2 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img2.SaveAs(ruta_img2);
                        var borrar = borrarImagen(imagenes[1]);
                    }
                }
                if (img3 != null)
                {
                    if (imagenes.Count < 3)
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img3.FileName;
                        String ruta_img3 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img3.SaveAs(ruta_img3);
                        usuarioEdit.fotos_local += ";" + nuevaImagen;
                    }
                    else
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img3.FileName;
                        usuarioEdit.fotos_local = usuarioEdit.fotos_local.Replace(imagenes[2], nuevaImagen);
                        ViewBag.comprobar = usuarioEdit.fotos_local;
                        String ruta_img3 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img3.SaveAs(ruta_img3);
                        var borrar = borrarImagen(imagenes[2]);
                    }
                }
                if (img4 != null)
                {
                    if (imagenes.Count < 4)
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img4.FileName;
                        String ruta_img4 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img4.SaveAs(ruta_img4);
                        usuarioEdit.fotos_local += ";" + nuevaImagen;
                    }
                    else
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img4.FileName;
                        usuarioEdit.fotos_local = usuarioEdit.fotos_local.Replace(imagenes[3], nuevaImagen);
                        ViewBag.comprobar = usuarioEdit.fotos_local;
                        String ruta_img4 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img4.SaveAs(ruta_img4);
                        var borrar = borrarImagen(imagenes[3]);
                    }

                }
                if (img5 != null)
                {
                    if (imagenes.Count < 5)

                    {
                            int numero = rnd.Next(52);
                            Convert.ToString(numero).Trim();
                            var nuevaImagen = numero + aspNetUsers.Id + img5.FileName;
                            String ruta_img5 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                            img5.SaveAs(ruta_img5);
                            usuarioEdit.fotos_local += ";" + nuevaImagen;
                    }
                    else
                    {
                            int numero = rnd.Next(52);
                            Convert.ToString(numero).Trim();
                            var nuevaImagen = numero + aspNetUsers.Id + img5.FileName;
                            usuarioEdit.fotos_local = usuarioEdit.fotos_local.Replace(imagenes[4], nuevaImagen);
                            ViewBag.comprobar = usuarioEdit.fotos_local;
                            String ruta_img5 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                            img5.SaveAs(ruta_img5);
                            var borrar = borrarImagen(imagenes[4]);
                    }
                }
                if (img6 != null)
                {
                    if (imagenes.Count < 6)
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img6.FileName;
                        String ruta_img6 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img6.SaveAs(ruta_img6);
                        usuarioEdit.fotos_local += ";" + nuevaImagen;
                    }
                    else
                    {
                        int numero = rnd.Next(52);
                        Convert.ToString(numero).Trim();
                        var nuevaImagen = numero + aspNetUsers.Id + img6.FileName;
                        usuarioEdit.fotos_local = usuarioEdit.fotos_local.Replace(imagenes[5], nuevaImagen);
                        ViewBag.comprobar = usuarioEdit.fotos_local;
                        String ruta_img6 = Server.MapPath("~/imagenes_local/") + nuevaImagen;
                        img6.SaveAs(ruta_img6);
                        var borrar = borrarImagen(imagenes[5]);
                    }
                }

            }



            if (ModelState.IsValid)
            {


                //db.Entry(aspNetUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUsers);
        }
        [Authorize(Roles = "administrador")]
        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [Authorize(Roles = "administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
