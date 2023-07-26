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
                cantones.Add("CUENCA");
                cantones.Add("GIRÓN");
                cantones.Add("GUALACEO");
                cantones.Add("NABÓN");
                cantones.Add("PAUTE");
                cantones.Add("PUCARA");
                cantones.Add("SAN FERNANDO");
                cantones.Add("SANTA ISABEL");
                cantones.Add("SIGSIG");
                cantones.Add("OÑA");
                cantones.Add("CHORDELEG");
                cantones.Add("EL PAN");
                cantones.Add("SEVILLA DE ORO");
                cantones.Add("GUACHAPALA");
                cantones.Add("CAMILO PONCE ENRÍQUEZ");
                cantones.Add("GUARANDA");
                cantones.Add("CHILLANES");
                cantones.Add("CHIMBO");
                cantones.Add("ECHEANDÍA");
                cantones.Add("SAN MIGUEL");
                cantones.Add("CALUMA");
                cantones.Add("LAS NAVES");
                cantones.Add("AZOGUES");
                cantones.Add("BIBLIÁN");
                cantones.Add("CAÑAR");
                cantones.Add("LA TRONCAL");
                cantones.Add("EL TAMBO");
                cantones.Add("DÉLEG");
                cantones.Add("SUSCAL");
                cantones.Add("TULCÁN");
                cantones.Add("BOLÍVAR");
                cantones.Add("ESPEJO");
                cantones.Add("MIRA");
                cantones.Add("MONTÚFAR");
                cantones.Add("SAN PEDRO DE HUACA");
                cantones.Add("LATACUNGA");
                cantones.Add("LA MANÁ");
                cantones.Add("PANGUA");
                cantones.Add("PUJILI");
                cantones.Add("SALCEDO");
                cantones.Add("SAQUISILÍ");
                cantones.Add("SIGCHOS");
                cantones.Add("RIOBAMBA");
                cantones.Add("ALAUSI");
                cantones.Add("COLTA");
                cantones.Add("CHAMBO");
                cantones.Add("CHUNCHI");
                cantones.Add("GUAMOTE");
                cantones.Add("GUANO");
                cantones.Add("PALLATANGA");
                cantones.Add("PENIPE");
                cantones.Add("CUMANDÁ");
                cantones.Add("MACHALA");
                cantones.Add("ARENILLAS");
                cantones.Add("ATAHUALPA");
                cantones.Add("BALSAS");
                cantones.Add("CHILLA");
                cantones.Add("EL GUABO");
                cantones.Add("HUAQUILLAS");
                cantones.Add("MARCABELÍ");
                cantones.Add("PASAJE");
                cantones.Add("PIÑAS");
                cantones.Add("PORTOVELO");
                cantones.Add("SANTA ROSA");
                cantones.Add("ZARUMA");
                cantones.Add("LAS LAJAS");
                cantones.Add("ESMERALDAS");
                cantones.Add("ELOY ALFARO");
                cantones.Add("MUISNE");
                cantones.Add("QUININDÉ");
                cantones.Add("SAN LORENZO");
                cantones.Add("ATACAMES");
                cantones.Add("RIOVERDE");
                cantones.Add("LA CONCORDIA");
                cantones.Add("GUAYAQUIL");
                cantones.Add("DO BAQUERIZO MORENO (JU");
                cantones.Add("BALAO");
                cantones.Add("BALZAR");
                cantones.Add("COLIMES");
                cantones.Add("DAULE");
                cantones.Add("DURÁN");
                cantones.Add("EL EMPALME");
                cantones.Add("EL TRIUNFO");
                cantones.Add("MILAGRO");
                cantones.Add("NARANJAL");
                cantones.Add("NARANJITO");
                cantones.Add("PALESTINA");
                cantones.Add("PEDRO CARBO");
                cantones.Add("SAMBORONDÓN");
                cantones.Add("SANTA LUCÍA");
                cantones.Add("SALITRE (URBINA JADO)");
                cantones.Add("SAN JACINTO DE YAGUACHI");
                cantones.Add("PLAYAS");
                cantones.Add("SIMÓN BOLÍVAR");
                cantones.Add("ORONEL MARCELINO MARIDUE");
                cantones.Add("LOMAS DE SARGENTILLO");
                cantones.Add("NOBOL");
                cantones.Add("GENERAL ANTONIO ELIZALDE");
                cantones.Add("ISIDRO AYORA");
                cantones.Add("IBARRA");
                cantones.Add("ANTONIO ANTE");
                cantones.Add("COTACACHI");
                cantones.Add("OTAVALO");
                cantones.Add("PIMAMPIRO");
                cantones.Add("SAN MIGUEL DE URCUQUÍ");
                cantones.Add("LOJA");
                cantones.Add("CALVAS");
                cantones.Add("CATAMAYO");
                cantones.Add("CELICA");
                cantones.Add("CHAGUARPAMBA");
                cantones.Add("ESPÍNDOLA");
                cantones.Add("GONZANAMÁ");
                cantones.Add("MACARÁ");
                cantones.Add("PALTAS");
                cantones.Add("PUYANGO");
                cantones.Add("SARAGURO");
                cantones.Add("SOZORANGA");
                cantones.Add("ZAPOTILLO");
                cantones.Add("PINDAL");
                cantones.Add("QUILANGA");
                cantones.Add("OLMEDO");
                cantones.Add("BABAHOYO");
                cantones.Add("BABA");
                cantones.Add("MONTALVO");
                cantones.Add("PUEBLOVIEJO");
                cantones.Add("QUEVEDO");
                cantones.Add("URDANETA");
                cantones.Add("VENTANAS");
                cantones.Add("VÍNCES");
                cantones.Add("PALENQUE");
                cantones.Add("BUENA FÉ");
                cantones.Add("VALENCIA");
                cantones.Add("MOCACHE");
                cantones.Add("QUINSALOMA");
                cantones.Add("PORTOVIEJO");
                cantones.Add("CHONE");
                cantones.Add("EL CARMEN");
                cantones.Add("FLAVIO ALFARO");
                cantones.Add("JIPIJAPA");
                cantones.Add("JUNÍN");
                cantones.Add("MANTA");
                cantones.Add("MONTECRISTI");
                cantones.Add("PAJÁN");
                cantones.Add("PICHINCHA");
                cantones.Add("ROCAFUERTE");
                cantones.Add("SANTA ANA");
                cantones.Add("SUCRE");
                cantones.Add("TOSAGUA");
                cantones.Add("24 DE MAYO");
                cantones.Add("PEDERNALES");
                cantones.Add("PUERTO LÓPEZ");
                cantones.Add("JAMA");
                cantones.Add("JARAMIJÓ");
                cantones.Add("SAN VICENTE");
                cantones.Add("MORONA");
                cantones.Add("GUALAQUIZA");
                cantones.Add("LIMÓN INDANZA");
                cantones.Add("PALORA");
                cantones.Add("SANTIAGO");
                cantones.Add("SUCÚA");
                cantones.Add("HUAMBOYA");
                cantones.Add("SAN JUAN BOSCO");
                cantones.Add("TAISHA");
                cantones.Add("LOGROÑO");
                cantones.Add("PABLO SEXTO");
                cantones.Add("TIWINTZA");
                cantones.Add("TENA");
                cantones.Add("ARCHIDONA");
                cantones.Add("EL CHACO");
                cantones.Add("QUIJOS");
                cantones.Add("ARLOS JULIO AROSEMENA TOL");
                cantones.Add("PASTAZA");
                cantones.Add("MERA");
                cantones.Add("SANTA CLARA");
                cantones.Add("ARAJUNO");
                cantones.Add("QUITO");
                cantones.Add("CAYAMBE");
                cantones.Add("MEJIA");
                cantones.Add("PEDRO MONCAYO");
                cantones.Add("RUMIÑAHUI");
                cantones.Add("SAN MIGUEL DE LOS BANCOS");
                cantones.Add("PEDRO VICENTE MALDONADO");
                cantones.Add("PUERTO QUITO");
                cantones.Add("AMBATO");
                cantones.Add("BAÑOS DE AGUA SANTA");
                cantones.Add("CEVALLOS");
                cantones.Add("MOCHA");
                cantones.Add("PATATE");
                cantones.Add("QUERO");
                cantones.Add("SAN PEDRO DE PELILEO");
                cantones.Add("SANTIAGO DE PÍLLARO");
                cantones.Add("TISALEO");
                cantones.Add("ZAMORA");
                cantones.Add("CHINCHIPE");
                cantones.Add("NANGARITZA");
                cantones.Add("YACUAMBI");
                cantones.Add("YANTZAZA (YANZATZA)");
                cantones.Add("EL PANGUI");
                cantones.Add("CENTINELA DEL CÓNDOR");
                cantones.Add("PALANDA");
                cantones.Add("PAQUISHA");
                cantones.Add("SAN CRISTÓBAL");
                cantones.Add("ISABELA");
                cantones.Add("SANTA CRUZ");
                cantones.Add("LAGO AGRIO");
                cantones.Add("GONZALO PIZARRO");
                cantones.Add("PUTUMAYO");
                cantones.Add("SHUSHUFINDI");
                cantones.Add("SUCUMBÍOS");
                cantones.Add("CASCALES");
                cantones.Add("CUYABENO");
                cantones.Add("ORELLANA");
                cantones.Add("AGUARICO");
                cantones.Add("LA JOYA DE LOS SACHAS");
                cantones.Add("LORETO");
                cantones.Add("SANTO DOMINGO");
                cantones.Add("SANTA ELENA");
                cantones.Add("LA LIBERTAD");
                cantones.Add("SALINAS");
                cantones.Add("LAS GOLONDRINAS");
                cantones.Add("MANGA DEL CURA");
                cantones.Add("EL PIEDRERO");

                #endregion

                List<string> sectores = new List<string>();
                #region
                sectores.Add("BELLAVISTA");
                sectores.Add("CAÑARIBAMBA");
                sectores.Add("EL BATÁN");
                sectores.Add("EL SAGRARIO");
                sectores.Add("EL VECINO");
                sectores.Add("GIL RAMÍREZ DÁVALOS");
                sectores.Add("HUAYNACÁPAC");
                sectores.Add("MACHÁNGARA");
                sectores.Add("MONAY");
                sectores.Add("SAN BLAS");
                sectores.Add("SAN SEBASTIÁN");
                sectores.Add("SUCRE");
                sectores.Add("TOTORACOCHA");
                sectores.Add("YANUNCAY");
                sectores.Add("HERMANO MIGUEL");
                sectores.Add("CUENCA");
                sectores.Add("BAÑOS");
                sectores.Add("CUMBE");
                sectores.Add("CHAUCHA");
                sectores.Add("CHECA (JIDCAY)");
                sectores.Add("CHIQUINTAD");
                sectores.Add("LLACAO");
                sectores.Add("MOLLETURO");
                sectores.Add("NULTI");
                sectores.Add("OCTAVIO CORDERO PALACIOS (SANTA ROSA)");
                sectores.Add("PACCHA");
                sectores.Add("QUINGEO");
                sectores.Add("RICAURTE");
                sectores.Add("SAN JOAQUÍN");
                sectores.Add("SANTA ANA");
                sectores.Add("SAYAUSÍ");
                sectores.Add("SIDCAY");
                sectores.Add("SININCAY");
                sectores.Add("TARQUI");
                sectores.Add("TURI");
                sectores.Add("VALLE");
                sectores.Add("VICTORIA DEL PORTETE (IRQUIS)");
                sectores.Add("GIRÓN");
                sectores.Add("ASUNCIÓN");
                sectores.Add("SAN GERARDO");
                sectores.Add("GUALACEO");
                sectores.Add("CHORDELEG");
                sectores.Add("DANIEL CÓRDOVA TORAL (EL ORIENTE)");
                sectores.Add("JADÁN");
                sectores.Add("MARIANO MORENO");
                sectores.Add("PRINCIPAL");
                sectores.Add("REMIGIO CRESPO TORAL (GÚLAG)");
                sectores.Add("SAN JUAN");
                sectores.Add("ZHIDMAD");
                sectores.Add("LUIS CORDERO VEGA");
                sectores.Add("SIMÓN BOLÍVAR (CAB. EN GAÑANZOL)");
                sectores.Add("NABÓN");
                sectores.Add("COCHAPATA");
                sectores.Add("EL PROGRESO (CAB.EN ZHOTA)");
                sectores.Add("LAS NIEVES (CHAYA)");
                sectores.Add("OÑA");
                sectores.Add("PAUTE");
                sectores.Add("AMALUZA");
                sectores.Add("BULÁN (JOSÉ VÍCTOR IZQUIERDO)");
                sectores.Add("CHICÁN (GUILLERMO ORTEGA)");
                sectores.Add("EL CABO");
                sectores.Add("GUACHAPALA");
                sectores.Add("GUARAINAG");
                sectores.Add("PALMAS");
                sectores.Add("PAN");
                sectores.Add("SAN CRISTÓBAL (CARLOS ORDÓÑEZ LAZO)");
                sectores.Add("SEVILLA DE ORO");
                sectores.Add("TOMEBAMBA");
                sectores.Add("DUG DUG");
                sectores.Add("PUCARÁ");
                sectores.Add("CAMILO PONCE ENRÍQUEZ (CAB. EN RÍO 7 DE MOLLEPONGO");
                sectores.Add("SAN RAFAEL DE SHARUG");
                sectores.Add("SAN FERNANDO");
                sectores.Add("CHUMBLÍN");
                sectores.Add("SANTA ISABEL (CHAGUARURCO)");
                sectores.Add("ABDÓN CALDERÓN (LA UNIÓN)");
                sectores.Add("EL CARMEN DE PIJILÍ");
                sectores.Add("ZHAGLLI (SHAGLLI)");
                sectores.Add("SAN SALVADOR DE CAÑARIBAMBA");
                sectores.Add("SIGSIG");
                sectores.Add("CUCHIL (CUTCHIL)");
                sectores.Add("GIMA");
                sectores.Add("GUEL");
                sectores.Add("LUDO");
                sectores.Add("SAN BARTOLOMÉ");
                sectores.Add("SAN JOSÉ DE RARANGA");
                sectores.Add("SAN FELIPE DE OÑA CABECERA CANTONAL");
                sectores.Add("SUSUDEL");
                sectores.Add("CHORDELEG");
                sectores.Add("PRINCIPAL");
                sectores.Add("LA UNIÓN");
                sectores.Add("LUIS GALARZA ORELLANA (CAB.EN DELEGSOL)");
                sectores.Add("SAN MARTÍN DE PUZHIO");
                sectores.Add("EL PAN");
                sectores.Add("AMALUZA");
                sectores.Add("PALMAS");
                sectores.Add("SAN VICENTE");
                sectores.Add("SEVILLA DE ORO");
                sectores.Add("AMALUZA");
                sectores.Add("PALMAS");
                sectores.Add("GUACHAPALA");
                sectores.Add("CAMILO PONCE ENRÍQUEZ");
                sectores.Add("EL CARMEN DE PIJILÍ");
                sectores.Add("ÁNGEL POLIBIO CHÁVES");
                sectores.Add("GABRIEL IGNACIO VEINTIMILLA");
                sectores.Add("GUANUJO");
                sectores.Add("GUARANDA");
                sectores.Add("FACUNDO VELA");
                sectores.Add("GUANUJO");
                sectores.Add("JULIO E. MORENO (CATANAHUÁN GRANDE)");
                sectores.Add("LAS NAVES");
                sectores.Add("SALINAS");
                sectores.Add("SAN LORENZO");
                sectores.Add("SAN SIMÓN (YACOTO)");
                sectores.Add("SANTA FÉ (SANTA FÉ)");
                sectores.Add("SIMIÁTUG");
                sectores.Add("SAN LUIS DE PAMBIL");
                sectores.Add("CHILLANES");
                sectores.Add("SAN JOSÉ DEL TAMBO (TAMBOPAMBA)");
                sectores.Add("SAN JOSÉ DE CHIMBO");
                sectores.Add("ASUNCIÓN (ASANCOTO)");
                sectores.Add("CALUMA");
                sectores.Add("MAGDALENA (CHAPACOTO)");
                sectores.Add("SAN SEBASTIÁN");
                sectores.Add("TELIMBELA");
                sectores.Add("ECHEANDÍA");
                sectores.Add("SAN MIGUEL");
                sectores.Add("BALSAPAMBA");
                sectores.Add("BILOVÁN");
                sectores.Add("RÉGULO DE MORA");
                sectores.Add("SAN PABLO (SAN PABLO DE ATENAS)");
                sectores.Add("SANTIAGO");
                sectores.Add("SAN VICENTE");
                sectores.Add("CALUMA");
                sectores.Add("LAS MERCEDES");
                sectores.Add("LAS NAVES");
                sectores.Add("LAS NAVES");
                sectores.Add("AURELIO BAYAS MARTÍNEZ");
                sectores.Add("AZOGUES");
                sectores.Add("BORRERO");
                sectores.Add("SAN FRANCISCO");
                sectores.Add("AZOGUES");
                sectores.Add("COJITAMBO");
                sectores.Add("DÉLEG");
                sectores.Add("GUAPÁN");
                sectores.Add("JAVIER LOYOLA (CHUQUIPATA)");
                sectores.Add("LUIS CORDERO");
                sectores.Add("PINDILIG");
                sectores.Add("RIVERA");
                sectores.Add("SAN MIGUEL");
                sectores.Add("SOLANO");
                sectores.Add("TADAY");
                sectores.Add("BIBLIÁN");
                sectores.Add("NAZÓN (CAB. EN PAMPA DE DOMÍNGUEZ)");
                sectores.Add("SAN FRANCISCO DE SAGEO");
                sectores.Add("TURUPAMBA");
                sectores.Add("JERUSALÉN");
                sectores.Add("CAÑAR");
                sectores.Add("CHONTAMARCA");
                sectores.Add("CHOROCOPTE");
                sectores.Add("GENERAL MORALES (SOCARTE)");
                sectores.Add("GUALLETURO");
                sectores.Add("HONORATO VÁSQUEZ (TAMBO VIEJO)");
                sectores.Add("INGAPIRCA");
                sectores.Add("JUNCAL");
                sectores.Add("SAN ANTONIO");
                sectores.Add("SUSCAL");
                sectores.Add("TAMBO");
                sectores.Add("ZHUD");
                sectores.Add("VENTURA");
                sectores.Add("DUCUR");
                sectores.Add("LA TRONCAL");
                sectores.Add("MANUEL J. CALLE");
                sectores.Add("PANCHO NEGRO");
                sectores.Add("EL TAMBO");
                sectores.Add("DÉLEG");
                sectores.Add("SOLANO");
                sectores.Add("SUSCAL");
                sectores.Add("GONZÁLEZ SUÁREZ");
                sectores.Add("TULCÁN");
                sectores.Add("TULCÁN");
                sectores.Add("EL CARMELO (EL PUN)");
                sectores.Add("HUACA");
                sectores.Add("JULIO ANDRADE (OREJUELA)");
                sectores.Add("MALDONADO");
                sectores.Add("PIOTER");
                sectores.Add("TOBAR DONOSO (LA BOCANA DE CAMUMBÍ)");
                sectores.Add("TUFIÑO");
                sectores.Add("URBINA (TAYA)");
                sectores.Add("EL CHICAL");
                sectores.Add("MARISCAL SUCRE");
                sectores.Add("SANTA MARTHA DE CUBA");
                sectores.Add("BOLÍVAR");
                sectores.Add("GARCÍA MORENO");
                sectores.Add("LOS ANDES");
                sectores.Add("MONTE OLIVO");
                sectores.Add("SAN VICENTE DE PUSIR");
                sectores.Add("SAN RAFAEL");
                sectores.Add("EL ÁNGEL");
                sectores.Add("27 DE SEPTIEMBRE");
                sectores.Add("EL ANGEL");
                sectores.Add("EL GOALTAL");
                sectores.Add("LA LIBERTAD (ALIZO)");
                sectores.Add("SAN ISIDRO");
                sectores.Add("MIRA (CHONTAHUASI)");
                sectores.Add("CONCEPCIÓN");
                sectores.Add("JIJÓN Y CAAMAÑO (CAB. EN RÍO BLANCO)");
                sectores.Add("JUAN MONTALVO (SAN IGNACIO DE QUIL)");
                sectores.Add("GONZÁLEZ SUÁREZ");
                sectores.Add("SAN JOSÉ");
                sectores.Add("SAN GABRIEL");
                sectores.Add("CRISTÓBAL COLÓN");
                sectores.Add("CHITÁN DE NAVARRETE");
                sectores.Add("FERNÁNDEZ SALVADOR");
                sectores.Add("LA PAZ");
                sectores.Add("PIARTAL");
                sectores.Add("HUACA");
                sectores.Add("MARISCAL SUCRE");
                sectores.Add("ELOY ALFARO (SAN FELIPE)");
                sectores.Add("IGNACIO FLORES (PARQUE FLORES)");
                sectores.Add("JUAN MONTALVO (SAN SEBASTIÁN)");
                sectores.Add("LA MATRIZ");
                sectores.Add("SAN BUENAVENTURA");
                sectores.Add("LATACUNGA");
                sectores.Add("ALAQUES (ALÁQUEZ)");
                sectores.Add("BELISARIO QUEVEDO (GUANAILÍN)");
                sectores.Add("GUAITACAMA (GUAYTACAMA)");
                sectores.Add("JOSEGUANGO BAJO");
                sectores.Add("LAS PAMPAS");
                sectores.Add("MULALÓ");
                sectores.Add("11 DE NOVIEMBRE (ILINCHISI)");
                sectores.Add("POALÓ");
                sectores.Add("SAN JUAN DE PASTOCALLE");
                sectores.Add("SIGCHOS");
                sectores.Add("TANICUCHÍ");
                sectores.Add("TOACASO");
                sectores.Add("PALO QUEMADO");
                sectores.Add("EL CARMEN");
                sectores.Add("LA MANÁ");
                sectores.Add("EL TRIUNFO");
                sectores.Add("LA MANÁ");
                sectores.Add("GUASAGANDA (CAB.EN GUASAGANDA");
                sectores.Add("PUCAYACU");
                sectores.Add("EL CORAZÓN");
                sectores.Add("MORASPUNGO");
                sectores.Add("PINLLOPATA");
                sectores.Add("RAMÓN CAMPAÑA");
                sectores.Add("PUJILÍ");
                sectores.Add("ANGAMARCA");
                sectores.Add("CHUCCHILÁN (CHUGCHILÁN)");
                sectores.Add("GUANGAJE");
                sectores.Add("ISINLIBÍ (ISINLIVÍ)");
                sectores.Add("LA VICTORIA");
                sectores.Add("PILALÓ");
                sectores.Add("TINGO");
                sectores.Add("ZUMBAHUA");
                sectores.Add("SAN MIGUEL");
                sectores.Add("ANTONIO JOSÉ HOLGUÍN (SANTA LUCÍA)");
                sectores.Add("CUSUBAMBA");
                sectores.Add("MULALILLO");
                sectores.Add("MULLIQUINDIL (SANTA ANA)");
                sectores.Add("PANSALEO");
                sectores.Add("SAQUISILÍ");
                sectores.Add("CANCHAGUA");
                sectores.Add("CHANTILÍN");
                sectores.Add("COCHAPAMBA");
                sectores.Add("SIGCHOS");
                sectores.Add("CHUGCHILLÁN");
                sectores.Add("ISINLIVÍ");
                sectores.Add("LAS PAMPAS");
                sectores.Add("PALO QUEMADO");
                sectores.Add("LIZARZABURU");
                sectores.Add("MALDONADO");
                sectores.Add("VELASCO");
                sectores.Add("VELOZ");
                sectores.Add("YARUQUÍES");
                sectores.Add("RIOBAMBA");
                sectores.Add("CACHA (CAB. EN MACHÁNGARA)");
                sectores.Add("CALPI");
                sectores.Add("CUBIJÍES");
                sectores.Add("FLORES");
                sectores.Add("LICÁN");
                sectores.Add("LICTO");
                sectores.Add("PUNGALÁ");
                sectores.Add("PUNÍN");
                sectores.Add("QUIMIAG");
                sectores.Add("SAN JUAN");
                sectores.Add("SAN LUIS");
                sectores.Add("ALAUSÍ");
                sectores.Add("ACHUPALLAS");
                sectores.Add("CUMANDÁ");
                sectores.Add("GUASUNTOS");
                sectores.Add("HUIGRA");
                sectores.Add("MULTITUD");
                sectores.Add("PISTISHÍ (NARIZ DEL DIABLO)");
                sectores.Add("PUMALLACTA");
                sectores.Add("SEVILLA");
                sectores.Add("SIBAMBE");
                sectores.Add("TIXÁN");
                sectores.Add("CAJABAMBA");
                sectores.Add("SICALPA");
                sectores.Add("VILLA LA UNIÓN (CAJABAMBA)");
                sectores.Add("CAÑI");
                sectores.Add("COLUMBE");
                sectores.Add("JUAN DE VELASCO (PANGOR)");
                sectores.Add("SANTIAGO DE QUITO (CAB. EN SAN ANTONIO DE QUITO)");
                sectores.Add("CHAMBO");
                sectores.Add("CHUNCHI");
                sectores.Add("CAPZOL");
                sectores.Add("COMPUD");
                sectores.Add("GONZOL");
                sectores.Add("LLAGOS");
                sectores.Add("GUAMOTE");
                sectores.Add("CEBADAS");
                sectores.Add("PALMIRA");
                sectores.Add("EL ROSARIO");
                sectores.Add("LA MATRIZ");
                sectores.Add("GUANO");
                sectores.Add("GUANANDO");
                sectores.Add("ILAPO");
                sectores.Add("LA PROVIDENCIA");
                sectores.Add("SAN ANDRÉS");
                sectores.Add("SAN GERARDO DE PACAICAGUÁN");
                sectores.Add("SAN ISIDRO DE PATULÚ");
                sectores.Add("SAN JOSÉ DEL CHAZO");
                sectores.Add("SANTA FÉ DE GALÁN");
                sectores.Add("VALPARAÍSO");
                sectores.Add("PALLATANGA");
                sectores.Add("PENIPE");
                sectores.Add("EL ALTAR");
                sectores.Add("MATUS");
                sectores.Add("PUELA");
                sectores.Add("SAN ANTONIO DE BAYUSHIG");
                sectores.Add("LA CANDELARIA");
                sectores.Add("BILBAO (CAB.EN QUILLUYACU)");
                sectores.Add("CUMANDÁ");
                sectores.Add("LA PROVIDENCIA");
                sectores.Add("MACHALA");
                sectores.Add("PUERTO BOLÍVAR");
                sectores.Add("NUEVE DE MAYO");
                sectores.Add("EL CAMBIO");
                sectores.Add("MACHALA");
                sectores.Add("EL CAMBIO");
                sectores.Add("EL RETIRO");
                sectores.Add("ARENILLAS");
                sectores.Add("CHACRAS");
                sectores.Add("LA LIBERTAD");
                sectores.Add("LAS LAJAS (CAB. EN LA VICTORIA)");
                sectores.Add("PALMALES");
                sectores.Add("CARCABÓN");
                sectores.Add("PACCHA");
                sectores.Add("AYAPAMBA");
                sectores.Add("CORDONCILLO");
                sectores.Add("MILAGRO");
                sectores.Add("SAN JOSÉ");
                sectores.Add("SAN JUAN DE CERRO AZUL");
                sectores.Add("BALSAS");
                sectores.Add("BELLAMARÍA");
                sectores.Add("CHILLA");
                sectores.Add("EL GUABO");
                sectores.Add("BARBONES (SUCRE)");
                sectores.Add("LA IBERIA");
                sectores.Add("TENDALES (CAB.EN PUERTO TENDALES)");
                sectores.Add("RÍO BONITO");
                sectores.Add("ECUADOR");
                sectores.Add("EL PARAÍSO");
                sectores.Add("HUALTACO");
                sectores.Add("MILTON REYES");
                sectores.Add("UNIÓN LOJANA");
                sectores.Add("HUAQUILLAS");
                sectores.Add("MARCABELÍ");
                sectores.Add("EL INGENIO");
                sectores.Add("BOLÍVAR");
                sectores.Add("LOMA DE FRANCO");
                sectores.Add("OCHOA LEÓN (MATRIZ)");
                sectores.Add("TRES CERRITOS");
                sectores.Add("PASAJE");
                sectores.Add("BUENAVISTA");
                sectores.Add("CASACAY");
                sectores.Add("LA PEAÑA");
                sectores.Add("PROGRESO");
                sectores.Add("UZHCURRUMI");
                sectores.Add("CAÑAQUEMADA");
                sectores.Add("LA MATRIZ");
                sectores.Add("LA SUSAYA");
                sectores.Add("PIÑAS GRANDE");
                sectores.Add("PIÑAS");
                sectores.Add("CAPIRO (CAB. EN LA CAPILLA DE CAPIRO)");
                sectores.Add("LA BOCANA");
                sectores.Add("MOROMORO (CAB. EN EL VADO)");
                sectores.Add("PIEDRAS");
                sectores.Add("SAN ROQUE (AMBROSIO MALDONADO)");
                sectores.Add("SARACAY");
                sectores.Add("PORTOVELO");
                sectores.Add("CURTINCAPA");
                sectores.Add("MORALES");
                sectores.Add("SALATÍ");
                sectores.Add("SANTA ROSA");
                sectores.Add("PUERTO JELÍ");
                sectores.Add("BALNEARIO JAMBELÍ (SATÉLITE)");
                sectores.Add("JUMÓN (SATÉLITE)");
                sectores.Add("NUEVO SANTA ROSA");
                sectores.Add("SANTA ROSA");
                sectores.Add("BELLAVISTA");
                sectores.Add("JAMBELÍ");
                sectores.Add("LA AVANZADA");
                sectores.Add("SAN ANTONIO");
                sectores.Add("TORATA");
                sectores.Add("VICTORIA");
                sectores.Add("BELLAMARÍA");
                sectores.Add("ZARUMA");
                sectores.Add("ABAÑÍN");
                sectores.Add("ARCAPAMBA");
                sectores.Add("GUANAZÁN");
                sectores.Add("GUIZHAGUIÑA");
                sectores.Add("HUERTAS");
                sectores.Add("MALVAS");
                sectores.Add("MULUNCAY GRANDE");
                sectores.Add("SINSAO");
                sectores.Add("SALVIAS");
                sectores.Add("LA VICTORIA");
                sectores.Add("PLATANILLOS");
                sectores.Add("VALLE HERMOSO");
                sectores.Add("LA VICTORIA");
                sectores.Add("LA LIBERTAD");
                sectores.Add("EL PARAÍSO");
                sectores.Add("SAN ISIDRO");
                sectores.Add("BARTOLOMÉ RUIZ (CÉSAR FRANCO CARRIÓN)");
                sectores.Add("5 DE AGOSTO");
                sectores.Add("ESMERALDAS");
                sectores.Add("LUIS TELLO (LAS PALMAS)");
                sectores.Add("SIMÓN PLATA TORRES");
                sectores.Add("ESMERALDAS");
                sectores.Add("ATACAMES");
                sectores.Add("CAMARONES (CAB. EN SAN VICENTE)");
                sectores.Add("CRNEL. CARLOS CONCHA TORRES (CAB.EN HUELE)");
                sectores.Add("CHINCA");
                sectores.Add("CHONTADURO");
                sectores.Add("CHUMUNDÉ");
                sectores.Add("LAGARTO");
                sectores.Add("LA UNIÓN");
                sectores.Add("MAJUA");
                sectores.Add("MONTALVO (CAB. EN HORQUETA)");
                sectores.Add("RÍO VERDE");
                sectores.Add("ROCAFUERTE");
                sectores.Add("SAN MATEO");
                sectores.Add("SÚA (CAB. EN LA BOCANA)");
                sectores.Add("TABIAZO");
                sectores.Add("TACHINA");
                sectores.Add("TONCHIGÜE");
                sectores.Add("VUELTA LARGA");
                sectores.Add("VALDEZ (LIMONES)");
                sectores.Add("ANCHAYACU");
                sectores.Add("ATAHUALPA (CAB. EN CAMARONES)");
                sectores.Add("BORBÓN");
                sectores.Add("LA TOLA");
                sectores.Add("LUIS VARGAS TORRES (CAB. EN PLAYA DE ORO)");
                sectores.Add("MALDONADO");
                sectores.Add("PAMPANAL DE BOLÍVAR");
                sectores.Add("SAN FRANCISCO DE ONZOLE");
                sectores.Add("SANTO DOMINGO DE ONZOLE");
                sectores.Add("SELVA ALEGRE");
                sectores.Add("TELEMBÍ");
                sectores.Add("COLÓN ELOY DEL MARÍA");
                sectores.Add("SAN JOSÉ DE CAYAPAS");
                sectores.Add("TIMBIRÉ");
                sectores.Add("MUISNE");
                sectores.Add("BOLÍVAR");
                sectores.Add("DAULE");
                sectores.Add("GALERA");
                sectores.Add("QUINGUE (OLMEDO PERDOMO FRANCO)");
                sectores.Add("SALIMA");
                sectores.Add("SAN FRANCISCO");
                sectores.Add("SAN GREGORIO");
                sectores.Add("SAN JOSÉ DE CHAMANGA (CAB.EN CHAMANGA)");
                sectores.Add("ROSA ZÁRATE (QUININDÉ)");
                sectores.Add("CUBE");
                sectores.Add("CHURA (CHANCAMA) (CAB. EN EL YERBERO)");
                sectores.Add("MALIMPIA");
                sectores.Add("VICHE");
                sectores.Add("LA UNIÓN");
                sectores.Add("SAN LORENZO");
                sectores.Add("ALTO TAMBO (CAB. EN GUADUAL)");
                sectores.Add("ANCÓN (PICHANGAL) (CAB. EN PALMA REAL)");
                sectores.Add("CALDERÓN");
                sectores.Add("CARONDELET");
                sectores.Add("5 DE JUNIO (CAB. EN UIMBI)");
                sectores.Add("CONCEPCIÓN");
                sectores.Add("MATAJE (CAB. EN SANTANDER)");
                sectores.Add("SAN JAVIER DE CACHAVÍ (CAB. EN SAN JAVIER)");
                sectores.Add("SANTA RITA");
                sectores.Add("TAMBILLO");
                sectores.Add("TULULBÍ (CAB. EN RICAURTE)");
                sectores.Add("URBINA");
                sectores.Add("ATACAMES");
                sectores.Add("LA UNIÓN");
                sectores.Add("SÚA (CAB. EN LA BOCANA)");
                sectores.Add("TONCHIGÜE");
                sectores.Add("TONSUPA");
                sectores.Add("RIOVERDE");
                sectores.Add("CHONTADURO");
                sectores.Add("CHUMUNDÉ");
                sectores.Add("LAGARTO");
                sectores.Add("MONTALVO (CAB. EN HORQUETA)");
                sectores.Add("ROCAFUERTE");
                sectores.Add("LA CONCORDIA");
                sectores.Add("MONTERREY");
                sectores.Add("LA VILLEGAS");
                sectores.Add("PLAN PILOTO");
                sectores.Add("AYACUCHO");
                sectores.Add("BOLÍVAR (SAGRARIO)");
                sectores.Add("CARBO (CONCEPCIÓN)");
                sectores.Add("FEBRES CORDERO");
                sectores.Add("GARCÍA MORENO");
                sectores.Add("LETAMENDI");
                sectores.Add("NUEVE DE OCTUBRE");
                sectores.Add("OLMEDO (SAN ALEJO)");
                sectores.Add("ROCA");
                sectores.Add("ROCAFUERTE");
                sectores.Add("SUCRE");
                sectores.Add("TARQUI");
                sectores.Add("URDANETA");
                sectores.Add("XIMENA");
                sectores.Add("PASCUALES");
                sectores.Add("GUAYAQUIL");
                sectores.Add("CHONGÓN");
                sectores.Add("JUAN GÓMEZ RENDÓN (PROGRESO)");
                sectores.Add("MORRO");
                sectores.Add("PASCUALES");
                sectores.Add("PLAYAS (GRAL. VILLAMIL)");
                sectores.Add("POSORJA");
                sectores.Add("PUNÁ");
                sectores.Add("TENGUEL");
                sectores.Add("ALFREDO BAQUERIZO MORENO (JUJÁN)");
                sectores.Add("BALAO");
                sectores.Add("BALZAR");
                sectores.Add("COLIMES");
                sectores.Add("SAN JACINTO");
                sectores.Add("DAULE");
                sectores.Add("LA AURORA (SATÉLITE)");
                sectores.Add("BANIFE");
                sectores.Add("EMILIANO CAICEDO MARCOS");
                sectores.Add("MAGRO");
                sectores.Add("PADRE JUAN BAUTISTA AGUIRRE");
                sectores.Add("SANTA CLARA");
                sectores.Add("VICENTE PIEDRAHITA");
                sectores.Add("DAULE");
                sectores.Add("ISIDRO AYORA (SOLEDAD)");
                sectores.Add("JUAN BAUTISTA AGUIRRE (LOS TINTOS)");
                sectores.Add("LAUREL");
                sectores.Add("LIMONAL");
                sectores.Add("LOMAS DE SARGENTILLO");
                sectores.Add("LOS LOJAS (ENRIQUE BAQUERIZO MORENO)");
                sectores.Add("PIEDRAHITA (NOBOL)");
                sectores.Add("ELOY ALFARO (DURÁN)");
                sectores.Add("EL RECREO");
                sectores.Add("ELOY ALFARO (DURÁN)");
                sectores.Add("VELASCO IBARRA (EL EMPALME)");
                sectores.Add("GUAYAS (PUEBLO NUEVO)");
                sectores.Add("EL ROSARIO");
                sectores.Add("EL TRIUNFO");
                sectores.Add("MILAGRO");
                sectores.Add("CHOBO");
                sectores.Add("GENERAL ELIZALDE (BUCAY)");
                sectores.Add("MARISCAL SUCRE (HUAQUES)");
                sectores.Add("ROBERTO ASTUDILLO (CAB. EN CRUCE DE VENECIA)");
                sectores.Add("NARANJAL");
                sectores.Add("JESÚS MARÍA");
                sectores.Add("SAN CARLOS");
                sectores.Add("SANTA ROSA DE FLANDES");
                sectores.Add("TAURA");
                sectores.Add("NARANJITO");
                sectores.Add("PALESTINA");
                sectores.Add("PEDRO CARBO");
                sectores.Add("VALLE DE LA VIRGEN");
                sectores.Add("SABANILLA");
                sectores.Add("SAMBORONDÓN");
                sectores.Add("LA PUNTILLA (SATÉLITE)");
                sectores.Add("SAMBORONDÓN");
                sectores.Add("TARIFA");
                sectores.Add("SANTA LUCÍA");
                sectores.Add("BOCANA");
                sectores.Add("CANDILEJOS");
                sectores.Add("CENTRAL");
                sectores.Add("PARAÍSO");
                sectores.Add("SAN MATEO");
                sectores.Add("EL SALITRE (LAS RAMAS)");
                sectores.Add("GRAL. VERNAZA (DOS ESTEROS)");
                sectores.Add("LA VICTORIA (ÑAUZA)");
                sectores.Add("JUNQUILLAL");
                sectores.Add("SAN JACINTO DE YAGUACHI");
                sectores.Add("CRNEL. LORENZO DE GARAICOA (PEDREGAL)");
                sectores.Add("CRNEL. MARCELINO MARIDUEÑA (SAN CARLOS)");
                sectores.Add("GRAL. PEDRO J. MONTERO (BOLICHE)");
                sectores.Add("SIMÓN BOLÍVAR");
                sectores.Add("YAGUACHI VIEJO (CONE)");
                sectores.Add("VIRGEN DE FÁTIMA");
                sectores.Add("GENERAL VILLAMIL (PLAYAS)");
                sectores.Add("SIMÓN BOLÍVAR");
                sectores.Add("CRNEL.LORENZO DE GARAICOA (PEDREGAL)");
                sectores.Add("CORONEL MARCELINO MARIDUEÑA (SAN CARLOS)");
                sectores.Add("LOMAS DE SARGENTILLO");
                sectores.Add("ISIDRO AYORA (SOLEDAD)");
                sectores.Add("NARCISA DE JESÚS");
                sectores.Add("GENERAL ANTONIO ELIZALDE (BUCAY)");
                sectores.Add("ISIDRO AYORA");
                sectores.Add("CARANQUI");
                sectores.Add("GUAYAQUIL DE ALPACHACA");
                sectores.Add("SAGRARIO");
                sectores.Add("SAN FRANCISCO");
                sectores.Add("LA DOLOROSA DEL PRIORATO");
                sectores.Add("SAN MIGUEL DE IBARRA");
                sectores.Add("AMBUQUÍ");
                sectores.Add("ANGOCHAGUA");
                sectores.Add("CAROLINA");
                sectores.Add("LA ESPERANZA");
                sectores.Add("LITA");
                sectores.Add("SALINAS");
                sectores.Add("SAN ANTONIO");
                sectores.Add("ANDRADE MARÍN (LOURDES)");
                sectores.Add("ATUNTAQUI");
                sectores.Add("ATUNTAQUI");
                sectores.Add("IMBAYA (SAN LUIS DE COBUENDO)");
                sectores.Add("SAN FRANCISCO DE NATABUELA");
                sectores.Add("SAN JOSÉ DE CHALTURA");
                sectores.Add("SAN ROQUE");
                sectores.Add("SAGRARIO");
                sectores.Add("SAN FRANCISCO");
                sectores.Add("COTACACHI");
                sectores.Add("APUELA");
                sectores.Add("GARCÍA MORENO (LLURIMAGUA)");
                sectores.Add("IMANTAG");
                sectores.Add("PEÑAHERRERA");
                sectores.Add("PLAZA GUTIÉRREZ (CALVARIO)");
                sectores.Add("QUIROGA");
                sectores.Add("6 DE JULIO DE CUELLAJE (CAB. EN CUELLAJE)");
                sectores.Add("VACAS GALINDO (EL CHURO) (CAB.EN SAN MIGUEL ALTO");
                sectores.Add("JORDÁN");
                sectores.Add("SAN LUIS");
                sectores.Add("OTAVALO");
                sectores.Add("DR. MIGUEL EGAS CABEZAS (PEGUCHE)");
                sectores.Add("EUGENIO ESPEJO (CALPAQUÍ)");
                sectores.Add("GONZÁLEZ SUÁREZ");
                sectores.Add("PATAQUÍ");
                sectores.Add("SAN JOSÉ DE QUICHINCHE");
                sectores.Add("SAN JUAN DE ILUMÁN");
                sectores.Add("SAN PABLO");
                sectores.Add("SAN RAFAEL");
                sectores.Add("SELVA ALEGRE (CAB.EN SAN MIGUEL DE PAMPLONA)");
                sectores.Add("PIMAMPIRO");
                sectores.Add("CHUGÁ");
                sectores.Add("MARIANO ACOSTA");
                sectores.Add("SAN FRANCISCO DE SIGSIPAMBA");
                sectores.Add("URCUQUÍ CABECERA CANTONAL");
                sectores.Add("CAHUASQUÍ");
                sectores.Add("LA MERCED DE BUENOS AIRES");
                sectores.Add("PABLO ARENAS");
                sectores.Add("SAN BLAS");
                sectores.Add("TUMBABIRO");
                sectores.Add("EL SAGRARIO");
                sectores.Add("SAN SEBASTIÁN");
                sectores.Add("SUCRE");
                sectores.Add("VALLE");
                sectores.Add("LOJA");
                sectores.Add("CHANTACO");
                sectores.Add("CHUQUIRIBAMBA");
                sectores.Add("EL CISNE");
                sectores.Add("GUALEL");
                sectores.Add("JIMBILLA");
                sectores.Add("MALACATOS (VALLADOLID)");
                sectores.Add("SAN LUCAS");
                sectores.Add("SAN PEDRO DE VILCABAMBA");
                sectores.Add("SANTIAGO");
                sectores.Add("TAQUIL (MIGUEL RIOFRÍO)");
                sectores.Add("VILCABAMBA (VICTORIA)");
                sectores.Add("YANGANA (ARSENIO CASTILLO)");
                sectores.Add("QUINARA");
                sectores.Add("CARIAMANGA");
                sectores.Add("CHILE");
                sectores.Add("SAN VICENTE");
                sectores.Add("CARIAMANGA");
                sectores.Add("COLAISACA");
                sectores.Add("EL LUCERO");
                sectores.Add("UTUANA");
                sectores.Add("SANGUILLÍN");
                sectores.Add("CATAMAYO");
                sectores.Add("SAN JOSÉ");
                sectores.Add("CATAMAYO (LA TOMA)");
                sectores.Add("EL TAMBO");
                sectores.Add("GUAYQUICHUMA");
                sectores.Add("SAN PEDRO DE LA BENDITA");
                sectores.Add("ZAMBI");
                sectores.Add("CELICA");
                sectores.Add("CRUZPAMBA (CAB. EN CARLOS BUSTAMANTE)");
                sectores.Add("CHAQUINAL");
                sectores.Add("12 DE DICIEMBRE (CAB. EN ACHIOTES)");
                sectores.Add("PINDAL (FEDERICO PÁEZ)");
                sectores.Add("POZUL (SAN JUAN DE POZUL)");
                sectores.Add("SABANILLA");
                sectores.Add("TNTE. MAXIMILIANO RODRÍGUEZ LOAIZA");
                sectores.Add("CHAGUARPAMBA");
                sectores.Add("BUENAVISTA");
                sectores.Add("EL ROSARIO");
                sectores.Add("SANTA RUFINA");
                sectores.Add("AMARILLOS");
                sectores.Add("AMALUZA");
                sectores.Add("BELLAVISTA");
                sectores.Add("JIMBURA");
                sectores.Add("SANTA TERESITA");
                sectores.Add("27 DE ABRIL (CAB. EN LA NARANJA)");
                sectores.Add("EL INGENIO");
                sectores.Add("EL AIRO");
                sectores.Add("GONZANAMÁ");
                sectores.Add("CHANGAIMINA (LA LIBERTAD)");
                sectores.Add("FUNDOCHAMBA");
                sectores.Add("NAMBACOLA");
                sectores.Add("PURUNUMA (EGUIGUREN)");
                sectores.Add("QUILANGA (LA PAZ)");
                sectores.Add("SACAPALCA");
                sectores.Add("SAN ANTONIO DE LAS ARADAS (CAB. EN LAS ARADAS)");
                sectores.Add("GENERAL ELOY ALFARO (SAN SEBASTIÁN)");
                sectores.Add("MACARÁ (MANUEL ENRIQUE RENGEL SUQUILANDA)");
                sectores.Add("MACARÁ");
                sectores.Add("LARAMA");
                sectores.Add("LA VICTORIA");
                sectores.Add("SABIANGO (LA CAPILLA)");
                sectores.Add("CATACOCHA");
                sectores.Add("LOURDES");
                sectores.Add("CATACOCHA");
                sectores.Add("CANGONAMÁ");
                sectores.Add("GUACHANAMÁ");
                sectores.Add("LA TINGUE");
                sectores.Add("LAURO GUERRERO");
                sectores.Add("OLMEDO (SANTA BÁRBARA)");
                sectores.Add("ORIANGA");
                sectores.Add("SAN ANTONIO");
                sectores.Add("CASANGA");
                sectores.Add("YAMANA");
                sectores.Add("ALAMOR");
                sectores.Add("CIANO");
                sectores.Add("EL ARENAL");
                sectores.Add("EL LIMO (MARIANA DE JESÚS)");
                sectores.Add("MERCADILLO");
                sectores.Add("VICENTINO");
                sectores.Add("SARAGURO");
                sectores.Add("EL PARAÍSO DE CELÉN");
                sectores.Add("EL TABLÓN");
                sectores.Add("LLUZHAPA");
                sectores.Add("MANÚ");
                sectores.Add("SAN ANTONIO DE QUMBE (CUMBE)");
                sectores.Add("SAN PABLO DE TENTA");
                sectores.Add("SAN SEBASTIÁN DE YÚLUC");
                sectores.Add("SELVA ALEGRE");
                sectores.Add("URDANETA (PAQUISHAPA)");
                sectores.Add("SUMAYPAMBA");
                sectores.Add("SOZORANGA");
                sectores.Add("NUEVA FÁTIMA");
                sectores.Add("TACAMOROS");
                sectores.Add("ZAPOTILLO");
                sectores.Add("MANGAHURCO (CAZADEROS)");
                sectores.Add("GARZAREAL");
                sectores.Add("LIMONES");
                sectores.Add("PALETILLAS");
                sectores.Add("BOLASPAMBA");
                sectores.Add("PINDAL");
                sectores.Add("CHAQUINAL");
                sectores.Add("12 DE DICIEMBRE (CAB.EN ACHIOTES)");
                sectores.Add("MILAGROS");
                sectores.Add("QUILANGA");
                sectores.Add("FUNDOCHAMBA");
                sectores.Add("SAN ANTONIO DE LAS ARADAS (CAB. EN LAS ARADAS)");
                sectores.Add("OLMEDO");
                sectores.Add("LA TINGUE");
                sectores.Add("CLEMENTE BAQUERIZO");
                sectores.Add("DR. CAMILO PONCE");
                sectores.Add("BARREIRO");
                sectores.Add("EL SALTO");
                sectores.Add("BABAHOYO");
                sectores.Add("BARREIRO (SANTA RITA)");
                sectores.Add("CARACOL");
                sectores.Add("FEBRES CORDERO (LAS JUNTAS)");
                sectores.Add("PIMOCHA");
                sectores.Add("LA UNIÓN");
                sectores.Add("BABA");
                sectores.Add("GUARE");
                sectores.Add("ISLA DE BEJUCAL");
                sectores.Add("MONTALVO");
                sectores.Add("PUEBLOVIEJO");
                sectores.Add("PUERTO PECHICHE");
                sectores.Add("SAN JUAN");
                sectores.Add("QUEVEDO");
                sectores.Add("SAN CAMILO");
                sectores.Add("SAN JOSÉ");
                sectores.Add("GUAYACÁN");
                sectores.Add("NICOLÁS INFANTE DÍAZ");
                sectores.Add("SAN CRISTÓBAL");
                sectores.Add("SIETE DE OCTUBRE");
                sectores.Add("24 DE MAYO");
                sectores.Add("VENUS DEL RÍO QUEVEDO");
                sectores.Add("VIVA ALFARO");
                sectores.Add("QUEVEDO");
                sectores.Add("BUENA FÉ");
                sectores.Add("MOCACHE");
                sectores.Add("SAN CARLOS");
                sectores.Add("VALENCIA");
                sectores.Add("LA ESPERANZA");
                sectores.Add("CATARAMA");
                sectores.Add("RICAURTE");
                sectores.Add("10 DE NOVIEMBRE");
                sectores.Add("VENTANAS");
                sectores.Add("QUINSALOMA");
                sectores.Add("ZAPOTAL");
                sectores.Add("CHACARITA");
                sectores.Add("LOS ÁNGELES");
                sectores.Add("VINCES");
                sectores.Add("ANTONIO SOTOMAYOR (CAB. EN PLAYAS DE VINCES)");
                sectores.Add("PALENQUE");
                sectores.Add("PALENQUE");
                sectores.Add("SAN JACINTO DE BUENA FÉ");
                sectores.Add("7 DE AGOSTO");
                sectores.Add("11 DE OCTUBRE");
                sectores.Add("SAN JACINTO DE BUENA FÉ");
                sectores.Add("PATRICIA PILAR");
                sectores.Add("VALENCIA");
                sectores.Add("MOCACHE");
                sectores.Add("QUINSALOMA");
                sectores.Add("PORTOVIEJO");
                sectores.Add("12 DE MARZO");
                sectores.Add("COLÓN");
                sectores.Add("PICOAZÁ");
                sectores.Add("SAN PABLO");
                sectores.Add("ANDRÉS DE VERA");
                sectores.Add("FRANCISCO PACHECO");
                sectores.Add("18 DE OCTUBRE");
                sectores.Add("SIMÓN BOLÍVAR");
                sectores.Add("PORTOVIEJO");
                sectores.Add("ABDÓN CALDERÓN (SAN FRANCISCO)");
                sectores.Add("ALHAJUELA (BAJO GRANDE)");
                sectores.Add("CRUCITA");
                sectores.Add("PUEBLO NUEVO");
                sectores.Add("RIOCHICO (RÍO CHICO)");
                sectores.Add("SAN PLÁCIDO");
                sectores.Add("CHIRIJOS");
                sectores.Add("CALCETA");
                sectores.Add("MEMBRILLO");
                sectores.Add("QUIROGA");
                sectores.Add("CHONE");
                sectores.Add("SANTA RITA");
                sectores.Add("BOYACÁ");
                sectores.Add("CANUTO");
                sectores.Add("CONVENTO");
                sectores.Add("CHIBUNGA");
                sectores.Add("ELOY ALFARO");
                sectores.Add("RICAURTE");
                sectores.Add("SAN ANTONIO");
                sectores.Add("EL CARMEN");
                sectores.Add("4 DE DICIEMBRE");
                sectores.Add("EL CARMEN");
                sectores.Add("WILFRIDO LOOR MOREIRA (MAICITO)");
                sectores.Add("SAN PEDRO DE SUMA");
                sectores.Add("FLAVIO ALFARO");
                sectores.Add("SAN FRANCISCO DE NOVILLO (CAB. EN");
                sectores.Add("ZAPALLO");
                sectores.Add("DR. MIGUEL MORÁN LUCIO");
                sectores.Add("MANUEL INOCENCIO PARRALES Y GUALE");
                sectores.Add("SAN LORENZO DE JIPIJAPA");
                sectores.Add("JIPIJAPA");
                sectores.Add("AMÉRICA");
                sectores.Add("EL ANEGADO (CAB. EN ELOY ALFARO)");
                sectores.Add("JULCUY");
                sectores.Add("LA UNIÓN");
                sectores.Add("MACHALILLA");
                sectores.Add("MEMBRILLAL");
                sectores.Add("PEDRO PABLO GÓMEZ");
                sectores.Add("PUERTO DE CAYO");
                sectores.Add("PUERTO LÓPEZ");
                sectores.Add("JUNÍN");
                sectores.Add("LOS ESTEROS");
                sectores.Add("MANTA");
                sectores.Add("SAN MATEO");
                sectores.Add("TARQUI");
                sectores.Add("ELOY ALFARO");
                sectores.Add("MANTA");
                sectores.Add("SAN LORENZO");
                sectores.Add("SANTA MARIANITA (BOCA DE PACOCHE)");
                sectores.Add("ANIBAL SAN ANDRÉS");
                sectores.Add("MONTECRISTI");
                sectores.Add("EL COLORADO");
                sectores.Add("GENERAL ELOY ALFARO");
                sectores.Add("LEONIDAS PROAÑO");
                sectores.Add("MONTECRISTI");
                sectores.Add("JARAMIJÓ");
                sectores.Add("LA PILA");
                sectores.Add("PAJÁN");
                sectores.Add("CAMPOZANO (LA PALMA DE PAJÁN)");
                sectores.Add("CASCOL");
                sectores.Add("GUALE");
                sectores.Add("LASCANO");
                sectores.Add("PICHINCHA");
                sectores.Add("BARRAGANETE");
                sectores.Add("SAN SEBASTIÁN");
                sectores.Add("ROCAFUERTE");
                sectores.Add("SANTA ANA");
                sectores.Add("LODANA");
                sectores.Add("SANTA ANA DE VUELTA LARGA");
                sectores.Add("AYACUCHO");
                sectores.Add("HONORATO VÁSQUEZ (CAB. EN VÁSQUEZ)");
                sectores.Add("LA UNIÓN");
                sectores.Add("OLMEDO");
                sectores.Add("SAN PABLO (CAB. EN PUEBLO NUEVO)");
                sectores.Add("BAHÍA DE CARÁQUEZ");
                sectores.Add("LEONIDAS PLAZA GUTIÉRREZ");
                sectores.Add("BAHÍA DE CARÁQUEZ");
                sectores.Add("CANOA");
                sectores.Add("COJIMÍES");
                sectores.Add("CHARAPOTÓ");
                sectores.Add("10 DE AGOSTO");
                sectores.Add("JAMA");
                sectores.Add("PEDERNALES");
                sectores.Add("SAN ISIDRO");
                sectores.Add("SAN VICENTE");
                sectores.Add("TOSAGUA");
                sectores.Add("BACHILLERO");
                sectores.Add("ANGEL PEDRO GILER (LA ESTANCILLA)");
                sectores.Add("SUCRE");
                sectores.Add("BELLAVISTA");
                sectores.Add("NOBOA");
                sectores.Add("ARQ. SIXTO DURÁN BALLÉN");
                sectores.Add("PEDERNALES");
                sectores.Add("COJIMÍES");
                sectores.Add("10 DE AGOSTO");
                sectores.Add("ATAHUALPA");
                sectores.Add("OLMEDO");
                sectores.Add("PUERTO LÓPEZ");
                sectores.Add("MACHALILLA");
                sectores.Add("SALANGO");
                sectores.Add("JAMA");
                sectores.Add("JARAMIJÓ");
                sectores.Add("SAN VICENTE");
                sectores.Add("CANOA");
                sectores.Add("MACAS");
                sectores.Add("ALSHI (CAB. EN 9 DE OCTUBRE)");
                sectores.Add("CHIGUAZA");
                sectores.Add("GENERAL PROAÑO");
                sectores.Add("HUASAGA (CAB.EN WAMPUIK)");
                sectores.Add("MACUMA");
                sectores.Add("SAN ISIDRO");
                sectores.Add("SEVILLA DON BOSCO");
                sectores.Add("SINAÍ");
                sectores.Add("TAISHA");
                sectores.Add("ZUÑA (ZÚÑAC)");
                sectores.Add("TUUTINENTZA");
                sectores.Add("CUCHAENTZA");
                sectores.Add("SAN JOSÉ DE MORONA");
                sectores.Add("RÍO BLANCO");
                sectores.Add("GUALAQUIZA");
                sectores.Add("MERCEDES MOLINA");
                sectores.Add("GUALAQUIZA");
                sectores.Add("AMAZONAS (ROSARIO DE CUYES)");
                sectores.Add("BERMEJOS");
                sectores.Add("BOMBOIZA");
                sectores.Add("CHIGÜINDA");
                sectores.Add("EL ROSARIO");
                sectores.Add("NUEVA TARQUI");
                sectores.Add("SAN MIGUEL DE CUYES");
                sectores.Add("EL IDEAL");
                sectores.Add("GENERAL LEONIDAS PLAZA GUTIÉRREZ (LIMÓN)");
                sectores.Add("INDANZA");
                sectores.Add("PAN DE AZÚCAR");
                sectores.Add("SAN ANTONIO (CAB. EN SAN ANTONIO CENTRO");
                sectores.Add("SAN CARLOS DE LIMÓN (SAN CARLOS DEL");
                sectores.Add("SAN JUAN BOSCO");
                sectores.Add("SAN MIGUEL DE CONCHAY");
                sectores.Add("SANTA SUSANA DE CHIVIAZA (CAB. EN CHIVIAZA)");
                sectores.Add("YUNGANZA (CAB. EN EL ROSARIO)");
                sectores.Add("PALORA (METZERA)");
                sectores.Add("ARAPICOS");
                sectores.Add("CUMANDÁ (CAB. EN COLONIA AGRÍCOLA SEVILLA DEL ORO)");
                sectores.Add("HUAMBOYA");
                sectores.Add("SANGAY (CAB. EN NAYAMANACA)");
                sectores.Add("SANTIAGO DE MÉNDEZ");
                sectores.Add("COPAL");
                sectores.Add("CHUPIANZA");
                sectores.Add("PATUCA");
                sectores.Add("SAN LUIS DE EL ACHO (CAB. EN EL ACHO)");
                sectores.Add("SANTIAGO");
                sectores.Add("TAYUZA");
                sectores.Add("SAN FRANCISCO DE CHINIMBIMI");
                sectores.Add("SUCÚA");
                sectores.Add("ASUNCIÓN");
                sectores.Add("HUAMBI");
                sectores.Add("LOGROÑO");
                sectores.Add("YAUPI");
                sectores.Add("SANTA MARIANITA DE JESÚS");
                sectores.Add("HUAMBOYA");
                sectores.Add("CHIGUAZA");
                sectores.Add("PABLO SEXTO");
                sectores.Add("SAN JUAN BOSCO");
                sectores.Add("PAN DE AZÚCAR");
                sectores.Add("SAN CARLOS DE LIMÓN");
                sectores.Add("SAN JACINTO DE WAKAMBEIS");
                sectores.Add("SANTIAGO DE PANANZA");
                sectores.Add("TAISHA");
                sectores.Add("HUASAGA (CAB. EN WAMPUIK)");
                sectores.Add("MACUMA");
                sectores.Add("TUUTINENTZA");
                sectores.Add("PUMPUENTSA");
                sectores.Add("LOGROÑO");
                sectores.Add("YAUPI");
                sectores.Add("SHIMPIS");
                sectores.Add("PABLO SEXTO");
                sectores.Add("SANTIAGO");
                sectores.Add("SAN JOSÉ DE MORONA");
                sectores.Add("TENA");
                sectores.Add("AHUANO");
                sectores.Add("CARLOS JULIO AROSEMENA TOLA (ZATZA-YACU)");
                sectores.Add("CHONTAPUNTA");
                sectores.Add("PANO");
                sectores.Add("PUERTO MISAHUALLI");
                sectores.Add("PUERTO NAPO");
                sectores.Add("TÁLAG");
                sectores.Add("SAN JUAN DE MUYUNA");
                sectores.Add("ARCHIDONA");
                sectores.Add("AVILA");
                sectores.Add("COTUNDO");
                sectores.Add("LORETO");
                sectores.Add("SAN PABLO DE USHPAYACU");
                sectores.Add("PUERTO MURIALDO");
                sectores.Add("EL CHACO");
                sectores.Add("GONZALO DíAZ DE PINEDA (EL BOMBÓN)");
                sectores.Add("LINARES");
                sectores.Add("OYACACHI");
                sectores.Add("SANTA ROSA");
                sectores.Add("SARDINAS");
                sectores.Add("BAEZA");
                sectores.Add("COSANGA");
                sectores.Add("CUYUJA");
                sectores.Add("PAPALLACTA");
                sectores.Add("SAN FRANCISCO DE BORJA (VIRGILIO DÁVILA)");
                sectores.Add("SAN JOSÉ DEL PAYAMINO");
                sectores.Add("SUMACO");
                sectores.Add("CARLOS JULIO AROSEMENA TOLA");
                sectores.Add("PUYO");
                sectores.Add("ARAJUNO");
                sectores.Add("CANELOS");
                sectores.Add("CURARAY");
                sectores.Add("DIEZ DE AGOSTO");
                sectores.Add("FÁTIMA");
                sectores.Add("MONTALVO (ANDOAS)");
                sectores.Add("POMONA");
                sectores.Add("RÍO CORRIENTES");
                sectores.Add("RÍO TIGRE");
                sectores.Add("SANTA CLARA");
                sectores.Add("SARAYACU");
                sectores.Add("SIMÓN BOLÍVAR (CAB. EN MUSHULLACTA)");
                sectores.Add("TARQUI");
                sectores.Add("TENIENTE HUGO ORTIZ");
                sectores.Add("VERACRUZ (INDILLAMA) (CAB. EN INDILLAMA)");
                sectores.Add("EL TRIUNFO");
                sectores.Add("MERA");
                sectores.Add("MADRE TIERRA");
                sectores.Add("SHELL");
                sectores.Add("SANTA CLARA");
                sectores.Add("SAN JOSÉ");
                sectores.Add("ARAJUNO");
                sectores.Add("CURARAY");
                sectores.Add("BELISARIO QUEVEDO");
                sectores.Add("CARCELÉN");
                sectores.Add("CENTRO HISTÓRICO");
                sectores.Add("COCHAPAMBA");
                sectores.Add("COMITÉ DEL PUEBLO");
                sectores.Add("COTOCOLLAO");
                sectores.Add("CHILIBULO");
                sectores.Add("CHILLOGALLO");
                sectores.Add("CHIMBACALLE");
                sectores.Add("EL CONDADO");
                sectores.Add("GUAMANÍ");
                sectores.Add("IÑAQUITO");
                sectores.Add("ITCHIMBÍA");
                sectores.Add("JIPIJAPA");
                sectores.Add("KENNEDY");
                sectores.Add("LA ARGELIA");
                sectores.Add("LA CONCEPCIÓN");
                sectores.Add("LA ECUATORIANA");
                sectores.Add("LA FERROVIARIA");
                sectores.Add("LA LIBERTAD");
                sectores.Add("LA MAGDALENA");
                sectores.Add("LA MENA");
                sectores.Add("MARISCAL SUCRE");
                sectores.Add("PONCEANO");
                sectores.Add("PUENGASÍ");
                sectores.Add("QUITUMBE");
                sectores.Add("RUMIPAMBA");
                sectores.Add("SAN BARTOLO");
                sectores.Add("SAN ISIDRO DEL INCA");
                sectores.Add("SAN JUAN");
                sectores.Add("SOLANDA");
                sectores.Add("TURUBAMBA");
                sectores.Add("QUITO DISTRITO METROPOLITANO");
                sectores.Add("ALANGASÍ");
                sectores.Add("AMAGUAÑA");
                sectores.Add("ATAHUALPA");
                sectores.Add("CALACALÍ");
                sectores.Add("CALDERÓN");
                sectores.Add("CONOCOTO");
                sectores.Add("CUMBAYÁ");
                sectores.Add("CHAVEZPAMBA");
                sectores.Add("CHECA");
                sectores.Add("EL QUINCHE");
                sectores.Add("GUALEA");
                sectores.Add("GUANGOPOLO");
                sectores.Add("GUAYLLABAMBA");
                sectores.Add("LA MERCED");
                sectores.Add("LLANO CHICO");
                sectores.Add("LLOA");
                sectores.Add("MINDO");
                sectores.Add("NANEGAL");
                sectores.Add("NANEGALITO");
                sectores.Add("NAYÓN");
                sectores.Add("NONO");
                sectores.Add("PACTO");
                sectores.Add("PEDRO VICENTE MALDONADO");
                sectores.Add("PERUCHO");
                sectores.Add("PIFO");
                sectores.Add("PÍNTAG");
                sectores.Add("POMASQUI");
                sectores.Add("PUÉLLARO");
                sectores.Add("PUEMBO");
                sectores.Add("SAN ANTONIO");
                sectores.Add("SAN JOSÉ DE MINAS");
                sectores.Add("SAN MIGUEL DE LOS BANCOS");
                sectores.Add("TABABELA");
                sectores.Add("TUMBACO");
                sectores.Add("YARUQUÍ");
                sectores.Add("ZAMBIZA");
                sectores.Add("PUERTO QUITO");
                sectores.Add("AYORA");
                sectores.Add("CAYAMBE");
                sectores.Add("JUAN MONTALVO");
                sectores.Add("CAYAMBE");
                sectores.Add("ASCÁZUBI");
                sectores.Add("CANGAHUA");
                sectores.Add("OLMEDO (PESILLO)");
                sectores.Add("OTÓN");
                sectores.Add("SANTA ROSA DE CUZUBAMBA");
                sectores.Add("MACHACHI");
                sectores.Add("ALÓAG");
                sectores.Add("ALOASÍ");
                sectores.Add("CUTUGLAHUA");
                sectores.Add("EL CHAUPI");
                sectores.Add("MANUEL CORNEJO ASTORGA (TANDAPI)");
                sectores.Add("TAMBILLO");
                sectores.Add("UYUMBICHO");
                sectores.Add("TABACUNDO");
                sectores.Add("LA ESPERANZA");
                sectores.Add("MALCHINGUÍ");
                sectores.Add("TOCACHI");
                sectores.Add("TUPIGACHI");
                sectores.Add("SANGOLQUÍ");
                sectores.Add("SAN PEDRO DE TABOADA");
                sectores.Add("SAN RAFAEL");
                sectores.Add("SANGOLQUI");
                sectores.Add("COTOGCHOA");
                sectores.Add("RUMIPAMBA");
                sectores.Add("SAN MIGUEL DE LOS BANCOS");
                sectores.Add("MINDO");
                sectores.Add("PEDRO VICENTE MALDONADO");
                sectores.Add("PUERTO QUITO");
                sectores.Add("PEDRO VICENTE MALDONADO");
                sectores.Add("PUERTO QUITO");
                sectores.Add("ATOCHA – FICOA");
                sectores.Add("CELIANO MONGE");
                sectores.Add("HUACHI CHICO");
                sectores.Add("HUACHI LORETO");
                sectores.Add("LA MERCED");
                sectores.Add("LA PENÍNSULA");
                sectores.Add("MATRIZ");
                sectores.Add("PISHILATA");
                sectores.Add("SAN FRANCISCO");
                sectores.Add("AMBATO");
                sectores.Add("AMBATILLO");
                sectores.Add("ATAHUALPA (CHISALATA)");
                sectores.Add("AUGUSTO N. MARTÍNEZ (MUNDUGLEO)");
                sectores.Add("CONSTANTINO FERNÁNDEZ (CAB. EN CULLITAHUA)");
                sectores.Add("HUACHI GRANDE");
                sectores.Add("IZAMBA");
                sectores.Add("JUAN BENIGNO VELA");
                sectores.Add("MONTALVO");
                sectores.Add("PASA");
                sectores.Add("PICAIGUA");
                sectores.Add("PILAGÜÍN (PILAHÜÍN)");
                sectores.Add("QUISAPINCHA (QUIZAPINCHA)");
                sectores.Add("SAN BARTOLOMÉ DE PINLLOG");
                sectores.Add("SAN FERNANDO (PASA SAN FERNANDO)");
                sectores.Add("SANTA ROSA");
                sectores.Add("TOTORAS");
                sectores.Add("CUNCHIBAMBA");
                sectores.Add("UNAMUNCHO");
                sectores.Add("BAÑOS DE AGUA SANTA");
                sectores.Add("LLIGUA");
                sectores.Add("RÍO NEGRO");
                sectores.Add("RÍO VERDE");
                sectores.Add("ULBA");
                sectores.Add("CEVALLOS");
                sectores.Add("MOCHA");
                sectores.Add("PINGUILÍ");
                sectores.Add("PATATE");
                sectores.Add("EL TRIUNFO");
                sectores.Add("LOS ANDES (CAB. EN POATUG)");
                sectores.Add("SUCRE (CAB. EN SUCRE-PATATE URCU)");
                sectores.Add("QUERO");
                sectores.Add("RUMIPAMBA");
                sectores.Add("YANAYACU - MOCHAPATA (CAB. EN YANAYACU)");
                sectores.Add("PELILEO");
                sectores.Add("PELILEO GRANDE");
                sectores.Add("PELILEO");
                sectores.Add("BENÍTEZ (PACHANLICA)");
                sectores.Add("BOLÍVAR");
                sectores.Add("COTALÓ");
                sectores.Add("CHIQUICHA (CAB. EN CHIQUICHA GRANDE)");
                sectores.Add("EL ROSARIO (RUMICHACA)");
                sectores.Add("GARCÍA MORENO (CHUMAQUI)");
                sectores.Add("GUAMBALÓ (HUAMBALÓ)");
                sectores.Add("SALASACA");
                sectores.Add("CIUDAD NUEVA");
                sectores.Add("PÍLLARO");
                sectores.Add("PÍLLARO");
                sectores.Add("BAQUERIZO MORENO");
                sectores.Add("EMILIO MARÍA TERÁN (RUMIPAMBA)");
                sectores.Add("MARCOS ESPINEL (CHACATA)");
                sectores.Add("PRESIDENTE URBINA (CHAGRAPAMBA -PATZUCUL)");
                sectores.Add("SAN ANDRÉS");
                sectores.Add("SAN JOSÉ DE POALÓ");
                sectores.Add("SAN MIGUELITO");
                sectores.Add("TISALEO");
                sectores.Add("QUINCHICOTO");
                sectores.Add("EL LIMÓN");
                sectores.Add("ZAMORA");
                sectores.Add("ZAMORA");
                sectores.Add("CUMBARATZA");
                sectores.Add("GUADALUPE");
                sectores.Add("IMBANA (LA VICTORIA DE IMBANA)");
                sectores.Add("PAQUISHA");
                sectores.Add("SABANILLA");
                sectores.Add("TIMBARA");
                sectores.Add("ZUMBI");
                sectores.Add("SAN CARLOS DE LAS MINAS");
                sectores.Add("ZUMBA");
                sectores.Add("CHITO");
                sectores.Add("EL CHORRO");
                sectores.Add("EL PORVENIR DEL CARMEN");
                sectores.Add("LA CHONTA");
                sectores.Add("PALANDA");
                sectores.Add("PUCAPAMBA");
                sectores.Add("SAN FRANCISCO DEL VERGEL");
                sectores.Add("VALLADOLID");
                sectores.Add("SAN ANDRÉS");
                sectores.Add("GUAYZIMI");
                sectores.Add("ZURMI");
                sectores.Add("NUEVO PARAÍSO");
                sectores.Add("28 DE MAYO (SAN JOSÉ DE YACUAMBI)");
                sectores.Add("LA PAZ");
                sectores.Add("TUTUPALI");
                sectores.Add("YANTZAZA (YANZATZA)");
                sectores.Add("CHICAÑA");
                sectores.Add("EL PANGUI");
                sectores.Add("LOS ENCUENTROS");
                sectores.Add("EL PANGUI");
                sectores.Add("EL GUISME");
                sectores.Add("PACHICUTZA");
                sectores.Add("TUNDAYME");
                sectores.Add("ZUMBI");
                sectores.Add("PAQUISHA");
                sectores.Add("TRIUNFO-DORADO");
                sectores.Add("PANGUINTZA");
                sectores.Add("PALANDA");
                sectores.Add("EL PORVENIR DEL CARMEN");
                sectores.Add("SAN FRANCISCO DEL VERGEL");
                sectores.Add("VALLADOLID");
                sectores.Add("LA CANELA");
                sectores.Add("PAQUISHA");
                sectores.Add("BELLAVISTA");
                sectores.Add("NUEVO QUITO");
                sectores.Add("PUERTO BAQUERIZO MORENO");
                sectores.Add("EL PROGRESO");
                sectores.Add("A SANTA MARÍA (FLOREANA) (CAB. EN PTO. VELASCO IBARR");
                sectores.Add("PUERTO VILLAMIL");
                sectores.Add("TOMÁS DE BERLANGA (SANTO TOMÁS)");
                sectores.Add("PUERTO AYORA");
                sectores.Add("BELLAVISTA");
                sectores.Add("SANTA ROSA (INCLUYE LA ISLA BALTRA)");
                sectores.Add("NUEVA LOJA");
                sectores.Add("CUYABENO");
                sectores.Add("DURENO");
                sectores.Add("GENERAL FARFÁN");
                sectores.Add("TARAPOA");
                sectores.Add("EL ENO");
                sectores.Add("PACAYACU");
                sectores.Add("JAMBELÍ");
                sectores.Add("SANTA CECILIA");
                sectores.Add("AGUAS NEGRAS");
                sectores.Add("EL DORADO DE CASCALES");
                sectores.Add("EL REVENTADOR");
                sectores.Add("GONZALO PIZARRO");
                sectores.Add("LUMBAQUÍ");
                sectores.Add("PUERTO LIBRE");
                sectores.Add("SANTA ROSA DE SUCUMBÍOS");
                sectores.Add("PUERTO EL CARMEN DEL PUTUMAYO");
                sectores.Add("PALMA ROJA");
                sectores.Add("PUERTO BOLÍVAR (PUERTO MONTÚFAR)");
                sectores.Add("PUERTO RODRÍGUEZ");
                sectores.Add("SANTA ELENA");
                sectores.Add("SHUSHUFINDI");
                sectores.Add("LIMONCOCHA");
                sectores.Add("PAÑACOCHA");
                sectores.Add("SAN ROQUE (CAB. EN SAN VICENTE)");
                sectores.Add("SAN PEDRO DE LOS COFANES");
                sectores.Add("SIETE DE JULIO");
                sectores.Add("LA BONITA");
                sectores.Add("EL PLAYÓN DE SAN FRANCISCO");
                sectores.Add("LA SOFÍA");
                sectores.Add("ROSA FLORIDA");
                sectores.Add("SANTA BÁRBARA");
                sectores.Add("EL DORADO DE CASCALES");
                sectores.Add("SANTA ROSA DE SUCUMBÍOS");
                sectores.Add("SEVILLA");
                sectores.Add("TARAPOA");
                sectores.Add("CUYABENO");
                sectores.Add("AGUAS NEGRAS");
                sectores.Add("PUERTO FRANCISCO DE ORELLANA (EL COCA)");
                sectores.Add("DAYUMA");
                sectores.Add("TARACOA (NUEVA ESPERANZA: YUCA)");
                sectores.Add("ALEJANDRO LABAKA");
                sectores.Add("EL DORADO");
                sectores.Add("EL EDÉN");
                sectores.Add("GARCÍA MORENO");
                sectores.Add("INÉS ARANGO (CAB. EN WESTERN)");
                sectores.Add("LA BELLEZA");
                sectores.Add("NUEVO PARAÍSO (CAB. EN UNIÓN");
                sectores.Add("SAN JOSÉ DE GUAYUSA");
                sectores.Add("SAN LUIS DE ARMENIA");
                sectores.Add("TIPITINI");
                sectores.Add("NUEVO ROCAFUERTE");
                sectores.Add("CAPITÁN AUGUSTO RIVADENEYRA");
                sectores.Add("CONONACO");
                sectores.Add("SANTA MARÍA DE HUIRIRIMA");
                sectores.Add("TIPUTINI");
                sectores.Add("YASUNÍ");
                sectores.Add("LA JOYA DE LOS SACHAS");
                sectores.Add("ENOKANQUI");
                sectores.Add("POMPEYA");
                sectores.Add("SAN CARLOS");
                sectores.Add("SAN SEBASTIÁN DEL COCA");
                sectores.Add("LAGO SAN PEDRO");
                sectores.Add("RUMIPAMBA");
                sectores.Add("TRES DE NOVIEMBRE");
                sectores.Add("UNIÓN MILAGREÑA");
                sectores.Add("LORETO");
                sectores.Add("AVILA (CAB. EN HUIRUNO)");
                sectores.Add("PUERTO MURIALDO");
                sectores.Add("SAN JOSÉ DE PAYAMINO");
                sectores.Add("SAN JOSÉ DE DAHUANO");
                sectores.Add("SAN VICENTE DE HUATICOCHA");
                sectores.Add("ABRAHAM CALAZACÓN");
                sectores.Add("BOMBOLÍ");
                sectores.Add("CHIGUILPE");
                sectores.Add("RÍO TOACHI");
                sectores.Add("RÍO VERDE");
                sectores.Add("SANTO DOMINGO DE LOS COLORADOS");
                sectores.Add("ZARACAY");
                sectores.Add("SANTO DOMINGO DE LOS COLORADOS");
                sectores.Add("ALLURIQUÍN");
                sectores.Add("PUERTO LIMÓN");
                sectores.Add("LUZ DE AMÉRICA");
                sectores.Add("SAN JACINTO DEL BÚA");
                sectores.Add("VALLE HERMOSO");
                sectores.Add("EL ESFUERZO");
                sectores.Add("SANTA MARÍA DEL TOACHI");
                sectores.Add("BALLENITA");
                sectores.Add("SANTA ELENA");
                sectores.Add("SANTA ELENA");
                sectores.Add("ATAHUALPA");
                sectores.Add("COLONCHE");
                sectores.Add("CHANDUY");
                sectores.Add("MANGLARALTO");
                sectores.Add("SIMÓN BOLÍVAR (JULIO MORENO)");
                sectores.Add("SAN JOSÉ DE ANCÓN");
                sectores.Add("LA LIBERTAD");
                sectores.Add("CARLOS ESPINOZA LARREA");
                sectores.Add("GRAL. ALBERTO ENRÍQUEZ GALLO");
                sectores.Add("VICENTE ROCAFUERTE");
                sectores.Add("SANTA ROSA");
                sectores.Add("SALINAS");
                sectores.Add("ANCONCITO");
                sectores.Add("JOSÉ LUIS TAMAYO (MUEY)");
                sectores.Add("LAS GOLONDRINAS");
                sectores.Add("MANGA DEL CURA");
                sectores.Add("EL PIEDRERO");

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
        public ActionResult Edit(HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, HttpPostedFileBase img5, HttpPostedFileBase img6, [Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,nombre,apellido,ciudad,sector,calle,telefono,latitud,longitud,nombre_peluqueria,estado,fecha_nacimiento_,fecha_creacion_,capacidad_simultanea_")] AspNetUsers aspNetUsers, string sectores, string cantones)
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

            if (usuarioEdit.fotos_local == null)
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
                SERVICIOSController validar = new SERVICIOSController();
                string idUser = User.Identity.GetUserId();
                var estatus = validar.VerificarUser(idUser);
                if(estatus == "redes")
                {
                    return RedirectToAction("Create", "REDES_SOCIALES", new { estatus });
                }
                if (estatus == "horario")
                {
                    return RedirectToAction("Create", "HORARIOS", new { estatus });
                }
                else
                {
                    return RedirectToAction("Index", "SERVICIOS");
                }

            }
            return View(aspNetUsers);
        }

        [Authorize]
        // GET: AspNetUsers/Edit/5
        public ActionResult EditUserCliente()
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

                ViewBag.fecha_nacimiento = aspNetUsers.fecha_nacimiento_;
                return View(aspNetUsers);
            
        }

    // POST: AspNetUsers/Edit/5
    // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
    // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserCliente([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,nombre,apellido,ciudad,sector,calle,telefono,latitud,longitud,nombre_peluqueria,estado,fecha_nacimiento_,fecha_creacion_,capacidad_simultanea_")] AspNetUsers aspNetUsers){
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
            usuarioEdit.telefono = aspNetUsers.telefono;
            usuarioEdit.fecha_nacimiento_ = aspNetUsers.fecha_nacimiento_;

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

