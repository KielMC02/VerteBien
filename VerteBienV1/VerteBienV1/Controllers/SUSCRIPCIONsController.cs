using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using VerteBienV1.Models;

namespace VerteBienV1.Controllers
{
    public class SUSCRIPCIONsController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();
        //Clase que recibira la respuesta del SaveCARD
        public class CardResponse
        {
            public String status { get; set; }
            public String token { get; set; }
            public String transaction_reference { get; set; }
            public String IdUser { get; set; }
            public string Membresia { get; set; }
            public String Email { get; set; }
            public String number { get; set; }
            public String expiry_month { get; set; }
            public String expiry_year { get; set; }
            public String message { get; set; }

        }
        [Authorize(Roles = "administrador")]
        // GET: SUSCRIPCIONs
        public ActionResult Index()
        {
            var sUSCRIPCION = db.SUSCRIPCION.Include(s => s.AspNetUsers);
            return View(sUSCRIPCION.ToList());
        }
        [Authorize(Roles = "administrador")]
        // GET: SUSCRIPCIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            if (sUSCRIPCION == null)
            {
                return HttpNotFound();
            }
            return View(sUSCRIPCION);
        }

        // GET: SUSCRIPCIONs/Create
        public ActionResult Create()
        {

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: SUSCRIPCIONs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_suscripcion,id_usuario,estado,id_suscripcion_kushki,comentario,fecha_suscripcion")] SUSCRIPCION sUSCRIPCION)
        {
            if (ModelState.IsValid)
            {
                db.SUSCRIPCION.Add(sUSCRIPCION);
                var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstado @id", new SqlParameter("@id", Convert.ToString(sUSCRIPCION.id_usuario)));

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
            return View(sUSCRIPCION);
        }

        // GET: SUSCRIPCIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            if (sUSCRIPCION == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
            return View(sUSCRIPCION);
        }

        // POST: SUSCRIPCIONs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_suscripcion,id_usuario,estado,id_suscripcion_kushki,comentario,fecha_suscripcion")] SUSCRIPCION sUSCRIPCION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sUSCRIPCION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
            return View(sUSCRIPCION);
        }

        // GET: SUSCRIPCIONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            if (sUSCRIPCION == null)
            {
                return HttpNotFound();
            }
            return View(sUSCRIPCION);
        }

        // POST: SUSCRIPCIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            db.SUSCRIPCION.Remove(sUSCRIPCION);
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

        //Recordatorio para mitad de mes
        public ActionResult RecordatorioMitad()
        {
            var today = DateTime.Today;
            var mesactual = new DateTime(today.Year, today.Month, 1);
            var mesanterior = mesactual.AddMonths(-1);
            var quincena = mesanterior.AddDays(14);
            AspNetUsers usuario = new AspNetUsers();
            List<SUSCRIPCION> suscripciones = new List<SUSCRIPCION>();
            suscripciones = (from busqueda in db.SUSCRIPCION where busqueda.fecha_suscripcion == quincena select busqueda).ToList();
            if (suscripciones.Count > 0)
            {

                foreach (var item in suscripciones)
                {
                    usuario = db.AspNetUsers.Find(item.id_usuario);
                    string from = "kielcuentas@gmail.com";
                    MailMessage mailUser = new MailMessage(from, usuario.Email);
                    {

                        mailUser.Subject = "Recordatorio de Pago";
                        mailUser.Body = "Hola " + usuario.nombre + " te recordamos que el 15 del mes en curso debes renovar tu suscripcion Verte Bien.";
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

            }

            return RedirectToAction("Index", "AspNetUsers");
        }

        //Recordatorio para mitad de mes
        public  ActionResult RecordatorioFin()
        {
            ViewBag.notificacion = 4;
            var today = DateTime.Today;
            var mesactual = new DateTime(today.Year, today.Month, 1);
            //var mesanterior = mesactual.AddMonths(-1);
            var finMes = mesactual.AddDays(-1);
            AspNetUsers usuario = new AspNetUsers();
            List<SUSCRIPCION> suscripciones = new List<SUSCRIPCION>();
            suscripciones = (from busqueda in db.SUSCRIPCION where busqueda.fecha_suscripcion == finMes select busqueda).ToList();
            if (suscripciones.Count > 0)
            {

                foreach (var item in suscripciones)
                {
                    usuario = db.AspNetUsers.Find(item.id_usuario);
                    string from = "kielcuentas@gmail.com";
                    MailMessage mailUser = new MailMessage(from, usuario.Email);
                    {

                        mailUser.Subject = "Recordatorio de Pago";
                        mailUser.Body = "Hola " + usuario.nombre + " te recordamos que el " + Convert.ToString(finMes.Day) + " del mes en curso debes renovar tu suscripcion Verte Bien.";
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
                ViewBag.notificacion = 5;
                return RedirectToAction("Index", "AspNetUsers");
            }

            return RedirectToAction("Index", "AspNetUsers");
        }
        //Redireccion de usuarios con estatus suspendido o new
        public ActionResult pagoRequerido(string respuesta) 
        {
            if(respuesta != null)
            {
                ViewBag.respuesta = respuesta;
            }
            return View();
        }

        //Metodo para debitar en masa a todos los usuarios.
        public async Task<String> debitarUsers()
        {
            List<AspNetUsers> listaUsers = new List<AspNetUsers>();
            CardResponse infoParaCobrar = new CardResponse();
            listaUsers = (from buscarUsers in db.AspNetUsers where buscarUsers.fecha_creacion_.Day == DateTime.Today.Day select buscarUsers).ToList();
            if (listaUsers.Count > 0)
            {
                foreach (var item in listaUsers)
                {
                    if (item.nombre_peluqueria != null && item.estado == "activo")
                    {
                        var resultadoBusqueda = db.Database.SqlQuery<string>("SP_SelectMembresia @id", new SqlParameter("@id", item.Id)).ToList();
                        var token = (from buscaToken in db.CARD where buscaToken.id_usuario == item.Id && buscaToken.comentario == "activo" select buscaToken.token).ToList();
                        infoParaCobrar.token = Convert.ToString(token[0]);
                        infoParaCobrar.IdUser = item.Id;
                        infoParaCobrar.Email = item.Email;
                        if (resultadoBusqueda[0] == "2")
                        {
                            infoParaCobrar.Membresia = "expres";

                        }
                        if (resultadoBusqueda[0] == "4")
                        {
                            infoParaCobrar.Membresia = "preferencial";
                        }
                        if (resultadoBusqueda[0] == "5")
                        {
                            infoParaCobrar.Membresia = "vip";
                        }
                            
                        var respuestaDebit = await debit(infoParaCobrar);
                        if (respuestaDebit != "transaccion_fallida")
                        {
                            //Evaluamos la respuesta obtenida---
                            //Limpiamos el contenido de la repsuesta y almacenamos en una lista
                            respuestaDebit = Regex.Replace(respuestaDebit, "[:\\{}@\n\"]", string.Empty);
                            List<String> contenidoRespuesta = (respuestaDebit.Split(',')).ToList();
                            if (contenidoRespuesta[0].Contains("success"))
                            {
                                //instancia nueva suscripcion
                                SUSCRIPCION newSuscripcion = new SUSCRIPCION();
                                newSuscripcion.id_usuario = infoParaCobrar.IdUser;
                                newSuscripcion.estado = "OK";
                                newSuscripcion.fecha_suscripcion = DateTime.Today;
                                newSuscripcion.trasaction_reference = contenidoRespuesta[20];
                                newSuscripcion.comentario = Convert.ToString(contenidoRespuesta);

                                SUSCRIPCIONsController insertarSuscripcion = new SUSCRIPCIONsController();
                                insertarSuscripcion.Create(newSuscripcion);
                            }
                            else
                            {
                                var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstadoSuspendido @id", new SqlParameter("@id", Convert.ToString(infoParaCobrar.IdUser)));
                            }
                        }
                        if (respuestaDebit == "transaccion_fallida")
                        {
                            var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstadoSuspendido @id", new SqlParameter("@id", Convert.ToString(infoParaCobrar.IdUser)));
                        }
                    }

                }
            }
            return "Debito realizado";

        }
        //Debitar usuarios pendientes de pago.
        public async Task<String> debitarUsersPendientes()
        {
            List<AspNetUsers> listaUsers = new List<AspNetUsers>();
            CardResponse infoParaCobrar = new CardResponse();
            listaUsers = (from buscarUsers in db.AspNetUsers where buscarUsers.estado == "suspendido" select buscarUsers).ToList();
            if (listaUsers.Count > 0)
            {
                foreach (var item in listaUsers)
                {
                    if (item.nombre_peluqueria != null && item.estado == "activo")
                    {
                        var resultadoBusqueda = db.Database.SqlQuery<string>("SP_SelectMembresia @id", new SqlParameter("@id", item.Id)).ToList();
                        var token = (from buscaToken in db.CARD where buscaToken.id_usuario == item.Id && buscaToken.comentario == "activo" select buscaToken.token).ToList();
                        infoParaCobrar.token = Convert.ToString(token[0]);
                        infoParaCobrar.IdUser = item.Id;
                        infoParaCobrar.Email = item.Email;
                        if (resultadoBusqueda[0] == "2")
                        {
                            infoParaCobrar.Membresia = "expres";

                        }
                        if (resultadoBusqueda[0] == "4")
                        {
                            infoParaCobrar.Membresia = "preferencial";
                        }
                        if (resultadoBusqueda[0] == "5")
                        {
                            infoParaCobrar.Membresia = "vip";
                        }

                        var respuestaDebit = await debit(infoParaCobrar);
                        if (respuestaDebit != "transaccion_fallida")
                        {
                            //Evaluamos la respuesta obtenida---
                            //Limpiamos el contenido de la repsuesta y almacenamos en una lista
                            respuestaDebit = Regex.Replace(respuestaDebit, "[:\\{}@\n\"]", string.Empty);
                            List<String> contenidoRespuesta = (respuestaDebit.Split(',')).ToList();
                            if (contenidoRespuesta[0].Contains("success"))
                            {
                                //instancia nueva suscripcion
                                SUSCRIPCION newSuscripcion = new SUSCRIPCION();
                                newSuscripcion.id_usuario = infoParaCobrar.IdUser;
                                newSuscripcion.estado = "OK";
                                newSuscripcion.fecha_suscripcion = DateTime.Today;
                                newSuscripcion.trasaction_reference = contenidoRespuesta[20];
                                newSuscripcion.comentario = Convert.ToString(contenidoRespuesta);

                                SUSCRIPCIONsController insertarSuscripcion = new SUSCRIPCIONsController();
                                insertarSuscripcion.Create(newSuscripcion);
                            }
                            else
                            {
                                var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstadoSuspendido @id", new SqlParameter("@id", Convert.ToString(infoParaCobrar.IdUser)));
                            }
                        }
                        if (respuestaDebit == "transaccion_fallida")
                        {
                            var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstadoSuspendido @id", new SqlParameter("@id", Convert.ToString(infoParaCobrar.IdUser)));
                        }
                    }

                }
            }
            return "Debito realizado";

        }
        //Auto-Debitar por el usuario
        public async Task<String> debitarManualUser( )
        {
            var estaAutenticado = User.Identity.IsAuthenticated;
            var id = "";
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
                CardResponse infoParaCobrar = new CardResponse();
                AspNetUsers usuario = db.AspNetUsers.Find(id);
                var resultadoBusqueda = db.Database.SqlQuery<string>("SP_SelectMembresia @id", new SqlParameter("@id", id)).ToList();
                var token = (from buscaToken in db.CARD where buscaToken.id_usuario == id && buscaToken.comentario == "activo" select buscaToken.token).ToList();
                infoParaCobrar.token = Convert.ToString(token[0]);
                infoParaCobrar.IdUser = id;
                infoParaCobrar.Email = usuario.Email;
                if (resultadoBusqueda[0] == "2")
                {
                    infoParaCobrar.Membresia = "expres";

                }
                if (resultadoBusqueda[0] == "4")
                {
                    infoParaCobrar.Membresia = "preferencial";
                }
                if (resultadoBusqueda[0] == "5")
                {
                    infoParaCobrar.Membresia = "vip";
                }

                var respuestaDebit = await debit(infoParaCobrar);
                if (respuestaDebit != "transaccion_fallida")
                {
                    //Evaluamos la respuesta obtenida---
                    //Limpiamos el contenido de la repsuesta y almacenamos en una lista
                    respuestaDebit = Regex.Replace(respuestaDebit, "[:\\{}@\n\"]", string.Empty);
                    List<String> contenidoRespuesta = (respuestaDebit.Split(',')).ToList();
                    if (contenidoRespuesta[0].Contains("success"))
                    {
                        //instancia nueva suscripcion
                        SUSCRIPCION newSuscripcion = new SUSCRIPCION();
                        newSuscripcion.id_usuario = infoParaCobrar.IdUser;
                        newSuscripcion.estado = "OK";
                        newSuscripcion.fecha_suscripcion = DateTime.Today;
                        newSuscripcion.trasaction_reference = contenidoRespuesta[20];
                        newSuscripcion.comentario = Convert.ToString(contenidoRespuesta);

                        SUSCRIPCIONsController insertarSuscripcion = new SUSCRIPCIONsController();
                        insertarSuscripcion.Create(newSuscripcion);
                        return "Debito realizado";
                    }
                    else
                    {
                        var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstadoSuspendido @id", new SqlParameter("@id", Convert.ToString(infoParaCobrar.IdUser)));
                    }
                }
                if (respuestaDebit == "transaccion_fallida")
                {
                    var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstadoSuspendido @id", new SqlParameter("@id", Convert.ToString(infoParaCobrar.IdUser)));
                }
            }
            return "Algo ha fallado";
        }

        //------------------------METODOS DE PAYMENTEZ-----------------
        [System.Web.Http.HttpPost]
        [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<ActionResult> CanalAsync(CardResponse respuesta) 
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(respuesta.IdUser);
            if ( aspNetUsers.estado == "new") 
            {
                //Esperamos respuesta del metodo debitar
               var respuestaDebit = await debit(respuesta);
                //Si es diferente de...
                if( respuestaDebit != "transaccion_fallida")
                {
                        //Evaluamos la respuesta obtenida---
                        //Limpiamos el contenido de la repsuesta y almacenamos en una lista
                    respuestaDebit = Regex.Replace(respuestaDebit, "[:\\{}@\n\"]", string.Empty);
                    List<String> contenidoRespuesta = (respuestaDebit.Split(',')).ToList();
                    if (contenidoRespuesta[0].Contains("success"))
                    {
                        //Instancia de una nueva tarjeta
                        CARD nuevaTarjeta = new CARD();
                        nuevaTarjeta.id_usuario = respuesta.IdUser;
                        nuevaTarjeta.estatus = respuesta.status;
                        nuevaTarjeta.token = respuesta.token;
                        nuevaTarjeta.trasaction_reference = contenidoRespuesta[20];
                        nuevaTarjeta.digitos = respuesta.number;
                        nuevaTarjeta.fecha_expiracion = respuesta.expiry_month + "/" + respuesta.expiry_year;
                        nuevaTarjeta.fecha_agregada = DateTime.Today;
                        nuevaTarjeta.comentario = "activo";

                        CARDController insertarTarjeta = new CARDController();

                        insertarTarjeta.Create(nuevaTarjeta);

                        return RedirectToAction("Index", "SERVICIOS");
                    }
                }
            }

            if (aspNetUsers.estado == "activo")
            {

                //Instancia de una nueva tarjeta
                CARD nuevaTarjeta = new CARD();
                nuevaTarjeta.id_usuario = respuesta.IdUser;
                nuevaTarjeta.estatus = respuesta.status;
                nuevaTarjeta.token = respuesta.token;
                nuevaTarjeta.trasaction_reference = respuesta.transaction_reference;
                nuevaTarjeta.digitos = respuesta.number;
                nuevaTarjeta.fecha_expiracion = respuesta.expiry_month + "/" + respuesta.expiry_year;
                nuevaTarjeta.fecha_agregada = DateTime.Today;
                nuevaTarjeta.comentario = "activo";

                CARDController insertarTarjeta = new CARDController();
                insertarTarjeta.Create(nuevaTarjeta);

                var resultadoProc = db.Database.ExecuteSqlCommand("SP_NuevaTarjetaDefault @id, @digitos", new SqlParameter("@id", Convert.ToString(respuesta.IdUser)), new SqlParameter("@id", Convert.ToString(respuesta.number)));

                return RedirectToAction("index", "SERVICIOS");
            }

            if (aspNetUsers.estado == "suspendido")
            {
                //Esperamos respuesta del metodo debitar
                var respuestaDebit = await debit(respuesta);
                //Si es diferente de...
                if (respuestaDebit != "transaccion_fallida")
                {
                    //Evaluamos la respuesta obtenida---
                    //Limpiamos el contenido de la repsuesta y almacenamos en una lista
                    respuestaDebit = Regex.Replace(respuestaDebit, "[:\\{}@\n\"]", string.Empty);
                    List<String> contenidoRespuesta = (respuestaDebit.Split(',')).ToList();
                    if (contenidoRespuesta[0].Contains("success"))
                    {
                        //Instancia de una nueva tarjeta
                        CARD nuevaTarjeta = new CARD();
                        nuevaTarjeta.id_usuario = respuesta.IdUser;
                        nuevaTarjeta.estatus = respuesta.status;
                        nuevaTarjeta.token = respuesta.token;
                        nuevaTarjeta.trasaction_reference = contenidoRespuesta[20];
                        nuevaTarjeta.digitos = respuesta.number;
                        nuevaTarjeta.fecha_expiracion = respuesta.expiry_month + "/" + respuesta.expiry_year;
                        nuevaTarjeta.fecha_agregada = DateTime.Today;
                        nuevaTarjeta.comentario = "activo";

                        CARDController insertarTarjeta = new CARDController();

                        insertarTarjeta.Create(nuevaTarjeta);
                        var resultadoProc = db.Database.ExecuteSqlCommand("SP_NuevaTarjetaDefault @id, @digitos", new SqlParameter("@id", Convert.ToString(respuesta.IdUser)), new SqlParameter("@id", Convert.ToString(respuesta.number)));

                        return RedirectToAction("Index", "SERVICIOS");
                    }
                }

                
            }
            return RedirectToAction("index", "SERVICIOS");
        }
        public ActionResult SaveCard(string Id, string Email, string membresiaSelec) 
        {
            if(Id != null && Email != null && membresiaSelec != null) 
            { 
                    ViewBag.idUser = Id;
                    ViewBag.Email = Email;
                    ViewBag.membresia = membresiaSelec;
            }
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                var idUser = User.Identity.GetUserId();
                AspNetUsers aspNetUsers = db.AspNetUsers.Find(idUser);
                if (aspNetUsers.estado == "activo")
                {
                    ViewBag.idUser = aspNetUsers.Id;
                    ViewBag.Email = aspNetUsers.Email;
                    ViewBag.membresia = "n/a";
                }
            }



                //String respuesta = "{\"transaction\": {\"status\": \"success\", \"authorization_code\": \"TEST00\", \"status_detail\": 3,\"message\": \"Response by mock\", \"id\": \"DF-243601\", \"payment_date\": \"2022-09-25T19:23:55.644\", \"payment_method_type\": \"0\", \"dev_reference\": \"debit\",\"carrier_code\": \"00\",\"product_description\": \"pago servicio Verte Bien\", \"current_status\": \"APPROVED\", \"amount\": 10.0,\"carrier\": \"DataFast\",\"installments\": 0,\"installments_type\": \"Revolving credit\"},\"card\": {\"bin\": \"450799\", \"status\": \"valid\", \"token\": \"5762035270905976501\", \"expiry_year\": \"2025\",\"expiry_month\": \"11\",\"transaction_reference\": \"DF-243601\",\"type\": \"vi\", \"number\": \"0010\",\"origin\": \"Paymentez\"}}";

                //respuesta = Regex.Replace(respuesta, "[:\\{}@\n\"]", string.Empty);
                //Console.WriteLine(respuesta);

                //List<String> Contenido = (respuesta.Split(',')).ToList();

                return View();

        }

        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[HttpPost]
        public async Task<string> deleteCard(Data data)
        {
            var info = new
            {
                user = new { id = "uid123456" },
                card = new { token = "16126824737099654930" }
            };
            var token = await getToken();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ccapi-stg.paymentez.com/v2/card/delete/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://ccapi-stg.paymentez.com/v2/card/delete/");
            request.Content = new StringContent(JsonConvert.SerializeObject(info),
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            request.Content.Headers.Add("Auth-Token", token);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString != null)
            {
                return responseString.ToString();
            }
            else
            {
                return "transaccion_fallida";
            }

        }

        public async Task<string> debit(CardResponse respuesta)
        {
            var obtainedToken = await getToken();
            double monto = 0;
            if (respuesta.Membresia == "expres")
            {
                monto = 8.00;
            }
            if (respuesta.Membresia == "preferencial")
            {
                monto = 20.00;
            }
            if (respuesta.Membresia == "vip")
            {
                monto = 40.00;
            }
            var info = new
            {
                user = new { id = respuesta.IdUser, email = respuesta.Email },
                order = new { amount = monto, description = "Pago servicio Verte Bien", dev_reference = "debit", tax_percentage = 0, vat = 0 },
                card = new { token = respuesta.token }
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ccapi-stg.paymentez.com/v2/transaction/debit/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://ccapi-stg.paymentez.com/v2/transaction/debit/");
            request.Content = new StringContent(JsonConvert.SerializeObject(info),
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            request.Content.Headers.Add("Auth-Token", obtainedToken);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString != null)
            {
                return responseString.ToString();
            }
            else
            {
                return "transaccion_fallida";
            }

        }

        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[HttpPost]
        public async Task<string> refund(Data data)
        {
            var info = new
            {
                transaction = new { id = "DF-214117" }
            };
            var token = await getToken();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ccapi-stg.paymentez.com/v2/transaction/refund/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://ccapi-stg.paymentez.com/v2/transaction/refund/");
            request.Content = new StringContent(JsonConvert.SerializeObject(info),
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            request.Content.Headers.Add("Auth-Token", token);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString != null)
            {
                return responseString.ToString();
            }
            else
            {
                return "oh oh esto esta nulo";
            }

        }

        public async Task<string> verifyOTP(Data data)
        {
            var info = new
            {
                user = new { id = "uid123456" },
                transaction = new { id = "DF-214117" },
                type = "BY_AMOUNT",
                value = "831283"
            };
            var token = await getToken();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ccapi-stg.paymentez.com/v2/transaction/verify");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://ccapi-stg.paymentez.com/v2/transaction/verify");
            request.Content = new StringContent(JsonConvert.SerializeObject(info),
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            request.Content.Headers.Add("Auth-Token", token);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString != null)
            {
                return responseString.ToString();
            }
            else
            {
                return "oh oh esto esta nulo";
            }

        }

        public async Task<string> getToken()
        {
            var API_LOGIN_DEV = "NUVEISTG-EC-SERVER";
            var API_KEY_DEV = "Kn9v6ICvoRXQozQG2rK92WtjG6l08a";

            var server_application_code = API_LOGIN_DEV;
            var server_app_key = API_KEY_DEV;
            var unix_timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            var uniq_token_string = server_app_key + unix_timestamp;
            var uniq_token_hash = sha256(uniq_token_string);
            var auth_token = Base64Encode(server_application_code + ";" + unix_timestamp + ";" + uniq_token_hash);
            return auth_token;
        }

        static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

    }
}
