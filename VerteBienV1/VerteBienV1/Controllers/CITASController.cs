using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VerteBienV1.Models;
using Newtonsoft.Json;

namespace VerteBienV1.Controllers
{
    [Authorize]
    public class CITASController : Controller
    {
        public class disponible
        {
            public int id { get; set; }
            public string hora { get; set; }
            public string tiempo { get; set; }
            public string disponibilidad { get; set; }
        }

        public class Mensaje
        {
            public string menssage { get; set; }
        }


        private VERTEBIENEntities db = new VERTEBIENEntities();

        // GET: CITAS
        [Authorize]
        public ActionResult Index(string estatusCitasSelec)
        {
            //Se obtiene el ID del usuairo logueado.
            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
            }
            //var cITAS = db.CITAS.Include(c => c.AspNetUsers).Include(c => c.SERVICIOS);
            //Lista que guarda el resultado de la Busqueda
            List<CITAS> citasUsuario = new List<CITAS>();
            //Cargas de listado por el tipo de Usuario
            if (User.IsInRole("cliente")) 
            { 
            citasUsuario = (from citas in db.CITAS where citas.id_usuario == id && citas.estado == "pendiente" select citas).ToList();
            }
            else
            {
                if(estatusCitasSelec != null) { 
                citasUsuario = (from citas in db.CITAS 
                                join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio  
                                where servicios.id_usuario == id && citas.estado == estatusCitasSelec
                                select citas).ToList();
                }
                else
                {
                    citasUsuario = (from citas in db.CITAS
                                    join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio
                                    where servicios.id_usuario == id select citas).ToList();
                }
            }

            return View(citasUsuario);
        }
        //Modulo de Contabilidad
        [Authorize(Roles = "preferencial,vip,administrador")]
        public ActionResult contabilidad(DateTime? desde, DateTime? hasta) 
        {
            SERVICIOSController validar = new SERVICIOSController();
            var idUser = User.Identity.GetUserId();
            var estatus = validar.VerificarUser(idUser);
            if(estatus == "activo" || estatus=="no fotos") 
            { 
                //Lista de Citas
                List<CITAS> citasPeluqueria = new List<CITAS>();
                //Se obtiene el ID del usuairo logueado.
                var id = "vacio";
                var estaAutenticado = User.Identity.IsAuthenticated;
                if (estaAutenticado)
                {
                    id = User.Identity.GetUserId();
                }
                if (desde != null && hasta != null) {
                    citasPeluqueria = (from citas in db.CITAS
                                       join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio
                                       where citas.fecha_cita >= desde && citas.fecha_cita <= hasta && citas.estado == "completado" && citas.SERVICIOS.id_usuario == id
                                       select citas).ToList();
                    return View(citasPeluqueria);
                }
                //Se obtiene el listado de citas.

                citasPeluqueria = (from citas in db.CITAS join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio 
                                   where citas.estado == "completado" && citas.SERVICIOS.id_usuario == id select citas).ToList();
                return View(citasPeluqueria);
            }
            else
            {
                ViewBag.respuesta = estatus;
                return RedirectToAction("pagoRequerido", "SUSCRIPCIONs");
            }
        }
        [Authorize]
        // GET: CITAS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CITAS cITAS = db.CITAS.Find(id);
            if (cITAS == null)
            {
                return HttpNotFound();
            }
            return View(cITAS);
        }
        [Authorize]
        public String Disponibilidad(int idservicio, DateTime fecha, string dia)
        {
            int id_servicio = idservicio;
            dia = "semanal";
            SERVICIOS sERVICIO = db.SERVICIOS.Find(idservicio);
            List<disponible> resultadoBusqueda = new List<disponible>();

             resultadoBusqueda = db.Database.SqlQuery<disponible>("disponibilidad @usuario_peluqueria, @Fecha, @dia, @servicio_id", new SqlParameter("@usuario_peluqueria", sERVICIO.id_usuario), new SqlParameter("@Fecha", fecha), new SqlParameter("@dia", dia), new SqlParameter("@servicio_id",id_servicio)).ToList();

            var resultadoDisponibilidad = JsonConvert.SerializeObject(resultadoBusqueda);

            return resultadoDisponibilidad;
        }
        [Authorize]
        public String AgregarCita(int idservicio, DateTime fecha, decimal hora, string comentario_cliente, string comentario_peluqueria) 
        {
            string estatus = "suspendido";
            string resultado = "no";
            List<SERVICIOS> idPeluqueria = new List<SERVICIOS>();
            idPeluqueria = (from busqueda in db.SERVICIOS where busqueda.id_servicio == idservicio select busqueda).ToList();
            foreach(var item in idPeluqueria)
            {
                SERVICIOSController validar = new SERVICIOSController();
                estatus = validar.VerificarUser(item.id_usuario);
            }
            if(estatus == "activo") 
            { 
                var dia = "semanal";
                SERVICIOS sERVICIO = db.SERVICIOS.Find(idservicio);
                var idcliente = User.Identity.GetUserId();
                List<CITAS> agregarCita = new List<CITAS>();

                agregarCita = db.Database.SqlQuery<CITAS>("addCita @usuario_peluqueria, @Fecha, @hora, @servicio, @dia, @usuario_cita", new SqlParameter("@usuario_peluqueria", sERVICIO.id_usuario), new SqlParameter("@Fecha", fecha), new SqlParameter("@hora", hora), new SqlParameter("@servicio", idservicio), new SqlParameter("@dia", dia), new SqlParameter("@usuario_cita", idcliente), new SqlParameter ("@comentario_cliente", comentario_cliente), new SqlParameter( "@comentario_peluqueria", comentario_peluqueria)).ToList();

                if (agregarCita.Count > 0)
                {
                    resultado = "si";
                    return resultado;
                }
            }
            else
            {
                return estatus;
            }
            return resultado;
        }

        [Authorize]
        // GET: CITAS/Createx
        public ActionResult Create(int idServicio)
        {
            SERVICIOS sERVICIOS = db.SERVICIOS.Find(idServicio);
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            //ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "nombre_servicio", sERVICIOS.id_servicio);
            ViewBag.servicio_selec = sERVICIOS.nombre_servicio;
            ViewBag.id_servicio = idServicio;
            ViewBag.tiempo = sERVICIOS.tiempo;
            return View();
        }

        // POST: CITAS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "id_cita,id_usuario,id_servicio,fecha_creacion,fecha_cita,estado")] CITAS cITAS)
        {
            var id = "vacio";
            var emailUsuario = ""; // Variable para capturar el email del user
            //Capturar email del negocio
            SERVICIOS servicioSelec = db.SERVICIOS.Find(cITAS.id_servicio);
            AspNetUsers usuarioNegocio = db.AspNetUsers.Find(servicioSelec.id_usuario);
            var emailNegocio = usuarioNegocio.Email;

            //Capturar el id dle usuario logueado.
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
                AspNetUsers usuario = db.AspNetUsers.Find(id);
                emailUsuario = usuario.Email;
            }
            cITAS.id_usuario = id;
            cITAS.fecha_creacion = DateTime.Today;
            cITAS.estado = "Pendiente";

            if (ModelState.IsValid)
            {
                db.CITAS.Add(cITAS);
                db.SaveChanges();
                //Correo para el usuario
                string from = "kielcuentas@gmail.com"; //example:- sourabh9303@gmail.com
                
                //Inserta la imagen
                LinkedResource theEmailImage = new LinkedResource("~/Imagenes/Verte Bien Transparente 2.png");
                theEmailImage.ContentId = "myImageID";

                MailMessage mailUser = new MailMessage(from, emailUsuario);
                {

                    mailUser.Subject = "Su cita ha sido creada con exito";
                    mailUser.Body = @"<center> <img src=cid:myImageID> </center>" +
                                     @"<style>
                                       h4{text-align:justify;
                                          margin-top: 2%;}
                                         </style>
                        <h4>Gracias por usar Verte Bien, su cita en " + usuarioNegocio.nombre_peluqueria+" ha sido creada con exito, le avisaremos cuando la peluqueria acepte su cita</h4>";
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
                //Correo para el Negocio
                MailMessage mailNegocio = new MailMessage(from, emailNegocio);
                {

                    mailNegocio.Subject = "Nueva Solicitud de Cita";
                    mailNegocio.Body = "Un usuario ha creado una nueva cita, por favor verificar y proceder a aceptar o cancelar la misma.";
                    mailNegocio.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from, "20175376Octubre0210*");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mailNegocio);
                    ViewBag.Message = "Enviado con exito";

                }
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", cITAS.id_usuario);
            ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "id_usuario", cITAS.id_servicio);
            return View(cITAS);
        }
        [Authorize]
        // GET: CITAS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var idPeluqueria = User.Identity.GetUserId();
            CITAS cITAS = db.CITAS.Find(id);
            ViewBag.idCita = id;
            if (cITAS == null || cITAS.SERVICIOS.id_usuario != idPeluqueria)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", cITAS.id_usuario);
            ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "nombre_servicio", cITAS.id_servicio);
            return View(cITAS);
        }

        // POST: CITAS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "id_cita,id_usuario,id_servicio,fecha_creacion,fecha_cita,estado")] CITAS cITAS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cITAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", cITAS.id_usuario);
            ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "id_usuario", cITAS.id_servicio);
            return View(cITAS);
        }
        [Authorize(Roles = "administrador")]
        // GET: CITAS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CITAS cITAS = db.CITAS.Find(id);
            if (cITAS == null)
            {
                return HttpNotFound();
            }
            return View(cITAS);
        }

        // POST: CITAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            CITAS cITAS = db.CITAS.Find(id);
            db.CITAS.Remove(cITAS);
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

        public ActionResult EstatusCita(int id, string estatus)
        {
            CITAS cITAS = db.CITAS.Find(id);
            //Obtenemos EMAIL dle usuario
            AspNetUsers emailUsuario = db.AspNetUsers.Find(cITAS.id_usuario);
            //Obtenemos El servicio seleccionado para asi buscar el nombre de la peluqueria.
            SERVICIOS servicioSelec = db.SERVICIOS.Find(cITAS.id_servicio);
            AspNetUsers usuarioNegocio = db.AspNetUsers.Find(servicioSelec.id_usuario);

            if (estatus == "aceptado")
            {
                cITAS.estado = "aceptado";
                ////Correo para el usuario
                //string from = "kielcuentas@gmail.com"; //example:- sourabh9303@gmail.com
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                MailMessage mailUser = new MailMessage();

                    mailUser.From = new MailAddress("postmaster@vertebien.net");
                    mailUser.To.Add(emailUsuario.Email);
                    mailUser.Subject = "Su Cita ha sido Aceptada";
                    mailUser.Body = "Gracias por usar Verte Bien, su cita en " + usuarioNegocio.nombre_peluqueria + " ha sido aceptada, puede ir al establecimiento en la fecha establecida, le recomendamos llegar 5 minutos antes para no surfir el riesgo de perder la misma.";
                    mailUser.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail5002.site4now.net";
                    
                    NetworkCredential networkCredential = new NetworkCredential("postmaster@vertebien.net", "VerteBien2021*"); 
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 465;
                    smtp.EnableSsl = true;
                    smtp.Send(mailUser);
                    ViewBag.Message = "Enviado con exito";

            }
            if (estatus == "cancelado")
            {
                cITAS.estado = "cancelado";
                if (User.IsInRole("Cliente"))
                {
                    //Correo para la peluqueria
                    string from = "kielcuentas@gmail.com"; //example:- sourabh9303@gmail.com
                    MailMessage mailUser = new MailMessage(from, usuarioNegocio.Email);
                    {

                        mailUser.Subject = "Cita  Cancelada";
                        mailUser.Body = "Lamentamos informarle que el usuario: " + emailUsuario.nombre + " " + emailUsuario.apellido + " ha cancelado su cita programada " + cITAS.fecha_cita + ".";
                        mailUser.IsBodyHtml = false;
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
                else
                {
                    //Correo para el usuario
                    string from = "kielcuentas@gmail.com"; //example:- sourabh9303@gmail.com
                    MailMessage mailUser = new MailMessage(from, emailUsuario.Email);
                    {
                        mailUser.Subject = "Su Cita ha sido Cancelada";
                        mailUser.Body = "Gracias por usar Verte Bien, su cita en " + usuarioNegocio.nombre_peluqueria + " ha sido cancelada, lamentablemente el lugar no puede atenderle en ese horario, disculpe los incovenientes causados, puede reagendar otra cita cuando cuando usted desee.";
                        mailUser.IsBodyHtml = false;
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
            }
            if (estatus == "completado")
            {
                var linkPuntuacionServicio = Url.Action("Create", "PUNTUACION_SERVICIOS", new {cITAS.id_cita}, protocol: Request.Url.Scheme);
                var linkPuntuacionPeluqueria = Url.Action("Create", "PUNTUACION_PELUQUERIA", new { cITAS.id_cita }, protocol: Request.Url.Scheme);
                cITAS.estado = "completado";
                //Correo para el usuario
                string from = "kielcuentas@gmail.com"; //example:- sourabh9303@gmail.com
                MailMessage mailUser = new MailMessage(from, emailUsuario.Email);
                {

                    mailUser.Subject = "Su Cita ha sido completado";
                    mailUser.Body = "Gracias por usar Verte Bien, su cita en " + usuarioNegocio.nombre_peluqueria + " ha sido completada. Por Favor cuentenos que tal su experiencia llenando este formulario. Para Puntuacion del Servicio: <a href=\"" + linkPuntuacionServicio + "\">Click Aqui</a> Para Puntuacion de la peluqueria: <a href=\"" + linkPuntuacionPeluqueria + "\">Click Aqui</a>";
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

            db.Entry(cITAS).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }

}

