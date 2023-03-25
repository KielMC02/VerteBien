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
        public ActionResult Details(string id, int? idServicio)
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

            List<REDES_SOCIALES> redes = new List<REDES_SOCIALES>();
            redes = (from busqueda in db.REDES_SOCIALES where busqueda.id_usuario == id select busqueda).ToList();
            ViewBag.whatsapp = "";
            ViewBag.instagram = "";
            ViewBag.facebook = "";
            ViewBag.web_app="";
            if (redes.Count != 0)
            {
                foreach(var item in redes)
                {
                    ViewBag.whatsapp = item.whatsapp;
                    ViewBag.instagram = item.instagram;
                    ViewBag.facebook = item.facebook;
                    ViewBag.web_app = item.web_app;

                }

            }
            ViewBag.idServicio = idServicio;
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
                List<string> cantones = new List<string>();
                cantones.Add("24 de Mayo");
                cantones.Add("Aguarico");
                cantones.Add("Alausí");
                cantones.Add("Alfredo Baquerizo");
                cantones.Add("Ambato");
                cantones.Add("Antonio Ante");
                cantones.Add("Arajuno");
                cantones.Add("Archidona");
                cantones.Add("Arenillas");
                cantones.Add("Atacames");
                cantones.Add("Atahualpa");
                cantones.Add("Azogues");
                cantones.Add("Baba");
                cantones.Add("Babahoyo");
                cantones.Add("Balao");
                cantones.Add("Balsas");
                cantones.Add("Balzar");
                cantones.Add("Baños");
                cantones.Add("Biblián");
                cantones.Add("Bolívar");
                cantones.Add("Buena Fe");
                cantones.Add("Caluma");
                cantones.Add("Calvas");
                cantones.Add("Camilo Ponce Enríquez");
                cantones.Add("Cañar");
                cantones.Add("Carlos Julio Arosemena Tola");
                cantones.Add("Cascales");
                cantones.Add("Catamayo");
                cantones.Add("Cayambe");
                cantones.Add("Celica");
                cantones.Add("Centinela del Cóndor");
                cantones.Add("Cevallos");
                cantones.Add("Chaguarpamba");
                cantones.Add("Chambo");
                cantones.Add("Chilla");
                cantones.Add("Chillanes");
                cantones.Add("Chimbo");
                cantones.Add("Chinchipe");
                cantones.Add("Chone");
                cantones.Add("Chordeleg");
                cantones.Add("Chunchi");
                cantones.Add("Colimes");
                cantones.Add("Colta");
                cantones.Add("Cotacachi");
                cantones.Add("Cuenca");
                cantones.Add("Cumandá");
                cantones.Add("Cuyabeno");
                cantones.Add("Daule");
                cantones.Add("Déleg");
                cantones.Add("Durán");
                cantones.Add("Echeandía");
                cantones.Add("El Carmen");
                cantones.Add("El Chaco");
                cantones.Add("El Empalme");
                cantones.Add("El Guabo");
                cantones.Add("El Pan");
                cantones.Add("El Pangui");
                cantones.Add("El Tambo");
                cantones.Add("El Triunfo");
                cantones.Add("Eloy Alfaro");
                cantones.Add("Esmeraldas");
                cantones.Add("Espejo");
                cantones.Add("Espíndola");
                cantones.Add("Flavio Alfaro");
                cantones.Add("Francisco de Orellana");
                cantones.Add("General Antonio Elizale");
                cantones.Add("Girón");
                cantones.Add("Gonzalo Pizarro");
                cantones.Add("Gonzanamá");
                cantones.Add("Guachapala");
                cantones.Add("Gualaceo");
                cantones.Add("Gualaquiza");
                cantones.Add("Guamote");
                cantones.Add("Guano");
                cantones.Add("Guaranda");
                cantones.Add("Guayaquil");
                cantones.Add("Huaca");
                cantones.Add("Huamboya");
                cantones.Add("Huaquillas");
                cantones.Add("Ibarra");
                cantones.Add("Isabela");
                cantones.Add("Isidro Ayora");
                cantones.Add("Jama");
                cantones.Add("Jaramijó");
                cantones.Add("Jipijapa");
                cantones.Add("Junín");
                cantones.Add("La Concordia");
                cantones.Add("La Joya de los Sachas");
                cantones.Add("La Libertad");
                cantones.Add("La Maná");
                cantones.Add("La Troncal");
                cantones.Add("Lago Agrio");
                cantones.Add("Las Lajas");
                cantones.Add("Las Naves");
                cantones.Add("Latacunga");
                cantones.Add("Limón Indanza");
                cantones.Add("Logroño");
                cantones.Add("Loja");
                cantones.Add("Lomas de Sargentillo");
                cantones.Add("Loreto");
                cantones.Add("Macará");
                cantones.Add("Machala");
                cantones.Add("Manta");
                cantones.Add("Marcabelí");
                cantones.Add("Marcelino Maridueña");
                cantones.Add("Mejía");
                cantones.Add("Milagro");
                cantones.Add("Mocacho");
                cantones.Add("Mocha");
                cantones.Add("Montalvo");
                cantones.Add("Montecristi");
                cantones.Add("Montúfar");
                cantones.Add("Morona");
                cantones.Add("Muisne");
                cantones.Add("Nabón");
                cantones.Add("Nangaritza");
                cantones.Add("Naranjal");
                cantones.Add("Naranjito");
                cantones.Add("Nobol");
                cantones.Add("Olmedo");
                cantones.Add("Oña");
                cantones.Add("Otavalo");
                cantones.Add("Pablo Sexto");
                cantones.Add("Paján");
                cantones.Add("Palenque");
                cantones.Add("Palestina");
                cantones.Add("Pallatanga");
                cantones.Add("Palora");
                cantones.Add("Paltas");
                cantones.Add("Pangua");
                cantones.Add("Paquisha");
                cantones.Add("Pasaje");
                cantones.Add("Pastaza");
                cantones.Add("Patate");
                cantones.Add("Paute");
                cantones.Add("Pedernales");
                cantones.Add("Pedro Carbo");
                cantones.Add("Pedro Moncayo");
                cantones.Add("Pedro Vicente Maldonado");
                cantones.Add("Penipe");
                cantones.Add("Pichincha");
                cantones.Add("Pimampiro");
                cantones.Add("Pindal");
                cantones.Add("Piñas");
                cantones.Add("Playas");
                cantones.Add("Portovelo");
                cantones.Add("Portoviejo");
                cantones.Add("Pucará");
                cantones.Add("Puebloviejo");
                cantones.Add("Puerto López");
                cantones.Add("Puerto Quito");
                cantones.Add("Pujilí");
                cantones.Add("Putumayo");
                cantones.Add("Puyango");
                cantones.Add("Quero");
                cantones.Add("Quevedo");
                cantones.Add("Quijos");
                cantones.Add("Quilanga");
                cantones.Add("Quinindé");
                cantones.Add("Quinsaloma");
                cantones.Add("Quito");
                cantones.Add("Riobamba");
                cantones.Add("Rioverde");
                cantones.Add("Rocafuerte");
                cantones.Add("Rumiñahui");
                cantones.Add("Salcedo");
                cantones.Add("Salinas");
                cantones.Add("Salitre");
                cantones.Add("Samborondón");
                cantones.Add("San Cristóbal");
                cantones.Add("San Fernando");
                cantones.Add("San Juan Bosco");
                cantones.Add("San Lorenzo");
                cantones.Add("San Miguel");
                cantones.Add("San Miguel de Urcuquí");
                cantones.Add("San Miguel de los Bancos");
                cantones.Add("San Pedro de Pelileo");
                cantones.Add("San Vicente");
                cantones.Add("Santa Ana");
                cantones.Add("Santa Clara");
                cantones.Add("Santa Cruz");
                cantones.Add("Santa Elena");
                cantones.Add("Santa Isabel");
                cantones.Add("Santa Lucía");
                cantones.Add("Santa Rosa");
                cantones.Add("Santiago de Méndez");
                cantones.Add("Santiago de Píllaro");
                cantones.Add("Santo Domingo");
                cantones.Add("Saquisilí");
                cantones.Add("Saraguro");
                cantones.Add("Sevilla de Oro");
                cantones.Add("Shushufindi");
                cantones.Add("Sigchos");
                cantones.Add("Sígsig");
                cantones.Add("Simón Bolívar");
                cantones.Add("Sozoranga");
                cantones.Add("Sucre");
                cantones.Add("Sucúa");
                cantones.Add("Sucumbíos");
                cantones.Add("Suscal");
                cantones.Add("Taisha");
                cantones.Add("Tena");
                cantones.Add("Tisaleo");
                cantones.Add("Tiwintza");
                cantones.Add("Tosagua");
                cantones.Add("Tulcán");
                cantones.Add("Urdaneta");
                cantones.Add("Valencia");
                cantones.Add("Ventanas");
                cantones.Add("Vinces");
                cantones.Add("Yacuambi");
                cantones.Add("Yaguachi");
                cantones.Add("Yantzaza");
                cantones.Add("Zamora");
                cantones.Add("Zapotillo");
                cantones.Add("Zaruma");
                #endregion

                List<string> sectores = new List<string>();
                #region
                sectores.Add("Cumbe");
                sectores.Add("El Carmen de Pijilí");
                sectores.Add("Principal");
                sectores.Add("San Vicente");
                sectores.Add("Asunción");
                sectores.Add("La Cabecera");
                sectores.Add("Daniel Córdova Toral");
                sectores.Add("Cochapata");
                sectores.Add("Susudel");
                sectores.Add("Bulán");
                sectores.Add("San Rafael de Sharug");
                sectores.Add("(Chumblín");
                sectores.Add("Abdón Calderón");
                sectores.Add("Amaluza");
                sectores.Add("Cuchil");
                sectores.Add("Guaranda son Facundo Vela");
                sectores.Add("Caluma");
                sectores.Add("San José del Tambo");
                sectores.Add("Asunción");
                sectores.Add("Echeandía");
                sectores.Add("Mercedes");
                sectores.Add("Balsapamba");
                sectores.Add("Cojitambo");
                sectores.Add("Jerusalén");
                sectores.Add("Chontamarca");
                sectores.Add("Solano");
                sectores.Add("El Tambo");
                sectores.Add("J. Calle");
                sectores.Add("Suscal");
                sectores.Add("El Carmelo");
                sectores.Add("García Moreno");
                sectores.Add("El Goaltal");
                sectores.Add("Concepción");
                sectores.Add("Cristóbal Colón");
                sectores.Add("Mariscal Sucre");
                sectores.Add("Calpi");
                sectores.Add("Achupallas");
                sectores.Add("Chambo");
                sectores.Add("Capzol");
                sectores.Add("Columbe");
                sectores.Add("Cumandá");
                sectores.Add("Cebadas");
                sectores.Add("Guanando");
                sectores.Add("(Pallatanga");
                sectores.Add("Bilbao");
                sectores.Add("11 de Noviembre");
                sectores.Add("Guasaganda");
                sectores.Add("Moraspungo");
                sectores.Add("Angamarca");
                sectores.Add("Antonio José Holguín");
                sectores.Add("Chantilín");
                sectores.Add("Chugchillán");
                sectores.Add("El Cambio");
                sectores.Add("Chilla");
                sectores.Add("El Guabo");
                sectores.Add("El Paraíso");
                sectores.Add("La Victoria");
                sectores.Add("El Ingenio");
                sectores.Add("Buenavista");
                sectores.Add("Capiro");
                sectores.Add("Curtincapa");
                sectores.Add("Bellamaría");
                sectores.Add("Camarones");
                sectores.Add("La Unión");
                sectores.Add("Anchayacu");
                sectores.Add("Bolívar");
                sectores.Add("Cube");
                sectores.Add("Chontaduro");
                sectores.Add("Alto Tambo");
                sectores.Add("Puerto Baquerizo Moreno");
                sectores.Add("Tomás De Berlanga");
                sectores.Add("Puerto Ayora");
                sectores.Add("Rocafuerte");
                sectores.Add("Juan Gómez Rendón");
                sectores.Add("Alfredo Baquerizo Moreno");
                sectores.Add("Balao");
                sectores.Add("Balzar");
                sectores.Add("Colimes");
                sectores.Add("Juan Bautista Aguirre");
                sectores.Add("Eloy Alfaro");
                sectores.Add("Velasco Ibarra");
                sectores.Add("El Triunfo");
                sectores.Add("General Antonio Elizalde (Bucay)");
                sectores.Add("Isidro Ayora");
                sectores.Add("Lomas De Sargentillo");
                sectores.Add("Marcelino Maridueña");
                sectores.Add("Milagro");
                sectores.Add("Naranjal");
                sectores.Add("Naranjito");
                sectores.Add("Narcisa De Jesús");
                sectores.Add("Palestina");
                sectores.Add("Pedro Carbo");
                sectores.Add("General Villamil");
                sectores.Add("Bocana");
                sectores.Add("Samborondón");
                sectores.Add("Santa Lucía");
                sectores.Add("Simón Bolívar");
                sectores.Add("General Pedro J. Montero");
                sectores.Add("Cahuasquí");
                sectores.Add("Ambuqui");
                sectores.Add("Andrade Marín");
                sectores.Add("Sagrario");
                sectores.Add("Jordán");
                sectores.Add("Pimampiro");
                sectores.Add("El Sagrario");
                sectores.Add("Colaisaca");
                sectores.Add("Tambo");
                sectores.Add("Cruzpamba");
                sectores.Add("Chaguarpamba");
                sectores.Add("Amaluza");
                sectores.Add("Changaimina");
                sectores.Add("General Eloy Alfaro");
                sectores.Add("Olmedo");
                sectores.Add("Cangonamá");
                sectores.Add("Pindal");
                sectores.Add("Ciano");
                sectores.Add("Clemente Baquerizo");
                sectores.Add("Baba");
                sectores.Add("San Jacinto De Buena Fé");
                sectores.Add("Mocache");
                sectores.Add("Montalvo");
                sectores.Add("Palenque");
                sectores.Add("Puerto Pechiche");
                sectores.Add("Quevedo");
                sectores.Add("Quinsaloma");
                sectores.Add("Catarama");
                sectores.Add("Valencia");
                sectores.Add("Chacarita");
                sectores.Add("Vinces");
                sectores.Add("Abraham Calazacón");
                sectores.Add("Alangasí");
                sectores.Add("Tipitini");
                sectores.Add("Atocha – Ficoa");
                sectores.Add("Celiano Monge");
                sectores.Add("Huachi Chico");
                sectores.Add("Huachi Loreto");
                sectores.Add("La Merced");
                sectores.Add("La Península");
                sectores.Add("Matriz");
                sectores.Add("Pishilata");
                sectores.Add("San Francisco");
                sectores.Add("Ambato");
                sectores.Add("Ambatillo");
                sectores.Add("Atahualpa (Chisalata)");
                sectores.Add("Augusto N. Martínez (Mundugleo)");
                sectores.Add("Constantino Fernández (Cab. En Cullitahua)");
                sectores.Add("Huachi Grande");
                sectores.Add("Izamba");
                sectores.Add("Juan Benigno Vela");
                sectores.Add("Montalvo");
                sectores.Add("Pasa");
                sectores.Add("Picaigua");
                sectores.Add("Pilagüín (Pilahüín)");
                sectores.Add("Quisapincha (Quizapincha)");
                sectores.Add("San Bartolomé De Pinllog");
                sectores.Add("San Fernando (Pasa San Fernando)");
                sectores.Add("Santa Rosa");
                sectores.Add("Totoras");
                sectores.Add("Cunchibamba");
                sectores.Add("Unamuncho");
                sectores.Add("Arajuno");
                sectores.Add("Curaray");
                sectores.Add("Archidona");
                sectores.Add("Avila");
                sectores.Add("Cotundo");
                sectores.Add("Loreto");
                sectores.Add("San Pablo De Ushpayacu");
                sectores.Add("Puerto Murialdo");
                sectores.Add("Chacras");
                sectores.Add("Palmales");
                sectores.Add("Carcabón");
                sectores.Add("Carlos_Julio_Arosemena_Tola");
                sectores.Add("Paccha");
                sectores.Add("Balsas");
                sectores.Add("Baños De Agua Santa");
                sectores.Add("Biblián");
                sectores.Add("Bolívar");
                sectores.Add("Camilo Ponce Enríquez");
                sectores.Add("Cañar");
                sectores.Add("El Dorado De Cascales");
                sectores.Add("Ayora");
                sectores.Add("Zumbi");
                sectores.Add("Cevallos");
                sectores.Add("Chillanes");
                sectores.Add("San José De Chimbo");
                sectores.Add("Zumba");
                sectores.Add("Chone");
                sectores.Add("Chordeleg");
                sectores.Add("Chunchi");
                sectores.Add("Cajabamba");
                sectores.Add("Cumandá");
                sectores.Add("Tarapoa");
                sectores.Add("Déleg");
                sectores.Add("El Carmen");
                sectores.Add("El Chaco");
                sectores.Add("El Pan");
                sectores.Add("El Pangui");
                sectores.Add("El Piedrero");
                sectores.Add("El Ángel");
                sectores.Add("Flavio Alfaro");
                sectores.Add("Girón");
                sectores.Add("El Dorado De Cascales");
                sectores.Add("Guachapala");
                sectores.Add("Gualaceo");
                sectores.Add("Mercedes Molina");
                sectores.Add("La Matriz");
                sectores.Add("Gabriel Ignacio Veintimilla");
                sectores.Add("Chiguaza");
                sectores.Add("Jama");
                sectores.Add("Jaramijó");
                sectores.Add("Dr. Miguel Morán Lucio");
                sectores.Add("Junín");
                sectores.Add("La Concordia");
                sectores.Add("La Joya De Los Sachas");
                sectores.Add("La Libertad");
                sectores.Add("El Carmen");
                sectores.Add("La Troncal");
                sectores.Add("Nueva Loja");
                sectores.Add("Las Golondrinas");
                sectores.Add("Las Mercedes");
                sectores.Add("Eloy Alfaro (San Felipe)");
                sectores.Add("General Leonidas Plaza Gutiérrez (Limón)");
                sectores.Add("Logroño");
                sectores.Add("Loreto");
                sectores.Add("La Providencia");
                sectores.Add("Manga Del Cura");
                sectores.Add("Los Esteros");
                sectores.Add("Mira (Chontahuasi)");
                sectores.Add("Alóag");
                sectores.Add("Mocha");
                sectores.Add("Sucre");
                sectores.Add("Anibal San Andrés");
                sectores.Add("González Suárez");
                sectores.Add("Macas");
                sectores.Add("Nabón");
                sectores.Add("Guayzimi");
                sectores.Add("Copal");
                sectores.Add("San Felipe De Oña Cabecera Cantonal");
                sectores.Add("Puerto Francisco De Orellana (El Coca)");
                sectores.Add("Pablo Sexto");
                sectores.Add("Paján");
                sectores.Add("Palanda");
                sectores.Add("Pallatanga");
                sectores.Add("Palora (Metzera)");
                sectores.Add("El Corazón");
                sectores.Add("Paquisha");
                sectores.Add("Puyo");
                sectores.Add("Patate");
                sectores.Add("Paute");
                sectores.Add("Pedernales");
                sectores.Add("Tabacundo");
                sectores.Add("Pedro Vicente Maldonado");
                sectores.Add("Penipe");
                sectores.Add("Pichincha");
                sectores.Add("Portovelo");
                sectores.Add("Portoviejo");
                sectores.Add("Puerto López");
                sectores.Add("Puerto Quito");
                sectores.Add("Puerto El Carmen Del Putumayo");
                sectores.Add("Quero");
                sectores.Add("Baeza");
                sectores.Add("Quilanga");
                sectores.Add("Rosa Zárate (Quinindé)");
                sectores.Add("Lizarzaburu");
                sectores.Add("Sangolquí");
                sectores.Add("San Miguel");
                sectores.Add("Carlos Espinoza Larrea");
                sectores.Add("San Fernando");
                sectores.Add("San Jacinto De Yaguachi");
                sectores.Add("San Juan Bosco");
                sectores.Add("San Miguel");
                sectores.Add("San Miguel De Los Bancos");
                sectores.Add("Huaca");
                sectores.Add("Pelileo");
                sectores.Add("San Vicente");
                sectores.Add("Santa Ana");
                sectores.Add("Santa Clara");
                sectores.Add("Ballenita");
                sectores.Add("Santa Isabel (Chaguarurco)");
                sectores.Add("Santiago De Méndez");
                sectores.Add("Ciudad Nueva");
                sectores.Add("Saquisilí");
                sectores.Add("Saraguro");
                sectores.Add("Shushufindi");
                sectores.Add("Sigchos");
                sectores.Add("Sigsig");
                sectores.Add("Sozoranga");
                sectores.Add("Bahía De Caráquez");
                sectores.Add("Sucúa");
                sectores.Add("La Bonita");
                sectores.Add("Taisha");
                sectores.Add("Tena");
                sectores.Add("Tisaleo");
                sectores.Add("Santiago");
                sectores.Add("Tosagua");
                sectores.Add("González Suárez");
                sectores.Add("28 De Mayo (San José De Yacuambi)");
                sectores.Add("Yantzaza (Yanzatza)");
                sectores.Add("Zapotillo");
                sectores.Add("Zapotillo");
                sectores.Add("Zaruma");
                sectores.Add("Aurelio Bayas Martínez");
                sectores.Add("Chaucha");
                sectores.Add("La Unión");
                sectores.Add("San Gerardo");
                sectores.Add("Jadán");
                sectores.Add("El Progreso");
                sectores.Add("Chicán");
                sectores.Add("San Salvador de Cañaribamba");
                sectores.Add("Palmas");
                sectores.Add("Gima");
                sectores.Add("Julio E. Moreno");
                sectores.Add("Magdalena");
                sectores.Add("Las Naves");
                sectores.Add("Bilován");
                sectores.Add("Guapán");
                sectores.Add("Nazón");
                sectores.Add("Chorocopte");
                sectores.Add("Pancho Negro");
                sectores.Add("El Chical");
                sectores.Add("Los Andes");
                sectores.Add("La libertad");
                sectores.Add("Jijón y Caamaño");
                sectores.Add("Chitán de Navarrete");
                sectores.Add("Cubijíes");
                sectores.Add("Guasuntos");
                sectores.Add("Comud");
                sectores.Add("Juan de Velasco");
                sectores.Add("Palmira");
                sectores.Add("Ilapo");
                sectores.Add("La Candelaria");
                sectores.Add("Alaques");
                sectores.Add("Pucayacu");
                sectores.Add("Pinllopata");
                sectores.Add("Guangaje");
                sectores.Add("Cusubamba");
                sectores.Add("Cochapamba");
                sectores.Add("Isinlivi");
                sectores.Add("La Providencia");
                sectores.Add("Carabota");
                sectores.Add("Barbones (Sucre)");
                sectores.Add("Hualtaco");
                sectores.Add("Platanillos");
                sectores.Add("Cañaquemada");
                sectores.Add("La Bocana");
                sectores.Add("Morales");
                sectores.Add("Bellavista");
                sectores.Add("Coronel Carlos Concha Torres");
                sectores.Add("Súa");
                sectores.Add("Atahualpa");
                sectores.Add("Daule");
                sectores.Add("Chura");
                sectores.Add("Chumundé");
                sectores.Add("Ancón");
                sectores.Add("Floreana");
                sectores.Add("Puerto Villamil");
                sectores.Add("Bellavista");
                sectores.Add("Morro");
                sectores.Add("San Jacinto");
                sectores.Add("Laurel");
                sectores.Add("El Recreo");
                sectores.Add("Guayas");
                sectores.Add("Chobo");
                sectores.Add("Jesús María");
                sectores.Add("Valle De La Virgen");
                sectores.Add("Candilejos");
                sectores.Add("La Puntilla");
                sectores.Add("Coronel Lorenzo de Garaicoa");
                sectores.Add("Yaguachi Viejo");
                sectores.Add("La Merced De Buenos Aires");
                sectores.Add("Angochagua");
                sectores.Add("Atuntaqui");
                sectores.Add("San Francisco");
                sectores.Add("San Luis");
                sectores.Add("Chugá");
                sectores.Add("San Sebastián");
                sectores.Add("El Lucero");
                sectores.Add("Guayquichuma");
                sectores.Add("Pozul");
                sectores.Add("Buenavista");
                sectores.Add("Bellavista");
                sectores.Add("Nambacola");
                sectores.Add("Larama");
                sectores.Add("Guachanamá");
                sectores.Add("Chaquinal");
                sectores.Add("El Arenal");
                sectores.Add("Dr. Camilo Ponce");
                sectores.Add("Guare");
                sectores.Add("7 De Agosto");
                sectores.Add("San Juan");
                sectores.Add("San Camilo");
                sectores.Add("Ricaurte");
                sectores.Add("Los Ángeles");
                sectores.Add("Antonio Sotomayor");
                sectores.Add("Bombolí");
                sectores.Add("Amaguaña");
                sectores.Add("Nuevo Rocafuerte");
                sectores.Add("Ayapamba");
                sectores.Add("Bellamaría");
                sectores.Add("Lligua");
                sectores.Add("Nazón (Cab. En Pampa De Domínguez)");
                sectores.Add("García Moreno");
                sectores.Add("El Carmen De Pijilí");
                sectores.Add("Chontamarca");
                sectores.Add("Santa Rosa De Sucumbíos");
                sectores.Add("Cayambe");
                sectores.Add("Paquisha");
                sectores.Add("San José Del Tambo (Tambopamba)");
                sectores.Add("Asunción (Asancoto)");
                sectores.Add("Chito");
                sectores.Add("Santa Rita");
                sectores.Add("Principal");
                sectores.Add("Capzol");
                sectores.Add("Sicalpa");
                sectores.Add("Cuyabeno");
                sectores.Add("Solano");
                sectores.Add("4 De Diciembre");
                sectores.Add("Gonzalo Díaz De Pineda (El Bombón)");
                sectores.Add("Amaluza");
                sectores.Add("El Guisme");
                sectores.Add("27 De Septiembre");
                sectores.Add("San Francisco De Novillo (Cab. En");
                sectores.Add("Asunción");
                sectores.Add("El Reventador");
                sectores.Add("Chordeleg");
                sectores.Add("Gualaquiza");
                sectores.Add("Guano");
                sectores.Add("Guanujo");
                sectores.Add("Pablo Sexto");
                sectores.Add("Manuel Inocencio Parrales Y Guale");
                sectores.Add("Monterrey");
                sectores.Add("Enokanqui");
                sectores.Add("La Maná");
                sectores.Add("Manuel J. Calle");
                sectores.Add("Cuyabeno");
                sectores.Add("Las Naves");
                sectores.Add("Ignacio Flores (Parque Flores)");
                sectores.Add("Indanza");
                sectores.Add("Yaupi");
                sectores.Add("Avila (Cab. En Huiruno)");
                sectores.Add("Machala");
                sectores.Add("Manta");
                sectores.Add("Concepción");
                sectores.Add("Aloasi");
                sectores.Add("Pinguilí");
                sectores.Add("Bellavista");
                sectores.Add("Montecristi");
                sectores.Add("San José");
                sectores.Add("Alshi (Cab. En 9 De Octubre)");
                sectores.Add("Cochapata");
                sectores.Add("Zurmi");
                sectores.Add("Chupianza");
                sectores.Add("Susudel");
                sectores.Add("Dayuma");
                sectores.Add("Campozano (La Palma De Paján)");
                sectores.Add("El Porvenir Del Carmen");
                sectores.Add("Arapicos");
                sectores.Add("Moraspungo");
                sectores.Add("Bellavista");
                sectores.Add("Arajuno");
                sectores.Add("El Triunfo");
                sectores.Add("Amaluza");
                sectores.Add("Cojimíes");
                sectores.Add("La Esperanza");
                sectores.Add("El Altar");
                sectores.Add("Barraganete");
                sectores.Add("Curtincapa");
                sectores.Add("12 De Marzo");
                sectores.Add("Machalilla");
                sectores.Add("Palma Roja");
                sectores.Add("Rumipamba");
                sectores.Add("Cosanga");
                sectores.Add("Fundochamba");
                sectores.Add("Cube");
                sectores.Add("Maldonado");
                sectores.Add("San Pedro De Taboada");
                sectores.Add("Antonio José Holguín (Santa Lucía)");
                sectores.Add("Gral. Alberto Enríquez Gallo");
                sectores.Add("Chumblín");
                sectores.Add("Crnel. Lorenzo De Garaicoa (Pedregal)");
                sectores.Add("Pan De Azúcar");
                sectores.Add("Balsapamba");
                sectores.Add("Mindo");
                sectores.Add("Mariscal Sucre");
                sectores.Add("Pelileo Grande");
                sectores.Add("Canoa");
                sectores.Add("Lodana");
                sectores.Add("San José");
                sectores.Add("Santa Elena");
                sectores.Add("Abdón Calderón (La Unión)");
                sectores.Add("Copal");
                sectores.Add("Píllaro");
                sectores.Add("Canchagua");
                sectores.Add("El Paraíso De Celén");
                sectores.Add("Limoncocha");
                sectores.Add("Chugchillán");
                sectores.Add("Cuchil (Cutchil)");
                sectores.Add("Nueva Fátima");
                sectores.Add("Leonidas Plaza Gutiérrez");
                sectores.Add("Asunción");
                sectores.Add("El Playón De San Francisco");
                sectores.Add("Huasaga (Cab. En Wampuik)");
                sectores.Add("Ahuano");
                sectores.Add("Quinchicoto");
                sectores.Add("San José De Morona");
                sectores.Add("Bachillero");
                sectores.Add("Tulcán");
                sectores.Add("La Paz");
                sectores.Add("Chicaña");
                sectores.Add("Mangahurco (Cazaderos)");
                sectores.Add("Mangahurco (Cazaderos)");
                sectores.Add("Abañín");
                sectores.Add("Azogues");
                sectores.Add("Checa");
                sectores.Add("Luis Galarza Orellana");
                sectores.Add("Luis Cordero Vega");
                sectores.Add("Las Nieves");
                sectores.Add("Dug");
                sectores.Add("Zhaglli");
                sectores.Add("Güel");
                sectores.Add("Salinas");
                sectores.Add("San Sebastián");
                sectores.Add("Régulo de Mora");
                sectores.Add("Javier Loyola");
                sectores.Add("San Francisco de Sageo");
                sectores.Add("Ducur");
                sectores.Add("Julio Andrade");
                sectores.Add("Monte Olivo");
                sectores.Add("San Isidro.");
                sectores.Add("Juan Montalvo");
                sectores.Add("Fernández Salvador");
                sectores.Add("Flores");
                sectores.Add("Huigra");
                sectores.Add("Gonzol");
                sectores.Add("Santiago de Quito");
                sectores.Add("La Providencia");
                sectores.Add("Matus");
                sectores.Add("Belisario Quevedo");
                sectores.Add("La Maná");
                sectores.Add("Ramón Campaña");
                sectores.Add("La Victoria");
                sectores.Add("Mulalillo");
                sectores.Add("Las Pamppas");
                sectores.Add("Machala");
                sectores.Add("Casacay");
                sectores.Add("La Iberia");
                sectores.Add("Milton Reyes");
                sectores.Add("Valle Hermoso");
                sectores.Add("Casacay");
                sectores.Add("Moromoro");
                sectores.Add("Salatí");
                sectores.Add("Jambelí");
                sectores.Add("Chinca");
                sectores.Add("Tonchigüe");
                sectores.Add("Borbón");
                sectores.Add("Galera");
                sectores.Add("La Unión");
                sectores.Add("Lagarto");
                sectores.Add("Calderón");
                sectores.Add("El Progreso");
                sectores.Add("Santa Rosa (Incluye La Isla Baltra)");
                sectores.Add("Posorja");
                sectores.Add("Limonal");
                sectores.Add("El Rosario");
                sectores.Add("Mariscal Sucre");
                sectores.Add("San Carlos");
                sectores.Add("Sabanilla");
                sectores.Add("Central");
                sectores.Add("Tarifa");
                sectores.Add("Virgen de Fátima");
                sectores.Add("Pablo Arenas");
                sectores.Add("Carolina");
                sectores.Add("Imbaya");
                sectores.Add("Cotacachi");
                sectores.Add("Otavalo");
                sectores.Add("Mariano Acosta");
                sectores.Add("Sucre");
                sectores.Add("Utuana");
                sectores.Add("San Pedro de la Bendita");
                sectores.Add("Sabanilla");
                sectores.Add("El Rosario");
                sectores.Add("Jimbura");
                sectores.Add("Purunuma");
                sectores.Add("La Victoria");
                sectores.Add("Lauro Guerrero");
                sectores.Add("12 De Diciembre (Cab.En Achiotes)");
                sectores.Add("El Limo");
                sectores.Add("Barreiro");
                sectores.Add("Isla De Bejucal");
                sectores.Add("11 De Octubre");
                sectores.Add("San José");
                sectores.Add("Zapotal");
                sectores.Add("Chiguilpe");
                sectores.Add("Atahualpa");
                sectores.Add("Capitán Augusto Rivadeneyra");
                sectores.Add("Cordoncillo");
                sectores.Add("Río Negro");
                sectores.Add("San Francisco De Sageo");
                sectores.Add("Los Andes");
                sectores.Add("Chorocopte");
                sectores.Add("Sevilla");
                sectores.Add("Juan Montalvo");
                sectores.Add("Triunfo-Dorado");
                sectores.Add("Caluma");
                sectores.Add("El Chorro");
                sectores.Add("Boyacá");
                sectores.Add("La Unión");
                sectores.Add("Compud");
                sectores.Add("Villa La Unión (Cajabamba)");
                sectores.Add("Aguas Negras");
                sectores.Add("El Carmen");
                sectores.Add("Linares");
                sectores.Add("Palmas");
                sectores.Add("Pachicutza");
                sectores.Add("El Angel");
                sectores.Add("Zapallo");
                sectores.Add("San Gerardo");
                sectores.Add("Gonzalo Pizarro");
                sectores.Add("Daniel Córdova Toral (El Oriente)");
                sectores.Add("Amazonas (Rosario De Cuyes)");
                sectores.Add("Guanando");
                sectores.Add("Guaranda");
                sectores.Add("San Juan Bosco");
                sectores.Add("San Lorenzo De Jipijapa");
                sectores.Add("La Villegas");
                sectores.Add("Pompeya");
                sectores.Add("El Triunfo");
                sectores.Add("Pancho Negro");
                sectores.Add("Dureno");
                sectores.Add("Las Naves");
                sectores.Add("Juan Montalvo (San Sebastián)");
                sectores.Add("Pan De Azúcar");
                sectores.Add("Shimpis");
                sectores.Add("Puerto Murialdo");
                sectores.Add("Puerto Bolívar");
                sectores.Add("San Mateo");
                sectores.Add("Jijón Y Caamaño (Cab. En Río Blanco)");
                sectores.Add("Cutuglahua");
                sectores.Add("Noboa");
                sectores.Add("El Colorado");
                sectores.Add("San Gabriel");
                sectores.Add("Chiguaza");
                sectores.Add("El Progreso (Cab.En Zhota)");
                sectores.Add("Nuevo Paraíso");
                sectores.Add("Patuca");
                sectores.Add("Yuca)");
                sectores.Add("Cascol");
                sectores.Add("San Francisco Del Vergel");
                sectores.Add("Cumandá (Cab. En Colonia Agrícola Sevilla Del Oro)");
                sectores.Add("Pinllopata");
                sectores.Add("Nuevo Quito");
                sectores.Add("Canelos");
                sectores.Add("Los Andes (Cab. En Poatug)");
                sectores.Add("Bulán (José Víctor Izquierdo)");
                sectores.Add("10 De Agosto");
                sectores.Add("Malchinguí");
                sectores.Add("Matus");
                sectores.Add("San Sebastián");
                sectores.Add("Morales");
                sectores.Add("Colón");
                sectores.Add("Salango");
                sectores.Add("Puerto Bolívar (Puerto Montúfar)");
                sectores.Add("Yanayacu - Mochapata (Cab. En Yanayacu)");
                sectores.Add("Cuyuja");
                sectores.Add("San Antonio De Las Aradas (Cab. En Las Aradas)");
                sectores.Add("Chura (Chancama) (Cab. En El Yerbero)");
                sectores.Add("Velasco");
                sectores.Add("San Rafael");
                sectores.Add("Cusubamba");
                sectores.Add("Vicente Rocafuerte");
                sectores.Add("Crnel. Marcelino Maridueña (San Carlos)");
                sectores.Add("San Carlos De Limón");
                sectores.Add("Bilován");
                sectores.Add("Pedro Vicente Maldonado");
                sectores.Add("Pelileo");
                sectores.Add("Santa Ana De Vuelta Larga");
                sectores.Add("Santa Elena");
                sectores.Add("El Carmen De Pijilí");
                sectores.Add("Chupianza");
                sectores.Add("Píllaro");
                sectores.Add("Chantilín");
                sectores.Add("El Tablón");
                sectores.Add("Pañacocha");
                sectores.Add("Isinliví");
                sectores.Add("Gima");
                sectores.Add("Tacamoros");
                sectores.Add("Bahía De Caráquez");
                sectores.Add("Huambi");
                sectores.Add("La Sofía");
                sectores.Add("Macuma");
                sectores.Add("Carlos Julio Arosemena Tola (Zatza-Yacu)");
                sectores.Add("Angel Pedro Giler (La Estancilla)");
                sectores.Add("Tulcán");
                sectores.Add("Tutupali");
                sectores.Add("El Pangui");
                sectores.Add("Garzareal");
                sectores.Add("Garzareal");
                sectores.Add("Arcapamba");
                sectores.Add("Borrero");
                sectores.Add("Chiquintad");
                sectores.Add("San Martín de Puzhio");
                sectores.Add("Mariano Moreno");
                sectores.Add("Tomebamba)");
                sectores.Add("Ludo");
                sectores.Add("San Lorenzo");
                sectores.Add("Telimbela");
                sectores.Add("San Pablo");
                sectores.Add("Luis Cordero");
                sectores.Add("Turupamba)");
                sectores.Add("General Morales");
                sectores.Add("Maldonado");
                sectores.Add("San Vicente de Pusir");
                sectores.Add("27 de Septiembre");
                sectores.Add("La Paz");
                sectores.Add("Licán");
                sectores.Add("Multitud");
                sectores.Add("Llagos");
                sectores.Add("Cajabamba");
                sectores.Add("San Andrés");
                sectores.Add("Puela");
                sectores.Add("Guaitacama");
                sectores.Add("El Triunfo");
                sectores.Add("El Corazón");
                sectores.Add("Pilaló");
                sectores.Add("Mulliquindil");
                sectores.Add("Palo Quemado");
                sectores.Add("Puerto Bolívar");
                sectores.Add("Challiguro");
                sectores.Add("Tendales");
                sectores.Add("Unión Lojana");
                sectores.Add("La Victoria");
                sectores.Add("La Peaña");
                sectores.Add("Piedras");
                sectores.Add("La Avanzada");
                sectores.Add("Majua");
                sectores.Add("Tonsupa");
                sectores.Add("Colón Eloy del María");
                sectores.Add("Quingue");
                sectores.Add("Malimpia");
                sectores.Add("Montalvo");
                sectores.Add("Carondelet");
                sectores.Add("A Santa María");
                sectores.Add("Puná");
                sectores.Add("Los Lojas");
                sectores.Add("Roberto Astudillo");
                sectores.Add("Santa Rosa De Flandes");
                sectores.Add("Paraíso");
                sectores.Add("San Blas");
                sectores.Add("La Esperanza");
                sectores.Add("San Francisco De Natabuela");
                sectores.Add("Apuela");
                sectores.Add("Dr. Miguel Egas Cabezas (Peguche)");
                sectores.Add("San Francisco De Sigsipamba");
                sectores.Add("Valle");
                sectores.Add("Sanguillín");
                sectores.Add("Zambi");
                sectores.Add("Teniente Maximiliano Rodríguez Loaiza");
                sectores.Add("Santa Rufina");
                sectores.Add("Santa Teresita");
                sectores.Add("Sacapalca");
                sectores.Add("Sabiango");
                sectores.Add("Orianga");
                sectores.Add("Milagros");
                sectores.Add("Mercadillo");
                sectores.Add("El Salto");
                sectores.Add("San Jacinto De Buena Fé");
                sectores.Add("Guayacán");
                sectores.Add("10 de Noviembre");
                sectores.Add("Río Toachi");
                sectores.Add("Calacalí");
                sectores.Add("Cononaco");
                sectores.Add("Milagro");
                sectores.Add("Río Verde");
                sectores.Add("Turupamba");
                sectores.Add("Monte Olivo");
                sectores.Add("General Morales (Socarte)");
                sectores.Add("Cayambe");
                sectores.Add("Panguintza");
                sectores.Add("Magdalena (Chapacoto)");
                sectores.Add("El Porvenir Del Carmen");
                sectores.Add("Canuto");
                sectores.Add("Luis Galarza Orellana (Cab.En Delegsol)");
                sectores.Add("Gonzol");
                sectores.Add("Cañi");
                sectores.Add("Wilfrido Loor Moreira (Maicito)");
                sectores.Add("Oyacachi");
                sectores.Add("San Vicente");
                sectores.Add("Tundayme");
                sectores.Add("El Goaltal");
                sectores.Add("Lumbaquí");
                sectores.Add("Jadán");
                sectores.Add("Bermejos");
                sectores.Add("Ilapo");
                sectores.Add("Facundo Vela");
                sectores.Add("Jipijapa");
                sectores.Add("Plan Piloto");
                sectores.Add("San Carlos");
                sectores.Add("La Maná");
                sectores.Add("General Farfán");
                sectores.Add("La Matriz");
                sectores.Add("San Antonio (Cab. En San Antonio Centro");
                sectores.Add("San José De Payamino");
                sectores.Add("Nueve De Mayo");
                sectores.Add("Tarqui");
                sectores.Add("Juan Montalvo (San Ignacio De Quil)");
                sectores.Add("El Chaupi");
                sectores.Add("Arq. Sixto Durán Ballén");
                sectores.Add("General Eloy Alfaro");
                sectores.Add("Cristóbal Colón");
                sectores.Add("General Proaño");
                sectores.Add("Las Nieves (Chaya)");
                sectores.Add("San Luis de El Acho");
                sectores.Add("Alejandro Labaka");
                sectores.Add("Guale");
                sectores.Add("Valladolid");
                sectores.Add("Huamboya");
                sectores.Add("Ramón Campaña");
                sectores.Add("Curaray");
                sectores.Add("Sucre (Cab. En Sucre-Patate Urcu)");
                sectores.Add("Chicán (Guillermo Ortega)");
                sectores.Add("Atahualpa");
                sectores.Add("Tocachi");
                sectores.Add("Puela");
                sectores.Add("Salatí");
                sectores.Add("Picoazá");
                sectores.Add("Puerto Rodríguez");
                sectores.Add("Papallacta");
                sectores.Add("Malimpia");
                sectores.Add("Veloz");
                sectores.Add("Sangolqui");
                sectores.Add("Mulalillo");
                sectores.Add("Santa Rosa");
                sectores.Add("Gral. Pedro J. Montero (Boliche)");
                sectores.Add("San Jacinto De Wakambeis");
                sectores.Add("Régulo De Mora");
                sectores.Add("Puerto Quito");
                sectores.Add("Benítez (Pachanlica)");
                sectores.Add("Ayacucho");
                sectores.Add("Atahualpa");
                sectores.Add("Zhaglli (Shaglli)");
                sectores.Add("Patuca");
                sectores.Add("Baquerizo Moreno");
                sectores.Add("Cochapamba");
                sectores.Add("Lluzhapa");
                sectores.Add("San Roque (Cab. En San Vicente)");
                sectores.Add("Las Pampas");
                sectores.Add("Guel");
                sectores.Add("Canoa");
                sectores.Add("Logroño");
                sectores.Add("Rosa Florida");
                sectores.Add("Tuutinentza");
                sectores.Add("Chontapunta");
                sectores.Add("El Carmelo (El Pun)");
                sectores.Add("Los Encuentros");
                sectores.Add("Limones");
                sectores.Add("Limones");
                sectores.Add("Guanazán");
                sectores.Add("San Francisco");
                sectores.Add("Llacao");
                sectores.Add("Remigio Crespo Toral");
                sectores.Add("El Cabo");
                sectores.Add("San Bartolomé");
                sectores.Add("San Luis de Pambil");
                sectores.Add("San Vicente");
                sectores.Add("Pindilig");
                sectores.Add("Gualleturo");
                sectores.Add("Pioter");
                sectores.Add("San Rafael");
                sectores.Add("El Ángel");
                sectores.Add("Piartal. Parroquias urbanas del Cantón Montufar son González Suárez");
                sectores.Add("Licto");
                sectores.Add("PistishÍ");
                sectores.Add("Cañi");
                sectores.Add("San Gerardo de Pacaicaguán");
                sectores.Add("San Antonio de Bayushig");
                sectores.Add("Joseguango Bajo");
                sectores.Add("El Carmen");
                sectores.Add("Tingo");
                sectores.Add("Pansaleo");
                sectores.Add("Nueve de Mayo");
                sectores.Add("Chucacay");
                sectores.Add("Río Bonito");
                sectores.Add("Huaquillas");
                sectores.Add("La Libertad");
                sectores.Add("Progreso");
                sectores.Add("San Roque");
                sectores.Add("San Antonio");
                sectores.Add("San Mateo");
                sectores.Add("La Tola");
                sectores.Add("Salima");
                sectores.Add("Viche");
                sectores.Add("Rocafuerte");
                sectores.Add("5 De Junio");
                sectores.Add("Tenguel");
                sectores.Add("La Aurora");
                sectores.Add("Taura");
                sectores.Add("San Mateo");
                sectores.Add("Tumbabiro");
                sectores.Add("Lita");
                sectores.Add("San José De Chaltura");
                sectores.Add("García Moreno");
                sectores.Add("Eugenio Espejo (Calpaquí)");
                sectores.Add("Loja");
                sectores.Add("Chile");
                sectores.Add("San José");
                sectores.Add("Amarillos");
                sectores.Add("27 De Abril");
                sectores.Add("Macará");
                sectores.Add("San Antonio");
                sectores.Add("Vicentino");
                sectores.Add("Babahoyo");
                sectores.Add("Patricia Pilar");
                sectores.Add("Nicolás Infante Díaz");
                sectores.Add("Río Verde");
                sectores.Add("Calderón");
                sectores.Add("Santa María De Huiririma");
                sectores.Add("San José");
                sectores.Add("Ulba");
                sectores.Add("Jerusalén");
                sectores.Add("San Vicente De Pusir");
                sectores.Add("Gualleturo");
                sectores.Add("Ascázubi");
                sectores.Add("San Sebastián");
                sectores.Add("La Chonta");
                sectores.Add("Convento");
                sectores.Add("San Martín De Puzhio");
                sectores.Add("Llagos");
                sectores.Add("Columbe");
                sectores.Add("San Pedro De Suma");
                sectores.Add("Santa Rosa");
                sectores.Add("La Libertad (Alizo)");
                sectores.Add("Puerto Libre");
                sectores.Add("Mariano Moreno");
                sectores.Add("Bomboiza");
                sectores.Add("La Providencia");
                sectores.Add("Guanujo");
                sectores.Add("América");
                sectores.Add("San Sebastián Del Coca");
                sectores.Add("Guasaganda (Cab.En Guasaganda");
                sectores.Add("Tarapoa");
                sectores.Add("San Buenaventura");
                sectores.Add("San Carlos De Limón (San Carlos Del");
                sectores.Add("San José De Dahuano");
                sectores.Add("El Cambio");
                sectores.Add("Eloy Alfaro");
                sectores.Add("Manuel Cornejo Astorga");
                sectores.Add("Leonidas Proaño");
                sectores.Add("Chitán De Navarrete");
                sectores.Add("Huasaga (Cab.En Wampuik)");
                sectores.Add("Oña");
                sectores.Add("Tayuza");
                sectores.Add("El Dorado");
                sectores.Add("Lascano");
                sectores.Add("La Canela");
                sectores.Add("Sangay (Cab. En Nayamanaca)");
                sectores.Add("Diez De Agosto");
                sectores.Add("El Cabo");
                sectores.Add("Tupigachi");
                sectores.Add("San Antonio De Bayushig");
                sectores.Add("San Pablo");
                sectores.Add("Santa Elena");
                sectores.Add("San Francisco De Borja (Virgilio Dávila)");
                sectores.Add("Viche");
                sectores.Add("Yaruquíes");
                sectores.Add("Cotogchoa");
                sectores.Add("Mulliquindil (Santa Ana)");
                sectores.Add("Salinas");
                sectores.Add("Simón Bolívar");
                sectores.Add("Santiago De Pananza");
                sectores.Add("San Pablo (San Pablo De Atenas)");
                sectores.Add("Bolívar");
                sectores.Add("Honorato Vásquez (Cab. En Vásquez)");
                sectores.Add("Colonche");
                sectores.Add("San Salvador De Cañaribamba");
                sectores.Add("San Luis De El Acho (Cab. En El Acho)");
                sectores.Add("Emilio María Terán (Rumipamba)");
                sectores.Add("Manú");
                sectores.Add("San Pedro De Los Cofanes");
                sectores.Add("Palo Quemado");
                sectores.Add("Ludo");
                sectores.Add("Cojimíes");
                sectores.Add("Yaupi");
                sectores.Add("Santa Bárbara");
                sectores.Add("Pumpuentsa");
                sectores.Add("Pano");
                sectores.Add("Huaca");
                sectores.Add("Paletillas");
                sectores.Add("Paletillas");
                sectores.Add("Guizhaguiña");
                sectores.Add("Azogues");
                sectores.Add("Molleturo");
                sectores.Add("San Juan");
                sectores.Add("Guarainag");
                sectores.Add("San José de Raranga");
                sectores.Add("San Simón");
                sectores.Add("Santiago");
                sectores.Add("Rivera");
                sectores.Add("Honorato Vásquez");
                sectores.Add("Santa Martha de Cuba");
                sectores.Add("San José");
                sectores.Add("Pungala");
                sectores.Add("Pumallacta");
                sectores.Add("San Isidro de Patulú");
                sectores.Add("Mulaló");
                sectores.Add("Zumbahua");
                sectores.Add("San Miguel");
                sectores.Add("Cune");
                sectores.Add("Marcabelí");
                sectores.Add("El Paraíso");
                sectores.Add("Uzhcurrumi");
                sectores.Add("Saracay. Parroquias urbanas del Cantón Piñas son");
                sectores.Add("Torata");
                sectores.Add("Tabiazo");
                sectores.Add("Luis Vargas Torres");
                sectores.Add("San Francisco");
                sectores.Add("Rosa Zárate");
                sectores.Add("Concepción");
                sectores.Add("Bolívar");
                sectores.Add("Banife");
                sectores.Add("El Salitre");
                sectores.Add("Salinas");
                sectores.Add("San Roque");
                sectores.Add("Imantag");
                sectores.Add("González Suárez");
                sectores.Add("Chantaco");
                sectores.Add("San Vicente");
                sectores.Add("Catamayo");
                sectores.Add("El Ingenio");
                sectores.Add("Casanga");
                sectores.Add("Alamor)");
                sectores.Add("Barreiro (Santa Rita)");
                sectores.Add("San Cristóbal");
                sectores.Add("Santo Domingo De Los Colorados");
                sectores.Add("Conocoto");
                sectores.Add("Tiputini");
                sectores.Add("San Juan De Cerro Azul");
                sectores.Add("San Rafael");
                sectores.Add("Honorato Vásquez (Tambo Viejo)");
                sectores.Add("Cangahua");
                sectores.Add("Telimbela");
                sectores.Add("Palanda");
                sectores.Add("Chibunga");
                sectores.Add("Juan De Velasco (Pangor)");
                sectores.Add("Sardinas");
                sectores.Add("San Isidro");
                sectores.Add("Santa Rosa De Sucumbíos");
                sectores.Add("Principal");
                sectores.Add("Chigüinda");
                sectores.Add("San Andrés");
                sectores.Add("Julio E. Moreno (Catanahuán Grande)");
                sectores.Add("El Anegado (Cab. En Eloy Alfaro)");
                sectores.Add("Lago San Pedro");
                sectores.Add("Pucayacu");
                sectores.Add("El Eno");
                sectores.Add("Latacunga");
                sectores.Add("San Juan Bosco");
                sectores.Add("San Vicente De Huaticocha");
                sectores.Add("Machala");
                sectores.Add("Manta");
                sectores.Add("Tambillo");
                sectores.Add("Montecristi");
                sectores.Add("Fernández Salvador");
                sectores.Add("Macuma");
                sectores.Add("San Francisco de Chinimbimi");
                sectores.Add("El Edén");
                sectores.Add("Fátima");
                sectores.Add("Guachapala");
                sectores.Add("La Candelaria");
                sectores.Add("Andrés De Vera");
                sectores.Add("San José Del Payamino");
                sectores.Add("La Unión");
                sectores.Add("Riobamba");
                sectores.Add("Rumipamba");
                sectores.Add("Pansaleo");
                sectores.Add("Anconcito");
                sectores.Add("Yaguachi Viejo (Cone)");
                sectores.Add("Santiago");
                sectores.Add("Cotaló");
                sectores.Add("La Unión");
                sectores.Add("Chanduy");
                sectores.Add("Santiago");
                sectores.Add("Marcos Espinel (Chacata)");
                sectores.Add("San Antonio De Qumbe (Cumbe)");
                sectores.Add("Siete De Julio");
                sectores.Add("San Bartolomé");
                sectores.Add("Charapotó");
                sectores.Add("Santa Marianita De Jesús");
                sectores.Add("Puerto Misahualli");
                sectores.Add("Julio Andrade (Orejuela)");
                sectores.Add("Bolaspamba");
                sectores.Add("Bolaspamba");
                sectores.Add("Huertas");
                sectores.Add("Cojitambo");
                sectores.Add("Nulti");
                sectores.Add("Simón Bolívar");
                sectores.Add("San Cristóbal");
                sectores.Add("Santafé");
                sectores.Add("San Miguel");
                sectores.Add("Ingapirca");
                sectores.Add("Tobar Donoso");
                sectores.Add("Punín");
                sectores.Add("Sevilla");
                sectores.Add("San José del Chazo");
                sectores.Add("Poaló");
                sectores.Add("Dumari");
                sectores.Add("San Isidro");
                sectores.Add("Loma de Franco");
                sectores.Add("La Susaya");
                sectores.Add("Victoria");
                sectores.Add("Tachina");
                sectores.Add("Maldonado");
                sectores.Add("San Gregorio");
                sectores.Add("Mataje");
                sectores.Add("Carbo");
                sectores.Add("Emiliano Caicedo Marcos");
                sectores.Add("Gral. Vernaza");
                sectores.Add("San Antonio");
                sectores.Add("Peñaherrera");
                sectores.Add("Pataquí");
                sectores.Add("Chuquiribamba");
                sectores.Add("Cariamanga");
                sectores.Add("El Airo");
                sectores.Add("Yamana");
                sectores.Add("Caracol");
                sectores.Add("7 De Octubre");
                sectores.Add("Zaracay");
                sectores.Add("Cumbayá");
                sectores.Add("Yasuní");
                sectores.Add("Calceta");
                sectores.Add("Ingapirca");
                sectores.Add("Olmedo (Pesillo)");
                sectores.Add("Pucapamba");
                sectores.Add("Eloy Alfaro");
                sectores.Add("Santiago De Quito (Cab. En San Antonio De Quito)");
                sectores.Add("Remigio Crespo Toral (Gúlag)");
                sectores.Add("El Rosario");
                sectores.Add("San Gerardo De Pacaicaguán");
                sectores.Add("Las Naves");
                sectores.Add("Julcuy");
                sectores.Add("Rumipamba");
                sectores.Add("Pacayacu");
                sectores.Add("Alaques (Aláquez)");
                sectores.Add("San Miguel De Conchay");
                sectores.Add("El Cambio");
                sectores.Add("San Lorenzo");
                sectores.Add("Uyumbicho");
                sectores.Add("Jaramijó");
                sectores.Add("La Paz");
                sectores.Add("San Isidro");
                sectores.Add("García Moreno");
                sectores.Add("Montalvo (Andoas)");
                sectores.Add("Guarainag");
                sectores.Add("Bilbao (Cab.En Quilluyacu)");
                sectores.Add("Francisco Pacheco");
                sectores.Add("Sumaco");
                sectores.Add("Cacha (Cab. En Machángara)");
                sectores.Add("José Luis Tamayo (Muey)");
                sectores.Add("Virgen De Fátima");
                sectores.Add("San Vicente");
                sectores.Add("Chiquicha (Cab. En Chiquicha Grande)");
                sectores.Add("Olmedo");
                sectores.Add("Manglaralto");
                sectores.Add("Tayuza");
                sectores.Add("Presidente Urbina (Chagrapamba -Patzucul)");
                sectores.Add("San Pablo De Tenta");
                sectores.Add("San José De Raranga");
                sectores.Add("10 De Agosto");
                sectores.Add("Puerto Napo");
                sectores.Add("Maldonado");
                sectores.Add("Malvas");
                sectores.Add("Déleg");
                sectores.Add("Octavio Cordero Palacios");
                sectores.Add("Zhidmad");
                sectores.Add("Tomebamba");
                sectores.Add("Simiátug");
                sectores.Add("Taday");
                sectores.Add("Juncal");
                sectores.Add("Tufiño");
                sectores.Add("Quimiag");
                sectores.Add("Sibambe");
                sectores.Add("Santa Fe de Galán");
                sectores.Add("San Juan de Pastocalle");
                sectores.Add("El Cedro");
                sectores.Add("Ochoa León");
                sectores.Add("Piñas Grande");
                sectores.Add("Jumón");
                sectores.Add("Vuelta Larga");
                sectores.Add("Pampanal de Bolívar");
                sectores.Add("San Jose de Chamanga");
                sectores.Add("San Javier De Cachaví");
                sectores.Add("Febres Cordero");
                sectores.Add("Magro");
                sectores.Add("La Victoria");
                sectores.Add("Guayaquil de Alpachaca");
                sectores.Add("Plaza Gutiérrez (Calvario)");
                sectores.Add("San José De Quichinche");
                sectores.Add("El Cisne");
                sectores.Add("Lourdes");
                sectores.Add("Febres Cordero (Las Juntas)");
                sectores.Add("24 De Mayo");
                sectores.Add("Santo Domingo De Los Colorados");
                sectores.Add("Chavezpamba");
                sectores.Add("Membrillo");
                sectores.Add("Juncal");
                sectores.Add("Otón");
                sectores.Add("San Francisco Del Vergel");
                sectores.Add("Ricaurte");
                sectores.Add("San Juan");
                sectores.Add("Nueva Tarqui");
                sectores.Add("San Isidro De Patulú");
                sectores.Add("Salinas");
                sectores.Add("La Unión");
                sectores.Add("Tres De Noviembre");
                sectores.Add("Jambelí");
                sectores.Add("Belisario Quevedo (Guanailín)");
                sectores.Add("Santa Susana De Chiviaza (Cab. En Chiviaza)");
                sectores.Add("El Retiro");
                sectores.Add("Santa Marianita (Boca De Pacoche)");
                sectores.Add("La Pila");
                sectores.Add("Piartal");
                sectores.Add("Sevilla Don Bosco");
                sectores.Add("Inés Arango (Cab. En Western)");
                sectores.Add("Pomona");
                sectores.Add("Palmas");
                sectores.Add("18 De Octubre");
                sectores.Add("Calpi");
                sectores.Add("El Rosario (Rumichaca)");
                sectores.Add("San Pablo (Cab. En Pueblo Nuevo)");
                sectores.Add("Simón Bolívar (Julio Moreno)");
                sectores.Add("San Francisco De Chinimbimi");
                sectores.Add("San Andrés");
                sectores.Add("San Sebastián De Yúluc");
                sectores.Add("Jama");
                sectores.Add("Tálag");
                sectores.Add("Pioter");
                sectores.Add("Muluncay Grande");
                sectores.Add("Guapán");
                sectores.Add("Paccha");
                sectores.Add("Quingeo");
                sectores.Add("Ricaurte");
                sectores.Add("San Joaquín");
                sectores.Add("Santa Ana");
                sectores.Add("Sayausí");
                sectores.Add("Sidcay");
                sectores.Add("Sinincay");
                sectores.Add("Tarqui");
                sectores.Add("Turi");
                sectores.Add("Valle");
                sectores.Add("Victoria del Portete");
                sectores.Add("Bellavista");
                sectores.Add("Cañaribamba");
                sectores.Add("El Batán");
                sectores.Add("El Sagrario");
                sectores.Add("El Vecino");
                sectores.Add("Gil Ramírez Dávalos");
                sectores.Add("Hermano Miguel");
                sectores.Add("Huayna Cápac");
                sectores.Add("Machángara");
                sectores.Add("Monay");
                sectores.Add("San Blas");
                sectores.Add("San Sebastián");
                sectores.Add("Sucre");
                sectores.Add("Totoracocha");
                sectores.Add("Yanuncay");
                sectores.Add("Ángel Polibio Chaves");
                sectores.Add("Gabriel Ignacio Veintimilla");
                sectores.Add("Guanujo. Guaranda es la Cabecera Cantonal");
                sectores.Add("Azogues");
                sectores.Add("Borrero");
                sectores.Add("San Francisco");
                sectores.Add("Azogues");
                sectores.Add("Aurelio Bayas Martínez");
                sectores.Add("Azogues");
                sectores.Add("Borrero");
                sectores.Add("San Francisco");
                sectores.Add("Azogues");
                sectores.Add("Aurelio Bayas Martínez");
                sectores.Add("San Antonio");
                sectores.Add("Ventura");
                sectores.Add("Zhud");
                sectores.Add("Urbina. Tulcán es la Cabecera cantonal. González Suárez es una Parroquia urbana del Cantón Tulcán)");
                sectores.Add("San Juan");
                sectores.Add("San Luis");
                sectores.Add("Maldonado");
                sectores.Add("Velasco");
                sectores.Add("Veloz");
                sectores.Add("Yaruquíes");
                sectores.Add("Lizarzaburu");
                sectores.Add("Tixán)");
                sectores.Add("Valparaíso");
                sectores.Add("El Rosario");
                sectores.Add("Gallo Cantana");
                sectores.Add("Luz de América");
                sectores.Add("Nudillo");
                sectores.Add("Pacay");
                sectores.Add("Pacayunga");
                sectores.Add("Pano");
                sectores.Add("Pejeyacu");
                sectores.Add("Playas de Daucay");
                sectores.Add("Playas de San Tin Tin");
                sectores.Add("Pueblo Viejo");
                sectores.Add("Quera Alto");
                sectores.Add("Shiguil");
                sectores.Add("Shiquil");
                sectores.Add("Tanicuchí");
                sectores.Add("Toacaso");
                sectores.Add("Ignacio Flores");
                sectores.Add("Juan Montalvo");
                sectores.Add("La Matriz");
                sectores.Add("San Buenaventura");
                sectores.Add("Eloy Alfaro");
                sectores.Add("Tres Cerritos");
                sectores.Add("Bolivar");
                sectores.Add("La Matriz");
                sectores.Add("Nuevo Santa Rosa");
                sectores.Add("Puerto Jelí");
                sectores.Add("Santa Rosa");
                sectores.Add("Balneario Jambelí");
                sectores.Add("Bartolomé Ruiz");
                sectores.Add("Esmeraldas");
                sectores.Add("Luis Tello");
                sectores.Add("Simón Plata Torres");
                sectores.Add("5 de Agosto");
                sectores.Add("San Francisco de Onzole");
                sectores.Add("San José de Cayapas");
                sectores.Add("Santo Domingo de Onzole");
                sectores.Add("Santa Lucía de las Peñas");
                sectores.Add("Selva Alegre");
                sectores.Add("Telembí");
                sectores.Add("Timbiré");
                sectores.Add("Valdez");
                sectores.Add("Santa Rita");
                sectores.Add("Tambillo");
                sectores.Add("Tululbí");
                sectores.Add("Urbina");
                sectores.Add("García Moreno");
                sectores.Add("Letamendi");
                sectores.Add("Nueve de Octubre");
                sectores.Add("Olmedo");
                sectores.Add("Roca");
                sectores.Add("Rocafuerte");
                sectores.Add("Sucre");
                sectores.Add("Tarqui");
                sectores.Add("Urdaneta");
                sectores.Add("Ximena");
                sectores.Add("Pascuales");
                sectores.Add("Guayaquil");
                sectores.Add("Ayacucho");
                sectores.Add("Padre Juan Bautista Aguirre");
                sectores.Add("Santa Clara");
                sectores.Add("Vicente Piedrahita");
                sectores.Add("Olmedo");
                sectores.Add("Roca");
                sectores.Add("Rocafuerte");
                sectores.Add("Sucre");
                sectores.Add("Tarqui");
                sectores.Add("Urdaneta");
                sectores.Add("Ximena");
                sectores.Add("Pascuales");
                sectores.Add("Daule");
                #endregion
                ViewBag.fecha_nacimiento = aspNetUsers.fecha_nacimiento_;
                ViewBag.cantones = new SelectList(cantones, aspNetUsers.ciudad);
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
                
                return RedirectToAction("pagoRequerido","SUSCRIPCIONs", new { estatus });
            }
        }

        // POST: AspNetUsers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, HttpPostedFileBase img5, HttpPostedFileBase img6,[Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,nombre,apellido,ciudad,sector,calle,telefono,latitud,longitud,nombre_peluqueria,estado,fecha_nacimiento_,fecha_creacion_,capacidad_simultanea_")] AspNetUsers aspNetUsers, string sectores, string cantones)
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
            usuarioEdit.ciudad = cantones;
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
                return RedirectToAction("Index", "SERVICIOS");
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
