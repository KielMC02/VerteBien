using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VerteBienV1.Models;

namespace VerteBienV1.Controllers
{
    [Authorize(Roles = "expres,preferencial,vip,administrador")]
    public class HORARIOSController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();

        // GET: HORARIOS
        [Authorize(Roles = "administrador")]
        public ActionResult Index()
        {
            var idUser = User.Identity.GetUserId();
            List<HORARIOS> verificar = new List<HORARIOS>();
            verificar = (from busqueda in db.HORARIOS where busqueda.id_usuario == idUser select busqueda).ToList();
            if (verificar.Count == 0)
            {
                ViewBag.comprobar = "True";
            }

            var hORARIOS = db.HORARIOS.Include(h => h.AspNetUsers);
            return View(hORARIOS.ToList());
        }
        [Authorize(Roles = "administrador")]
        // GET: HORARIOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HORARIOS hORARIOS = db.HORARIOS.Find(id);
            if (hORARIOS == null)
            {
                return HttpNotFound();
            }
            return View(hORARIOS);
        }

        // GET: HORARIOS/Create
        public ActionResult Create(string respuesta)
        {
            if(respuesta != null) 
            {
                ViewBag.respuesta = respuesta;
            }
            HORARIOS hORARIOS = new HORARIOS();
            var idUser = User.Identity.GetUserId();
            List<HORARIOS> verificar = new List<HORARIOS>();
            verificar = (from busqueda in db.HORARIOS where busqueda.id_usuario == idUser select busqueda).ToList();
            if (verificar.Count == 1)
            {
                return RedirectToAction("Edit", "HORARIOS");
            }
            //ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: HORARIOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_horario,id_usuario,semanales_inicio,semanales_cierre,sabados_inicio,sabados_cierre,domingo_inicio,domingo_cierre,estado")] HORARIOS hORARIOS, decimal? semanales_inicio, decimal? semanales_cierre, decimal? sabados_inicio, decimal? sabados_cierre, decimal? domingo_inicio, decimal? domingo_cierre)
        {
            hORARIOS.semanales_inicio = semanales_inicio;
            hORARIOS.semanales_cierre = semanales_cierre;
            hORARIOS.sabados_inicio = sabados_inicio;
            hORARIOS.sabados_cierre = sabados_cierre;
            hORARIOS.domingo_inicio = domingo_inicio;
            hORARIOS.domingo_cierre = domingo_cierre;

            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
            }
            hORARIOS.id_usuario = id;
            hORARIOS.estado = "activo";
            if (ModelState.IsValid)
            {
                db.HORARIOS.Add(hORARIOS);
                db.SaveChanges();
                SERVICIOSController validar = new SERVICIOSController();
                string idUser = User.Identity.GetUserId();
                string respuesta = validar.VerificarUser(idUser);
                if (respuesta == "redes")
                {
                    return RedirectToAction("Create", "REDES_SOCIALES", new { respuesta });
                }
                if (respuesta == "no fotos")
                {
                    return RedirectToAction("Edit", "AspNetUsers", new { respuesta });
                }
                return RedirectToAction("Index", "SERVICIOS");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", hORARIOS.id_usuario);
            return RedirectToAction("Index", "SERVICIOS");
        }

        // GET: HORARIOS/Edit/5
        public ActionResult Edit()
        {
            HORARIOS hORARIOS = new HORARIOS();
            var idUser = User.Identity.GetUserId();
            List<HORARIOS> verificar = new List<HORARIOS>();
            verificar =  (from busqueda in db.HORARIOS where busqueda.id_usuario == idUser select busqueda).ToList();
            if (verificar.Count == 1)
            { 
                foreach (var item in verificar) {
                    hORARIOS = db.HORARIOS.Find(item.id_horario);
                }
            
                if (hORARIOS == null)
                {
                    return HttpNotFound();
                }

                //Horarios
                #region
                List<decimal> semanalesInicio = new List<decimal>();
                semanalesInicio.Add(Convert.ToDecimal(7));
                semanalesInicio.Add(Convert.ToDecimal(7.50));
                semanalesInicio.Add(Convert.ToDecimal(8));
                semanalesInicio.Add(Convert.ToDecimal(8.50));
                semanalesInicio.Add(Convert.ToDecimal(9));
                semanalesInicio.Add(Convert.ToDecimal(9.50));
                semanalesInicio.Add(Convert.ToDecimal(10));
                semanalesInicio.Add(Convert.ToDecimal(10.50));
                semanalesInicio.Add(Convert.ToDecimal(11));
                semanalesInicio.Add(Convert.ToDecimal(11.50));
                semanalesInicio.Add(Convert.ToDecimal(12));
                semanalesInicio.Add(Convert.ToDecimal(12.50));
                semanalesInicio.Add(Convert.ToDecimal(13));
                semanalesInicio.Add(Convert.ToDecimal(13.50));
                semanalesInicio.Add(Convert.ToDecimal(14));
                semanalesInicio.Add(Convert.ToDecimal(14.50));
                semanalesInicio.Add(Convert.ToDecimal(15));
                semanalesInicio.Add(Convert.ToDecimal(15.50));
                semanalesInicio.Add(Convert.ToDecimal(16));
                semanalesInicio.Add(Convert.ToDecimal(16.50));
                semanalesInicio.Add(Convert.ToDecimal(17));
                semanalesInicio.Add(Convert.ToDecimal(17.50));
                semanalesInicio.Add(Convert.ToDecimal(18));
                semanalesInicio.Add(Convert.ToDecimal(18.50));
                semanalesInicio.Add(Convert.ToDecimal(19));
                semanalesInicio.Add(Convert.ToDecimal(19.50));
                semanalesInicio.Add(Convert.ToDecimal(20));
                semanalesInicio.Add(Convert.ToDecimal(20.50));
                semanalesInicio.Add(Convert.ToDecimal(21));
                semanalesInicio.Add(Convert.ToDecimal(21.50));
                semanalesInicio.Add(Convert.ToDecimal(22));
                semanalesInicio.Add(Convert.ToDecimal(22.50));
                semanalesInicio.Add(Convert.ToDecimal(23));
                semanalesInicio.Add(Convert.ToDecimal(23.50));

                List<decimal> semanalesCierre = new List<decimal>();
                semanalesCierre.Add(Convert.ToDecimal(7.50));
                semanalesCierre.Add(Convert.ToDecimal(8));
                semanalesCierre.Add(Convert.ToDecimal(8.50));
                semanalesCierre.Add(Convert.ToDecimal(9));
                semanalesCierre.Add(Convert.ToDecimal(9.50));
                semanalesCierre.Add(Convert.ToDecimal(10));
                semanalesCierre.Add(Convert.ToDecimal(10.50));
                semanalesCierre.Add(Convert.ToDecimal(11));
                semanalesCierre.Add(Convert.ToDecimal(11.50));
                semanalesCierre.Add(Convert.ToDecimal(12));
                semanalesCierre.Add(Convert.ToDecimal(12.50));
                semanalesCierre.Add(Convert.ToDecimal(13));
                semanalesCierre.Add(Convert.ToDecimal(13.50));
                semanalesCierre.Add(Convert.ToDecimal(14));
                semanalesCierre.Add(Convert.ToDecimal(14.50));
                semanalesCierre.Add(Convert.ToDecimal(15));
                semanalesCierre.Add(Convert.ToDecimal(15.50));
                semanalesCierre.Add(Convert.ToDecimal(16));
                semanalesCierre.Add(Convert.ToDecimal(16.50));
                semanalesCierre.Add(Convert.ToDecimal(17));
                semanalesCierre.Add(Convert.ToDecimal(17.50));
                semanalesCierre.Add(Convert.ToDecimal(18));
                semanalesCierre.Add(Convert.ToDecimal(18.50));
                semanalesCierre.Add(Convert.ToDecimal(19));
                semanalesCierre.Add(Convert.ToDecimal(19.50));
                semanalesCierre.Add(Convert.ToDecimal(20));
                semanalesCierre.Add(Convert.ToDecimal(20.50));
                semanalesCierre.Add(Convert.ToDecimal(21));
                semanalesCierre.Add(Convert.ToDecimal(21.50));
                semanalesCierre.Add(Convert.ToDecimal(22));
                semanalesCierre.Add(Convert.ToDecimal(22.50));
                semanalesCierre.Add(Convert.ToDecimal(23));
                semanalesCierre.Add(Convert.ToDecimal(23.50));

                List<decimal> sabadoInicio= new List<decimal>();
                sabadoInicio.Add(Convert.ToDecimal(7.50));
                sabadoInicio.Add(Convert.ToDecimal(8));
                sabadoInicio.Add(Convert.ToDecimal(8.50));
                sabadoInicio.Add(Convert.ToDecimal(9));
                sabadoInicio.Add(Convert.ToDecimal(9.50));
                sabadoInicio.Add(Convert.ToDecimal(10));
                sabadoInicio.Add(Convert.ToDecimal(10.50));
                sabadoInicio.Add(Convert.ToDecimal(11));
                sabadoInicio.Add(Convert.ToDecimal(11.50));
                sabadoInicio.Add(Convert.ToDecimal(12));
                sabadoInicio.Add(Convert.ToDecimal(12.50));
                sabadoInicio.Add(Convert.ToDecimal(13));
                sabadoInicio.Add(Convert.ToDecimal(13.50));
                sabadoInicio.Add(Convert.ToDecimal(14));
                sabadoInicio.Add(Convert.ToDecimal(14.50));
                sabadoInicio.Add(Convert.ToDecimal(15));
                sabadoInicio.Add(Convert.ToDecimal(15.50));
                sabadoInicio.Add(Convert.ToDecimal(16));
                sabadoInicio.Add(Convert.ToDecimal(16.50));
                sabadoInicio.Add(Convert.ToDecimal(17));
                sabadoInicio.Add(Convert.ToDecimal(17.50));
                sabadoInicio.Add(Convert.ToDecimal(18));
                sabadoInicio.Add(Convert.ToDecimal(18.50));
                sabadoInicio.Add(Convert.ToDecimal(19));
                sabadoInicio.Add(Convert.ToDecimal(19.50));
                sabadoInicio.Add(Convert.ToDecimal(20));
                sabadoInicio.Add(Convert.ToDecimal(20.50));
                sabadoInicio.Add(Convert.ToDecimal(21));
                sabadoInicio.Add(Convert.ToDecimal(21.50));
                sabadoInicio.Add(Convert.ToDecimal(22));
                sabadoInicio.Add(Convert.ToDecimal(22.50));
                sabadoInicio.Add(Convert.ToDecimal(23));
                sabadoInicio.Add(Convert.ToDecimal(23.50));

                List<decimal> sabadoCierre = new List<decimal>();
                sabadoCierre.Add(Convert.ToDecimal(7.50));
                sabadoCierre.Add(Convert.ToDecimal(8));
                sabadoCierre.Add(Convert.ToDecimal(8.50));
                sabadoCierre.Add(Convert.ToDecimal(9));
                sabadoCierre.Add(Convert.ToDecimal(9.50));
                sabadoCierre.Add(Convert.ToDecimal(10));
                sabadoCierre.Add(Convert.ToDecimal(10.50));
                sabadoCierre.Add(Convert.ToDecimal(11));
                sabadoCierre.Add(Convert.ToDecimal(11.50));
                sabadoCierre.Add(Convert.ToDecimal(12));
                sabadoCierre.Add(Convert.ToDecimal(12.50));
                sabadoCierre.Add(Convert.ToDecimal(13));
                sabadoCierre.Add(Convert.ToDecimal(13.50));
                sabadoCierre.Add(Convert.ToDecimal(14));
                sabadoCierre.Add(Convert.ToDecimal(14.50));
                sabadoCierre.Add(Convert.ToDecimal(15));
                sabadoCierre.Add(Convert.ToDecimal(15.50));
                sabadoCierre.Add(Convert.ToDecimal(16));
                sabadoCierre.Add(Convert.ToDecimal(16.50));
                sabadoCierre.Add(Convert.ToDecimal(17));
                sabadoCierre.Add(Convert.ToDecimal(17.50));
                sabadoCierre.Add(Convert.ToDecimal(18));
                sabadoCierre.Add(Convert.ToDecimal(18.50));
                sabadoCierre.Add(Convert.ToDecimal(19));
                sabadoCierre.Add(Convert.ToDecimal(19.50));
                sabadoCierre.Add(Convert.ToDecimal(20));
                sabadoCierre.Add(Convert.ToDecimal(20.50));
                sabadoCierre.Add(Convert.ToDecimal(21));
                sabadoCierre.Add(Convert.ToDecimal(21.50));
                sabadoCierre.Add(Convert.ToDecimal(22));
                sabadoCierre.Add(Convert.ToDecimal(22.50));
                sabadoCierre.Add(Convert.ToDecimal(23));
                sabadoCierre.Add(Convert.ToDecimal(23.50));

                List<decimal> domingoInicio = new List<decimal>();
                domingoInicio.Add(Convert.ToDecimal(7.50));
                domingoInicio.Add(Convert.ToDecimal(8));
                domingoInicio.Add(Convert.ToDecimal(8.50));
                domingoInicio.Add(Convert.ToDecimal(9));
                domingoInicio.Add(Convert.ToDecimal(9.50));
                domingoInicio.Add(Convert.ToDecimal(10));
                domingoInicio.Add(Convert.ToDecimal(10.50));
                domingoInicio.Add(Convert.ToDecimal(11));
                domingoInicio.Add(Convert.ToDecimal(11.50));
                domingoInicio.Add(Convert.ToDecimal(12));
                domingoInicio.Add(Convert.ToDecimal(12.50));
                domingoInicio.Add(Convert.ToDecimal(13));
                domingoInicio.Add(Convert.ToDecimal(13.50));
                domingoInicio.Add(Convert.ToDecimal(14));
                domingoInicio.Add(Convert.ToDecimal(14.50));
                domingoInicio.Add(Convert.ToDecimal(15));
                domingoInicio.Add(Convert.ToDecimal(15.50));
                domingoInicio.Add(Convert.ToDecimal(16));
                domingoInicio.Add(Convert.ToDecimal(16.50));
                domingoInicio.Add(Convert.ToDecimal(17));
                domingoInicio.Add(Convert.ToDecimal(17.50));
                domingoInicio.Add(Convert.ToDecimal(18));
                domingoInicio.Add(Convert.ToDecimal(18.50));
                domingoInicio.Add(Convert.ToDecimal(19));
                domingoInicio.Add(Convert.ToDecimal(19.50));
                domingoInicio.Add(Convert.ToDecimal(20));
                domingoInicio.Add(Convert.ToDecimal(20.50));
                domingoInicio.Add(Convert.ToDecimal(21));
                domingoInicio.Add(Convert.ToDecimal(21.50));
                domingoInicio.Add(Convert.ToDecimal(22));
                domingoInicio.Add(Convert.ToDecimal(22.50));
                domingoInicio.Add(Convert.ToDecimal(23));
                domingoInicio.Add(Convert.ToDecimal(23.50));

                List<decimal> domingoCierre = new List<decimal>();
                domingoCierre.Add(Convert.ToDecimal(7.50));
                domingoCierre.Add(Convert.ToDecimal(8));
                domingoCierre.Add(Convert.ToDecimal(8.50));
                domingoCierre.Add(Convert.ToDecimal(9));
                domingoCierre.Add(Convert.ToDecimal(9.50));
                domingoCierre.Add(Convert.ToDecimal(10));
                domingoCierre.Add(Convert.ToDecimal(10.50));
                domingoCierre.Add(Convert.ToDecimal(11));
                domingoCierre.Add(Convert.ToDecimal(11.50));
                domingoCierre.Add(Convert.ToDecimal(12));
                domingoCierre.Add(Convert.ToDecimal(12.50));
                domingoCierre.Add(Convert.ToDecimal(13));
                domingoCierre.Add(Convert.ToDecimal(13.50));
                domingoCierre.Add(Convert.ToDecimal(14));
                domingoCierre.Add(Convert.ToDecimal(14.50));
                domingoCierre.Add(Convert.ToDecimal(15));
                domingoCierre.Add(Convert.ToDecimal(15.50));
                domingoCierre.Add(Convert.ToDecimal(16));
                domingoCierre.Add(Convert.ToDecimal(16.50));
                domingoCierre.Add(Convert.ToDecimal(17));
                domingoCierre.Add(Convert.ToDecimal(17.50));
                domingoCierre.Add(Convert.ToDecimal(18));
                domingoCierre.Add(Convert.ToDecimal(18.50));
                domingoCierre.Add(Convert.ToDecimal(19));
                domingoCierre.Add(Convert.ToDecimal(19.50));
                domingoCierre.Add(Convert.ToDecimal(20));
                domingoCierre.Add(Convert.ToDecimal(20.50));
                domingoCierre.Add(Convert.ToDecimal(21));
                domingoCierre.Add(Convert.ToDecimal(21.50));
                domingoCierre.Add(Convert.ToDecimal(22));
                domingoCierre.Add(Convert.ToDecimal(22.50));
                domingoCierre.Add(Convert.ToDecimal(23));
                domingoCierre.Add(Convert.ToDecimal(23.50));


                #endregion
                ViewBag.semanalesInicio = new SelectList(semanalesInicio, hORARIOS.semanales_inicio);
                ViewBag.semanalesCierre = new SelectList(semanalesCierre, hORARIOS.semanales_cierre);
                ViewBag.sabadoInicio = new SelectList(sabadoInicio, hORARIOS.sabados_inicio);
                ViewBag.sabadoCierre = new SelectList(sabadoCierre, hORARIOS.sabados_cierre);
                ViewBag.domingoInicio = new SelectList(domingoInicio, hORARIOS.domingo_inicio);
                ViewBag.domingoCierre = new SelectList(domingoCierre, hORARIOS.domingo_cierre);
                ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", hORARIOS.id_usuario);

                return View(hORARIOS);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // POST: HORARIOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_horario,id_usuario,semanales_inicio,semanales_cierre,sabados_inicio,sabados_cierre,domingo_inicio,domingo_cierre,estado")] HORARIOS hORARIOS,decimal semanalesInicio, decimal semanalesCierre, decimal sabadoInicio, decimal sabadoCierre, decimal domingoInicio, decimal domingoCierre)
        {
            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;

            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
            }
            hORARIOS.id_usuario = id;
            hORARIOS.semanales_inicio = semanalesInicio;
            hORARIOS.semanales_cierre = semanalesCierre;
            hORARIOS.sabados_inicio = sabadoInicio;
            hORARIOS.sabados_cierre = sabadoCierre;
            hORARIOS.domingo_inicio = domingoInicio;
            hORARIOS.domingo_cierre = domingoCierre;
            hORARIOS.estado = "activo";
            if (ModelState.IsValid)
            {
                db.Entry(hORARIOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "SERVICIOS");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", hORARIOS.id_usuario);

            return View(hORARIOS);
        }
        [Authorize(Roles = "administrador")]
        // GET: HORARIOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HORARIOS hORARIOS = db.HORARIOS.Find(id);
            if (hORARIOS == null)
            {
                return HttpNotFound();
            }
            return View(hORARIOS);
        }
        [Authorize(Roles = "administrador")]
        // POST: HORARIOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HORARIOS hORARIOS = db.HORARIOS.Find(id);
            db.HORARIOS.Remove(hORARIOS);
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
