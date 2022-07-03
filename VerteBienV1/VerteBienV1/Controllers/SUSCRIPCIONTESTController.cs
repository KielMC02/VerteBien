//using Microsoft.AspNetCore.Cors;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http.Cors;
//using System.Web.Mvc;
//using VerteBienV1.Models;
//using System.Web.Http;
//using System.Text.RegularExpressions;
//using Microsoft.AspNet.Identity;

//namespace VerteBienV1.Controllers
//{
//    [System.Web.Mvc.Authorize(Roles = "expres,preferencial,vip,administrador")]
//    public class SUSCRIPCIONController : Controller
//    {
//        private VERTEBIENEntities db = new VERTEBIENEntities();

//        // GET: SUSCRIPCION
//        public ActionResult Index()
//        {
//            var sUSCRIPCION = db.SUSCRIPCION.Include(s => s.AspNetUsers);
//            return View(sUSCRIPCION.ToList());
//        }
//        [System.Web.Mvc.Authorize(Roles = "administrador")]
//        // GET: SUSCRIPCION/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
//            if (sUSCRIPCION == null)
//            {
//                return HttpNotFound();
//            }
//            return View(sUSCRIPCION);
//        }



//        [System.Web.Http.HttpGet]
//        //Metodo Get Para la suscripcion Kushki
//        public ActionResult Subscribe(string email, string nombre, string apellido, string telefono, string membresiaSelec)
//        {
//            var resultado = "";
//            //Obtenemos el Id del usuario logueado.
//            List<AspNetUsers> idUser = new List<AspNetUsers>();
//            idUser = (from busqueda in db.AspNetUsers where busqueda.Email == email select busqueda).ToList();
//            if (idUser != null)
//            {
//                foreach (var item in idUser)
//                {
//                    resultado = item.Id;
//                }


//            }

//            ViewBag.membresiaSelec = membresiaSelec;
//            ViewBag.email = email;
//            ViewBag.nombre = nombre;
//            ViewBag.apellido = apellido;
//            ViewBag.telefono = telefono;
//            ViewBag.idUser = resultado;

//            return View();

//        }
//        [System.Web.Http.HttpPost]
//        [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
//        public async Task<ActionResult> SubscribeKushki(Data data, string membresiaSelec, string Email, string nombre, string apellido, string telefono, string idUser)
//        {

//            var fechaInicio = DateTime.Now.ToString("yyyy-MM-dd");
//            double precio = new double();
//            //Establecer el monto de la membresia
//            if (membresiaSelec == "expres")
//            {
//                precio = 8.00;
//            }
//            if (membresiaSelec == "preferencial")
//            {
//                precio = 14.00;
//            }
//            if (membresiaSelec == "vip")
//            {
//                precio = 16.00;
//            }

//            var info = new
//            {
//                token = data.token, // Reemplaza esto por el token que recibiste
//                planName = membresiaSelec,
//                periodicity = "monthly",
//                contactDetails = new { firstName = nombre, lastName = apellido, email = Email, phoneNumber = telefono },
//                amount = new { subtotalIva = 0, subtotalIva0 = precio, ice = 0, iva = 0, currency = "USD" },
//                startDate = Convert.ToString(fechaInicio), // Fecha del primer cobro
//                metadata = new { contractID = "158AB" }
//            };

//            HttpClient client = new HttpClient();
//            client.BaseAddress = new Uri("https://api-uat.kushkipagos.com/subscriptions/v1/card");
//            client.DefaultRequestHeaders
//                  .Accept
//                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

//            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api-uat.kushkipagos.com/subscriptions/v1/card");
//            request.Content = new StringContent(JsonConvert.SerializeObject(info),
//                                                Encoding.UTF8,
//                                                "application/json");//CONTENT-TYPE header

//            request.Content.Headers.Add("Private-Merchant-Id", "eafea94f7dec4d60a68d7c9183576dbc");
//            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            var result = await client.SendAsync(request);
//            var jsonPuro = "";
//            var codigoMovimientoEspecial = string.Empty;
//            if (result.IsSuccessStatusCode)
//            {
//                jsonPuro = await result.Content.ReadAsStringAsync();
//                var jsonDesarializado = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonPuro);

//                codigoMovimientoEspecial = jsonDesarializado.ToString();
//                codigoMovimientoEspecial = Regex.Replace(codigoMovimientoEspecial, "[{\\\r:n@\n\"}subscriptioId ]", string.Empty);

//                //Si el pago se hizo de forma correcta se realiza el pago de suscripcion
//                SUSCRIPCIONController nuevaSusripcion = new SUSCRIPCIONController();
//                SUSCRIPCION sUSCRIPCION = new SUSCRIPCION();
//                sUSCRIPCION.id_suscripcion_kushki = codigoMovimientoEspecial;
//                sUSCRIPCION.id_usuario = idUser;
//                sUSCRIPCION.estado = "activa";

//                nuevaSusripcion.Create(sUSCRIPCION);
//            }
//            else
//            {
//                return RedirectToAction("Register", "account");
//            }
//            return RedirectToAction("Login", "account");
//        }

//        //Metodo para pago fallido al momento de registrarse
//        public ActionResult PagoRequerido()
//        {


//            return View();
//        }

//        public ActionResult DatosParaPagos()
//        {
//            var idUsuario = User.Identity.GetUserId();
//            AspNetUsers usuario = db.AspNetUsers.Find(idUsuario);
//            if (User.IsInRole("expres"))
//            {
//                string email = usuario.Email;
//                string nombre = usuario.nombre;
//                string apellido = usuario.apellido;
//                string telefono = usuario.telefono;
//                string membresiaSelec = "expres";
//                return RedirectToAction("Subscribe", "SUSCRIPCION", new { email, nombre, apellido, telefono, membresiaSelec });
//            }
//            if (User.IsInRole("preferencial"))
//            {
//                string email = usuario.Email;
//                string nombre = usuario.nombre;
//                string apellido = usuario.apellido;
//                string telefono = usuario.telefono;
//                string membresiaSelec = "preferencial";
//                return RedirectToAction("Subscribe", "SUSCRIPCION", new { email, nombre, apellido, telefono, membresiaSelec });
//            }
//            if (User.IsInRole("vip"))
//            {
//                string email = usuario.Email;
//                string nombre = usuario.nombre;
//                string apellido = usuario.apellido;
//                string telefono = usuario.telefono;
//                string membresiaSelec = "vip";
//                return RedirectToAction("Subscribe", "SUSCRIPCION", new { email, nombre, apellido, telefono, membresiaSelec });
//            }

//            return View();
//        }

//        // GET: SUSCRIPCION/Create
//        public ActionResult Create()
//        {
//            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
//            return View();
//        }

//        // POST: SUSCRIPCION/Create
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
//        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
//        [System.Web.Mvc.HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "id_suscripcion,id_usuario,estado,id_suscripcion_kushki")] SUSCRIPCION sUSCRIPCION)
//        {
//            //Obtenemos el usuario que acaba de pagar
//            AspNetUsers Usuario = db.AspNetUsers.Find(sUSCRIPCION.id_usuario);
//            //Actualizamos su estado como usuario.
//            Usuario.estado = "Pago";
//            ///Guardamos
//            db.SaveChanges();


//            if (ModelState.IsValid)
//            {
//                db.SUSCRIPCION.Add(sUSCRIPCION);
//                db.SaveChanges();
//                return RedirectToAction("Index", "servicios");
//            }

//            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
//            return View(sUSCRIPCION);
//        }
//        [System.Web.Mvc.Authorize(Roles = "administrador")]
//        // GET: SUSCRIPCION/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
//            if (sUSCRIPCION == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
//            return View(sUSCRIPCION);
//        }

//        // POST: SUSCRIPCION/Edit/5
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
//        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
//        [System.Web.Mvc.HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "id_suscripcion,id_usuario,estado,id_suscripcion_kushki")] SUSCRIPCION sUSCRIPCION)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(sUSCRIPCION).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
//            return View(sUSCRIPCION);
//        }
//        [System.Web.Mvc.Authorize(Roles = "administrador")]
//        // GET: SUSCRIPCION/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
//            if (sUSCRIPCION == null)
//            {
//                return HttpNotFound();
//            }
//            return View(sUSCRIPCION);
//        }

//        // POST: SUSCRIPCION/Delete/5
//        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
//            db.SUSCRIPCION.Remove(sUSCRIPCION);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
