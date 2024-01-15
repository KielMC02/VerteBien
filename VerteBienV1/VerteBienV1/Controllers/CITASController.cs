﻿using Microsoft.AspNet.Identity;
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
            public string mensaje { get; set; }
            
        }


        private VERTEBIENEntities db = new VERTEBIENEntities();

        // GET: CITAS
        [Authorize]
        //[OutputCache(Duration = 3600, VaryByParam = "estatusCitasSelec")]
        public ActionResult Index(string estatusCitasSelec, DateTime? desde, DateTime? hasta)
        {

            //Se obtiene el ID del usuairo logueado.
            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
            }
            //Validar estado del usuario.
            SERVICIOSController validar = new SERVICIOSController();
            var estatus = validar.VerificarUser(id);
            if(estatus == "suspendido" || estatus == "new")
            {
                return RedirectToAction("pagoRequerido", "SUSCRIPCIONs");
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
                //Si el usuario filtra solo por estado
                if (estatusCitasSelec != null && desde == null && hasta == null) 
                { 
                citasUsuario = (from citas in db.CITAS 
                                join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio  
                                where servicios.id_usuario == id && citas.estado == estatusCitasSelec
                                select citas).ToList();
                    return View(citasUsuario);
                }
                //Si el usuario filtra solo por fechas
                if ((estatusCitasSelec == "" || estatusCitasSelec == null) && desde != null && hasta != null)
                {
                    citasUsuario = (from citas in db.CITAS
                                    join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio
                                    where citas.fecha_cita >= desde && citas.fecha_cita <= hasta && citas.SERVICIOS.id_usuario == id
                                    select citas).ToList();
                    return View(citasUsuario);
                }
                //Si el usuario filtra por estado y fechas
                if (estatusCitasSelec != null && desde != null && hasta != null) 
                {
                    citasUsuario = (from citas in db.CITAS
                                    join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio
                                    where citas.fecha_cita >= desde && citas.fecha_cita <= hasta && citas.estado == estatusCitasSelec && citas.SERVICIOS.id_usuario == id
                                    select citas).ToList();
                    return View(citasUsuario);
                }
                //Si el usuario no filtra nada
                if((estatusCitasSelec == null || estatusCitasSelec == "") && desde == null && hasta == null)
                {
                    citasUsuario = (from citas in db.CITAS
                                    join servicios in db.SERVICIOS on citas.id_servicio equals servicios.id_servicio
                                    where servicios.id_usuario == id
                                    select citas).ToList();
                    return View(citasUsuario);
                }
            }
            return View(citasUsuario);

        }
        //Modulo de Contabilidad
        [Authorize(Roles = "preferencial,vip,administrador")]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult contabilidad(DateTime? desde, DateTime? hasta) 
        {
            SERVICIOSController validar = new SERVICIOSController();
            var idUser = User.Identity.GetUserId();
            var estatus = validar.VerificarUser(idUser);
            if(estatus == "activo" || estatus == "no fotos" ||estatus == "redes" || estatus == "horario") 
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
                
                return RedirectToAction("pagoRequerido", "SUSCRIPCIONs", new { estatus });
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
        public String AgregarCita(int idservicio, DateTime fecha, decimal hora, string comentarios, string comentario_peluqueria) 
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
                //Obtenemos el dia de la fecha
                var tipoDia = fecha.DayOfWeek;
                string dia = "";
                switch (tipoDia)
                {
                    case DayOfWeek.Saturday:
                        dia = "sabado";
                            break;
                    case DayOfWeek.Sunday:
                        dia = "domingo";
                            break;
                    default:
                        dia = "semanal";
                        break;
                }

                //Variable a utilizar en caso de que uno de los parametros sea un string vacio
                var nulo = DBNull.Value;
                if (comentarios == "")
                {
                    comentarios = null;
                }
                //if(comentario_peluqueria == null)
                //{
                //    comentario_peluqueria = "Sin comentarios";
                //}


                SERVICIOS sERVICIO = db.SERVICIOS.Find(idservicio);
                var idcliente = User.Identity.GetUserId();
                List<Mensaje> agregarCita = new List<Mensaje>();

                 agregarCita = db.Database.SqlQuery<Mensaje>("addCita @usuario_peluqueria, @Fecha, @hora, @servicio, @dia, @usuario_cita, @comentario_cliente, @comentario_peluqueria", new SqlParameter("@usuario_peluqueria", sERVICIO.id_usuario), new SqlParameter("@Fecha", fecha), new SqlParameter("@hora", hora), new SqlParameter("@servicio", idservicio), new SqlParameter("@dia", dia), new SqlParameter("@usuario_cita", idcliente), new SqlParameter("@comentario_cliente", comentarios == null ? (object)nulo : comentarios), new SqlParameter("@comentario_peluqueria", comentario_peluqueria == null ? (object)nulo : comentario_peluqueria)).ToList();


                foreach (var item in agregarCita)
                {
                    if (item.mensaje == "si")
                    {
                        resultado = "si";
                        SERVICIOS servicioSelec = db.SERVICIOS.Find(idservicio);
                        AspNetUsers usuarioNegocio = db.AspNetUsers.Find(servicioSelec.id_usuario);
                        AspNetUsers clienteCita = db.AspNetUsers.Find(idcliente);
                        var emailNegocio = usuarioNegocio.Email;

                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        MailMessage mailUser = new MailMessage();
                        mailUser.From = new MailAddress("informaciones@vertebien.net");
                        mailUser.To.Add(usuarioNegocio.Email);
                        mailUser.Subject = "Nueva Solicitud de Cita";
                        mailUser.Body = "Gracias por usar Verte Bien, el cliente " + clienteCita.Email + " ha creado una cita, por favor verifique la cita proceda a aceptar o cancelar la misma.";
                        mailUser.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("mail5009.site4now.net");
                        //smtp.Host = "mail5009.site4now.net";
                        NetworkCredential networkCredential = new NetworkCredential("informaciones@vertebien.net", "Octubre0210*");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = networkCredential;
                        smtp.Port = 8889;
                        smtp.EnableSsl = false;
                        smtp.Send(mailUser);
                        ViewBag.Message = "Enviado con exito";
                        return resultado;
                    }
                    else
                    {
                        resultado = "no";
                        return resultado;
                    }
                }

                //if (agregarCita[0].Contains("si"))
                //{
                //    resultado = "si";
                //    return resultado;
                //}
                //if (Convert.ToString(agregarCita[0]) == "no")
                //{
                //    resultado = "no";
                //    return resultado;
                //}
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
                string from = "informaciones@vertebien.net"; //example:- sourabh9303@gmail.com
                
                //Inserta la imagen
                LinkedResource theEmailImage = new LinkedResource("~/Imagenes/Verte Bien negro.png");
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
                    smtp.EnableSsl = false;
                    NetworkCredential networkCredential = new NetworkCredential(from, "Octubre0210*");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 889;
                    smtp.Send(mailUser);
                    ViewBag.Message = "Enviado con exito";

                }
                //Correo para el Negocio
                MailMessage mailNegocio = new MailMessage(from, emailNegocio);
                {

                    mailNegocio.Subject = "Nueva Solicitud de Cita";
                    mailNegocio.Body = "Un usuario ha creado una nueva cita, por favor verificar y proceder a aceptar o cancelar la misma.";
                    mailNegocio.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = false;
                    NetworkCredential networkCredential = new NetworkCredential(from, "Octubre0210*");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 889;
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
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                MailMessage mailUser = new MailMessage();
                mailUser.From = new MailAddress("informaciones@vertebien.net");
                mailUser.To.Add(emailUsuario.Email);
                mailUser.Subject = "Su Cita ha sido Aceptada";
                mailUser.Body = "Gracias por usar Verte Bien, su cita en " + usuarioNegocio.nombre_peluqueria + " ha sido aceptada, puede ir al establecimiento en la fecha establecida, le recomendamos llegar 5 minutos antes para no para su cita.";
                mailUser.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("mail5009.site4now.net");
                //smtp.Host = "mail5009.site4now.net";
                NetworkCredential networkCredential = new NetworkCredential("informaciones@vertebien.net", "Octubre0210*");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.Port = 8889;
                smtp.EnableSsl = false;
                smtp.Send(mailUser);
                ViewBag.Message = "Enviado con exito";
            }
            if (estatus == "cancelado")
            {
                cITAS.estado = "cancelado";
                if (User.IsInRole("Cliente"))
                {
                    //Correo para la peluqueria
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    MailMessage mailUser = new MailMessage();
                    mailUser.From = new MailAddress("informaciones@vertebien.net");
                    mailUser.To.Add(usuarioNegocio.Email);
                    mailUser.Subject = "Una cita ha sido cancelada";
                    mailUser.Body = "Lamentamos informarle que el usuario: " + emailUsuario.nombre + " " + emailUsuario.apellido + " ha cancelado su cita programada para; " + cITAS.fecha_cita + ".";
                    mailUser.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("mail5009.site4now.net");
                    //smtp.Host = "mail5009.site4now.net";
                    NetworkCredential networkCredential = new NetworkCredential("informaciones@vertebien.net", "Octubre0210*");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 8889;
                    smtp.EnableSsl = false;
                    smtp.Send(mailUser);
                    ViewBag.Message = "Enviado con exito";


                }
                else
                {
                    //Correo Cancelar Usuario
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    MailMessage mailUser = new MailMessage();
                    mailUser.From = new MailAddress("informaciones@vertebien.net");
                    mailUser.To.Add(usuarioNegocio.Email);
                    mailUser.Subject = "Una cita ha sido cancelado";
                    mailUser.Body = "Gracias por usar Verte Bien, su cita en " + usuarioNegocio.nombre_peluqueria + " ha sido cancelada, lamentablemente el lugar no puede atenderle en ese horario, disculpe los incovenientes causados, puede reagendar otra cita en la fecha que usted desee.";
                    mailUser.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("mail5009.site4now.net");
                    //smtp.Host = "mail5009.site4now.net";
                    NetworkCredential networkCredential = new NetworkCredential("informaciones@vertebien.net", "Octubre0210*");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 8889;
                    smtp.EnableSsl = false;
                    smtp.Send(mailUser);
                    ViewBag.Message = "Enviado con exito";
                }
            }
            if (estatus == "completado")
            {
                var linkPuntuacionServicio = Url.Action("Create", "PUNTUACION_SERVICIOS", new {cITAS.id_cita}, protocol: Request.Url.Scheme);
                var linkPuntuacionPeluqueria = Url.Action("Create", "PUNTUACION_PELUQUERIA", new { cITAS.id_cita }, protocol: Request.Url.Scheme);
                cITAS.estado = "completado";
                //Correo para el usuario            
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                MailMessage mailUser = new MailMessage();
                mailUser.From = new MailAddress("informaciones@vertebien.net");
                mailUser.To.Add(emailUsuario.Email);
                mailUser.Subject = "Su cita ha sido completada";
                mailUser.Body = "Gracias por usar Verte Bien, su cita en " + usuarioNegocio.nombre_peluqueria + " ha sido completada. Por Favor cuentenos que tal su experiencia llenando este formulario. Para Puntuacion del Servicio: <a href=\"" + linkPuntuacionServicio + "\">Click Aqui</a> Para Puntuacion de la peluqueria: <a href=\"" + linkPuntuacionPeluqueria + "\">Click Aqui</a>";
                mailUser.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("mail5009.site4now.net");
                //smtp.Host = "mail5009.site4now.net";
                NetworkCredential networkCredential = new NetworkCredential("informaciones@vertebien.net", "Octubre0210*");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.Port = 8889;
                smtp.EnableSsl = false;
                smtp.Send(mailUser);
                ViewBag.Message = "Enviado con exito";

            }

            db.Entry(cITAS).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }

}

