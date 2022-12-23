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
                ViewBag.respuesta = estatus;
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
                sectores.Add("24 de Mayo");
                sectores.Add("Aguarico");
                sectores.Add("Alausí");
                sectores.Add("Alfredo Baquerizo");
                sectores.Add("Ambato");
                sectores.Add("Antonio Ante");
                sectores.Add("Arajuno");
                sectores.Add("Archidona");
                sectores.Add("Arenillas");
                sectores.Add("Atacames");
                sectores.Add("Atahualpa");
                sectores.Add("Azogues");
                sectores.Add("Baba");
                sectores.Add("Babahoyo");
                sectores.Add("Balao");
                sectores.Add("Balsas");
                sectores.Add("Balzar");
                sectores.Add("Baños");
                sectores.Add("Biblián");
                sectores.Add("Bolívar");
                sectores.Add("Buena Fe");
                sectores.Add("Caluma");
                sectores.Add("Calvas");
                sectores.Add("Camilo Ponce Enríquez");
                sectores.Add("Cañar");
                sectores.Add("Carlos Julio Arosemena Tola");
                sectores.Add("Cascales");
                sectores.Add("Catamayo");
                sectores.Add("Cayambe");
                sectores.Add("Celica");
                sectores.Add("Centinela del Cóndor");
                sectores.Add("Cevallos");
                sectores.Add("Chaguarpamba");
                sectores.Add("Chambo");
                sectores.Add("Chilla");
                sectores.Add("Chillanes");
                sectores.Add("Chimbo");
                sectores.Add("Chinchipe");
                sectores.Add("Chone");
                sectores.Add("Chordeleg");
                sectores.Add("Chunchi");
                sectores.Add("Colimes");
                sectores.Add("Colta");
                sectores.Add("Cotacachi");
                sectores.Add("Cuenca");
                sectores.Add("Cumandá");
                sectores.Add("Cuyabeno");
                sectores.Add("Daule");
                sectores.Add("Déleg");
                sectores.Add("Durán");
                sectores.Add("Echeandía");
                sectores.Add("El Carmen");
                sectores.Add("El Chaco");
                sectores.Add("El Empalme");
                sectores.Add("El Guabo");
                sectores.Add("El Pan");
                sectores.Add("El Pangui");
                sectores.Add("El Tambo");
                sectores.Add("El Triunfo");
                sectores.Add("Eloy Alfaro");
                sectores.Add("Esmeraldas");
                sectores.Add("Espejo");
                sectores.Add("Espíndola");
                sectores.Add("Flavio Alfaro");
                sectores.Add("Francisco de Orellana");
                sectores.Add("General Antonio Elizale");
                sectores.Add("Girón");
                sectores.Add("Gonzalo Pizarro");
                sectores.Add("Gonzanamá");
                sectores.Add("Guachapala");
                sectores.Add("Gualaceo");
                sectores.Add("Gualaquiza");
                sectores.Add("Guamote");
                sectores.Add("Guano");
                sectores.Add("Guaranda");
                sectores.Add("Guayaquil");
                sectores.Add("Huaca");
                sectores.Add("Huamboya");
                sectores.Add("Huaquillas");
                sectores.Add("Ibarra");
                sectores.Add("Isabela");
                sectores.Add("Isidro Ayora");
                sectores.Add("Jama");
                sectores.Add("Jaramijó");
                sectores.Add("Jipijapa");
                sectores.Add("Junín");
                sectores.Add("La Concordia");
                sectores.Add("La Joya de los Sachas");
                sectores.Add("La Libertad");
                sectores.Add("La Maná");
                sectores.Add("La Troncal");
                sectores.Add("Lago Agrio");
                sectores.Add("Las Lajas");
                sectores.Add("Las Naves");
                sectores.Add("Latacunga");
                sectores.Add("Limón Indanza");
                sectores.Add("Logroño");
                sectores.Add("Loja");
                sectores.Add("Lomas de Sargentillo");
                sectores.Add("Loreto");
                sectores.Add("Macará");
                sectores.Add("Machala");
                sectores.Add("Manta");
                sectores.Add("Marcabelí");
                sectores.Add("Marcelino Maridueña");
                sectores.Add("Mejía");
                sectores.Add("Milagro");
                sectores.Add("Mocacho");
                sectores.Add("Mocha");
                sectores.Add("Montalvo");
                sectores.Add("Montecristi");
                sectores.Add("Montúfar");
                sectores.Add("Morona");
                sectores.Add("Muisne");
                sectores.Add("Nabón");
                sectores.Add("Nangaritza");
                sectores.Add("Naranjal");
                sectores.Add("Naranjito");
                sectores.Add("Nobol");
                sectores.Add("Olmedo");
                sectores.Add("Oña");
                sectores.Add("Otavalo");
                sectores.Add("Pablo Sexto");
                sectores.Add("Paján");
                sectores.Add("Palenque");
                sectores.Add("Palestina");
                sectores.Add("Pallatanga");
                sectores.Add("Palora");
                sectores.Add("Paltas");
                sectores.Add("Pangua");
                sectores.Add("Paquisha");
                sectores.Add("Pasaje");
                sectores.Add("Pastaza");
                sectores.Add("Patate");
                sectores.Add("Paute");
                sectores.Add("Pedernales");
                sectores.Add("Pedro Carbo");
                sectores.Add("Pedro Moncayo");
                sectores.Add("Pedro Vicente Maldonado");
                sectores.Add("Penipe");
                sectores.Add("Pichincha");
                sectores.Add("Pimampiro");
                sectores.Add("Pindal");
                sectores.Add("Piñas");
                sectores.Add("Playas");
                sectores.Add("Portovelo");
                sectores.Add("Portoviejo");
                sectores.Add("Pucará");
                sectores.Add("Puebloviejo");
                sectores.Add("Puerto López");
                sectores.Add("Puerto Quito");
                sectores.Add("Pujilí");
                sectores.Add("Putumayo");
                sectores.Add("Puyango");
                sectores.Add("Quero");
                sectores.Add("Quevedo");
                sectores.Add("Quijos");
                sectores.Add("Quilanga");
                sectores.Add("Quinindé");
                sectores.Add("Quinsaloma");
                sectores.Add("Quito");
                sectores.Add("Riobamba");
                sectores.Add("Rioverde");
                sectores.Add("Rocafuerte");
                sectores.Add("Rumiñahui");
                sectores.Add("Salcedo");
                sectores.Add("Salinas");
                sectores.Add("Salitre");
                sectores.Add("Samborondón");
                sectores.Add("San Cristóbal");
                sectores.Add("San Fernando");
                sectores.Add("San Juan Bosco");
                sectores.Add("San Lorenzo");
                sectores.Add("San Miguel");
                sectores.Add("San Miguel de Urcuquí");
                sectores.Add("San Miguel de los Bancos");
                sectores.Add("San Pedro de Pelileo");
                sectores.Add("San Vicente");
                sectores.Add("Santa Ana");
                sectores.Add("Santa Clara");
                sectores.Add("Santa Cruz");
                sectores.Add("Santa Elena");
                sectores.Add("Santa Isabel");
                sectores.Add("Santa Lucía");
                sectores.Add("Santa Rosa");
                sectores.Add("Santiago de Méndez");
                sectores.Add("Santiago de Píllaro");
                sectores.Add("Santo Domingo");
                sectores.Add("Saquisilí");
                sectores.Add("Saraguro");
                sectores.Add("Sevilla de Oro");
                sectores.Add("Shushufindi");
                sectores.Add("Sigchos");
                sectores.Add("Sígsig");
                sectores.Add("Simón Bolívar");
                sectores.Add("Sozoranga");
                sectores.Add("Sucre");
                sectores.Add("Sucúa");
                sectores.Add("Sucumbíos");
                sectores.Add("Suscal");
                sectores.Add("Taisha");
                sectores.Add("Tena");
                sectores.Add("Tisaleo");
                sectores.Add("Tiwintza");
                sectores.Add("Tosagua");
                sectores.Add("Tulcán");
                sectores.Add("Urdaneta");
                sectores.Add("Valencia");
                sectores.Add("Ventanas");
                sectores.Add("Vinces");
                sectores.Add("Yacuambi");
                sectores.Add("Yaguachi");
                sectores.Add("Yantzaza");
                sectores.Add("Zamora");
                sectores.Add("Zapotillo");
                sectores.Add("Zaruma");

                #endregion

                List<string> ciudades = new List<string>();
                ViewBag.fecha_nacimiento = aspNetUsers.fecha_nacimiento_;
                ViewBag.sectores = new SelectList(sectores, aspNetUsers.sector);
                ViewBag.ciudades = new SelectList(ciudades, aspNetUsers.ciudad);

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
                
                return RedirectToAction("pagoRequerido","SUSCRIPCIONs", new { estatus });
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
