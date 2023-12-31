using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VerteBienV1.Models;

namespace VerteBienV1.Controllers
{
    public class SERVICIOSController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();

        //Metodo para borrar imagenes reemplazadas
        public string borrarImagen(string imagenParaBorrar)
        {
            string confirmado;
            String imagenBorra = Path.Combine(HttpContext.Server.MapPath("~/imagenes_servicios/"), imagenParaBorrar);
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
        //Metodo que valida suspension del usuario
        //public string ValidarEstado()
        //{

        //    //var estaAutenticado = User.Identity.IsAuthenticated;
        //    var idUsuario = User.Identity.GetUserId();


        //    AspNetUsers usuarioLogueado = new AspNetUsers();
        //    usuarioLogueado = db.AspNetUsers.Find(idUsuario);

        //    if (usuarioLogueado.estado == "suspendido")
        //    {

        //        return "suspendido";
        //    }
        //    if (usuarioLogueado.estado == "new")
        //    {

        //        return "new";
        //    }
        //    else 
        //    {

        //        return "activo";
        //    }
        //}
        //Metodo que valida si el usuario tienen horarios y redes creados.
        public string VerificarUser(string idUser)
        {
            //var idUser = User.Identity.GetUserId();

            //Horarios
            List<HORARIOS> verificarHorario = new List<HORARIOS>();
            verificarHorario = (from busqueda in db.HORARIOS where busqueda.id_usuario == idUser select busqueda).ToList();

            //Redes sociales
            List<REDES_SOCIALES> verificarRedes = new List<REDES_SOCIALES>();
            verificarRedes = (from busqueda in db.REDES_SOCIALES where busqueda.id_usuario == idUser select busqueda).ToList();

            AspNetUsers usuarioLogueado = new AspNetUsers();
            usuarioLogueado = db.AspNetUsers.Find(idUser);

            if (verificarHorario.Count == 0)
            {
                return "horario";
            }
            if (verificarRedes.Count == 0)
            {
                return "redes";
            }
            if (usuarioLogueado.fotos_local == null)
            {
                return "no fotos";
            }

            if (usuarioLogueado.estado == "suspendido")
            {

                return "suspendido";
            }
            if (usuarioLogueado.estado == "new")
            {

                return "new";
            }
            else
            {

                return "activo";
            }
        }


        // GET: SERVICIOS
        //[OutputCache(Duration = 3600, VaryByParam = canton)]
        public ActionResult Index(string canton, string sector, string servicio, string puntuacion)
        {
            //Variable a utilizar en caso de que uno de los parametros sea un string vacio
            var nulo = DBNull.Value;
            //Poner paremetros
            if(sector == "")
            {
                sector = null;
            }
            if (servicio == "")
            {
                servicio = null;
            }
            if (puntuacion == "")
            {
                puntuacion = null;
            }
            ////Lista que guarda el resultado de la Busqueda
            List<SERVICIOS> resultadoBusqueda = new List<SERVICIOS>();
            string alertVerificacion = "Cargada inicial";
            //Cuando se utilizan todos los parametros.
            if( canton != "" && canton != null && canton != "Seleccione un cantón")
             {
                alertVerificacion = "se busco";
                resultadoBusqueda = db.Database.SqlQuery<SERVICIOS>("filtrar @ciudad, @sector, @puntuacion, @dato", new SqlParameter("@ciudad", Convert.ToString(canton)), new SqlParameter("@sector", sector == null ? (object)nulo : sector), new SqlParameter("@puntuacion", puntuacion == null ? (object)nulo : puntuacion), new SqlParameter("@dato", servicio == null ? (object)nulo : servicio)).ToList();


            }
            if (alertVerificacion == "se busco" && resultadoBusqueda.Count == 0)
            {
                ViewBag.noResult = "Sin resultados";
            }

            //Lista completa de servicios sin filtrado
            //var sERVICIOS = db.SERVICIOS.Include(s => s.AspNetUsers).Include(s => s.CATEGORIAS_SERVICIOS);
            //resultadoBusqueda = db.Database.SqlQuery<SERVICIOS>("SP_SELECT @dato", new SqlParameter("@dato",servicio)).ToList();

            return View(resultadoBusqueda);
        }
        public ActionResult TodosLosServicios(string servicio)
        {

            //Lista que guarda el resultado de la Busqueda
            List<SERVICIOS> resultadoBusqueda = new List<SERVICIOS>();
            if (servicio != "" || servicio != null)
            {
                resultadoBusqueda = (from Nbusqueda in db.SERVICIOS where Nbusqueda.nombre_servicio.Contains(servicio) select Nbusqueda).ToList();

            }
            if(servicio == "" || servicio == null)
            { 
            //Lista completa de servicios sin filtrado
             resultadoBusqueda = db.SERVICIOS.Include(s => s.AspNetUsers).Include(s => s.CATEGORIAS_SERVICIOS).ToList();
            }

            return View(resultadoBusqueda);
        }

        [Authorize(Roles = "expres,preferencial,vip,administrador")]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        // GET: SERVICIOS POR USUARIOS
        public ActionResult misServicios(string servicio)
        {
            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
            }
            List<SERVICIOS> resultadoBusqueda = new List<SERVICIOS>();
            if (servicio != "" || servicio != null)
            {
                resultadoBusqueda = (from Nbusqueda in db.SERVICIOS where Nbusqueda.id_usuario == id && Nbusqueda.nombre_servicio.Contains(servicio) select Nbusqueda).ToList();
            }
            if((servicio == "" || servicio == null)) 
            {
                //Lista que guarda el resultado de la Busqueda
                resultadoBusqueda = (from Nbusqueda in db.SERVICIOS where Nbusqueda.id_usuario == id select Nbusqueda).ToList();
            }





            return View(resultadoBusqueda);
        }

        public ActionResult Tendencias() 
        {
            var serviciosTendencia = db.Database.SqlQuery<SERVICIOS>("tendencia_servicios").ToList();
            //var peluqueriaTendencia = db.Database.SqlQuery<AspNetUsers>("tendencia_peluquerias").ToList();

            return View();
        }
        // GET: SERVICIOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SERVICIOS sERVICIOS = db.SERVICIOS.Find(id);
            if (sERVICIOS == null)
            {
                return HttpNotFound();
            }

            //Separamos los nombres de las imagenes y guardamos en una lista
            List<String> imagenes = (sERVICIOS.imagenes.Split(';')).ToList();
            //Enviamos la lista a la vista
            ViewData["imagenes_s"] = imagenes;
            //Enviamos ID peluqueria
            ViewBag.id = sERVICIOS.id_usuario;
            //Enviamos nombre
            AspNetUsers nombre = db.AspNetUsers.Find(sERVICIOS.id_usuario);
            ViewBag.nombre = nombre.nombre_peluqueria; 
            //Promedio 
            List<PUNTUACION_SERVICIOS> puntuacion = (from puntuacionS in db.PUNTUACION_SERVICIOS where puntuacionS.id_servicio == id select puntuacionS).ToList();
            decimal sumadas = new decimal();
            int contador = 0;
            decimal promedio = new decimal();
            int cantidad = puntuacion.Count();
            if (cantidad > 0)
            {
                foreach (var item in puntuacion)
                {
                    sumadas += Convert.ToDecimal(item.estrellas);
                    contador = contador + 1;
                }

                promedio = sumadas / contador;
            }
            ViewBag.promedio = promedio;
            ViewBag.idservicio = sERVICIOS.id_servicio;

            return View(sERVICIOS);
        }

        [Authorize(Roles = "expres,preferencial,vip,administrador")]
        // GET: SERVICIOS/Create
        public ActionResult Create()
        {
            var idUser = User.Identity.GetUserId();
            string respuesta = VerificarUser(idUser);
            if (respuesta == "horario") 
            {
                ViewBag.respuesta = respuesta;
                return RedirectToAction("Create", "HORARIOS",new { respuesta });
            }
            if (respuesta == "redes")
            {
                ViewBag.respuesta = respuesta;
                return RedirectToAction("Create", "REDES_SOCIALES", new { respuesta });
            }
            if (respuesta == "no fotos")
            {
                ViewBag.respuesta = respuesta;
                return RedirectToAction("Edit", "AspNetUsers", new { respuesta });
            }
            if (respuesta == "activo")
            {
                ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
                ViewBag.id_categoria = new SelectList(db.CATEGORIAS_SERVICIOS, "id_categoria_servicio", "nombre_categoria");
                return View();
            }
            else
            {
                return RedirectToAction("pagoRequerido", "SUSCRIPCIONs", new { respuesta });
            }
        }

        // POST: SERVICIOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, HttpPostedFileBase img5, HttpPostedFileBase img6, HttpPostedFileBase img7,HttpPostedFileBase img8, HttpPostedFileBase img9,[Bind(Include = "id_servicio,id_usuario,id_categoria,nombre_servicio,descripcion,precio_servicio,tiempo,imagenes,estado")] SERVICIOS sERVICIOS, decimal duracion)
        {
            var decimaConvertido = Convert.ToDecimal(duracion);

            sERVICIOS.tiempo = decimaConvertido;
            //Variable para generar numeros ramdon para el nombre de las imagenes.
            Random rnd = new Random();


            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado) {
                id = User.Identity.GetUserId();
            }
            sERVICIOS.id_usuario = id;
            sERVICIOS.estado = "activo";
            //Guardado de las imagenes
            #region
            List<String> ruta_imagenes = new List<string>();
            //Establecemos la ruta donde se guardaran las imagenes
            if (img1 != null)
            {
                
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img1.FileName;
                String ruta_img1 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img1.SaveAs(ruta_img1);
                sERVICIOS.imagenes = nuevaImagen;
            }
            if (img2 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img2.FileName;
                String ruta_img2 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img2.SaveAs(ruta_img2);
                sERVICIOS.imagenes += ";" + nuevaImagen; 
            }
            if (img3 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img3.FileName;
                String ruta_img3 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img3.SaveAs(ruta_img3);
                sERVICIOS.imagenes += ";" + nuevaImagen;
            }
            if (img4 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img4.FileName;
                String ruta_img4 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img4.SaveAs(ruta_img4);
                sERVICIOS.imagenes += ";" + nuevaImagen;
            }
            if (img5 != null) 
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img5.FileName;
                String ruta_img5 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img5.SaveAs(ruta_img5);
                sERVICIOS.imagenes += ";" + nuevaImagen;

            }
            if (img6 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img6.FileName;
                String ruta_img6 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img6.SaveAs(ruta_img6);
                sERVICIOS.imagenes += ";" + nuevaImagen;
            }
            if (img7 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img7.FileName;
                String ruta_img7 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img7.SaveAs(ruta_img7);
                sERVICIOS.imagenes += ";" + nuevaImagen;
            }
            if (img8 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img8.FileName;
                String ruta_img8 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img8.SaveAs(ruta_img8);
                sERVICIOS.imagenes += ";" + nuevaImagen;
            }
            if (img9 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img9.FileName;
                String ruta_img9 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img9.SaveAs(ruta_img9);
                sERVICIOS.imagenes += ";" + nuevaImagen;
            }
            #endregion
            //Fin guardado de imagen
            if (ModelState.IsValid)
            {
                db.SERVICIOS.Add(sERVICIOS);
                db.SaveChanges();
                return RedirectToAction("misServicios");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sERVICIOS.id_usuario);
            ViewBag.id_categoria = new SelectList(db.CATEGORIAS_SERVICIOS, "id_categoria_servicio", "nombre_categoria", sERVICIOS.id_categoria);
            return View(sERVICIOS);
        }
        [Authorize(Roles = "expres,preferencial,vip,administrador")]
        // GET: SERVICIOS/Edit/5
        public ActionResult Edit(int? id)
        {
            var idUser = User.Identity.GetUserId();
            var estatus = VerificarUser(idUser);
            if (estatus == "activo" || estatus == "no fotos")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var idPeluqueria = User.Identity.GetUserId();
                SERVICIOS sERVICIOS = db.SERVICIOS.Find(id);
                if (sERVICIOS == null || sERVICIOS.id_usuario != idPeluqueria)
                {
                    return HttpNotFound();
                }
                ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sERVICIOS.id_usuario);
                ViewBag.id_categoria = new SelectList(db.CATEGORIAS_SERVICIOS, "id_categoria_servicio", "nombre_categoria", sERVICIOS.id_categoria);
                //Estados del servicio
                List<String> estado = new List<string>();
                estado.Add("activo");
                estado.Add("inactivo");
                //Duracion Servicio
                List<decimal> duracion = new List<decimal>();
                duracion.Add(Convert.ToDecimal(0.50));
                duracion.Add(Convert.ToDecimal(1.00));
                duracion.Add(Convert.ToDecimal(1.50));
                duracion.Add(Convert.ToDecimal(2.00));

                ViewBag.duraciones = new SelectList(duracion, sERVICIOS.tiempo); 
                ViewBag.estados = new SelectList(estado, sERVICIOS.estado);
                //Separamos los nombres de las imagenes y guardamos en una lista
                List<String> imagenes = (sERVICIOS.imagenes.Split(';')).ToList();
                //Enviamos la lista a la vista
                ViewData["imagenes_s"] = imagenes;


                return View(sERVICIOS);
            }
            else
            {
                ViewBag.respuesta = estatus;
                return RedirectToAction("pagoRequerido", "SUSCRIPCIONs");
            }
        }
        [Authorize(Roles = "expres,preferencial,vip,administrador")]
        // POST: SERVICIOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_servicio,id_usuario,id_categoria,nombre_servicio,descripcion,precio_servicio,tiempo,imagenes,estado")] SERVICIOS sERVICIOS,HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, HttpPostedFileBase img5, HttpPostedFileBase img6, HttpPostedFileBase img7, HttpPostedFileBase img8, HttpPostedFileBase img9, string estados, decimal duraciones)
        {
            //Datos Adicionales
            var id = User.Identity.GetUserId();
            sERVICIOS.id_usuario = id;
            sERVICIOS.estado = estados;
            sERVICIOS.tiempo = duraciones;

            //Variable para generar numeros ramdon para el nombre de las imagenes.
            Random rnd = new Random();

            //Separamos los nombres de las imagenes y guardamos en una lista
            List<String> imagenes = (sERVICIOS.imagenes.Split(';')).ToList();

            //Establecemos la ruta donde se guardaran las imagenes
            if (img1 != null)
            {
                int numero = rnd.Next(52);
                Convert.ToString(numero).Trim();
                var nuevaImagen = numero + sERVICIOS.id_usuario + img1.FileName;
                sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[0], nuevaImagen);
                ViewBag.comprobar = sERVICIOS.imagenes;
                String ruta_img1 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                img1.SaveAs(ruta_img1);
                var borrar = borrarImagen(imagenes[0]);
            }
            if (img2 != null)
            {
                if(imagenes.Count < 2)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img2.FileName;
                    String ruta_img2 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img2.SaveAs(ruta_img2);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else 
                { 
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img2.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[1], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img2 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
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
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img3.FileName;
                    String ruta_img3 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img3.SaveAs(ruta_img3);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else 
                { 
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img3.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[2], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img3 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
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
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img4.FileName;
                    String ruta_img4 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img4.SaveAs(ruta_img4);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else 
                { 
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img4.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[3], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img4 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
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
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img5.FileName;
                    String ruta_img5 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img5.SaveAs(ruta_img5);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else 
                { 
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img5.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[4], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img5 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
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
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img6.FileName;
                    String ruta_img6 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img6.SaveAs(ruta_img6);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else 
                { 
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img6.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[5], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img6 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img6.SaveAs(ruta_img6);
                    var borrar = borrarImagen(imagenes[5]);
                }
            }
            if (img7 != null)
            {
                if (imagenes.Count < 7)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img7.FileName;
                    String ruta_img7 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img7.SaveAs(ruta_img7);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img7.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[6], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img7 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img7.SaveAs(ruta_img7);
                    var borrar = borrarImagen(imagenes[6]);
                }
            }
            if (img8 != null)
            {
                if (imagenes.Count < 8)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img8.FileName;
                    String ruta_img8 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img8.SaveAs(ruta_img8);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img8.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[7], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img8 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img8.SaveAs(ruta_img8);
                    var borrar = borrarImagen(imagenes[7]);
                }
            }
            if (img9 != null)
            {
                if (imagenes.Count < 9)
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img9.FileName;
                    String ruta_img9 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img9.SaveAs(ruta_img9);
                    sERVICIOS.imagenes += ";" + nuevaImagen;
                }
                else
                {
                    int numero = rnd.Next(52);
                    Convert.ToString(numero).Trim();
                    var nuevaImagen = numero + sERVICIOS.id_usuario + img9.FileName;
                    sERVICIOS.imagenes = sERVICIOS.imagenes.Replace(imagenes[8], nuevaImagen);
                    ViewBag.comprobar = sERVICIOS.imagenes;
                    String ruta_img9 = Server.MapPath("~/imagenes_servicios/") + nuevaImagen;
                    img9.SaveAs(ruta_img9);
                    var borrar = borrarImagen(imagenes[8]);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(sERVICIOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("misServicios");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sERVICIOS.id_usuario);
            ViewBag.id_categoria = new SelectList(db.CATEGORIAS_SERVICIOS, "id_categoria_servicio", "nombre_categoria", sERVICIOS.id_categoria);
            return View(sERVICIOS);
        }
        [Authorize(Roles = "administrador")]
        // GET: SERVICIOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SERVICIOS sERVICIOS = db.SERVICIOS.Find(id);
            if (sERVICIOS == null)
            {
                return HttpNotFound();
            }
            return View(sERVICIOS);
        }

        // POST: SERVICIOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SERVICIOS sERVICIOS = db.SERVICIOS.Find(id);
            db.SERVICIOS.Remove(sERVICIOS);
            db.SaveChanges();
            return RedirectToAction("misServicios");
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
