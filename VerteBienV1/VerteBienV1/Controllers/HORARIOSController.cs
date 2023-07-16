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
                var estatus = validar.VerificarUser(idUser);
                if (estatus == "redes")
                {
                    return RedirectToAction("Create", "REDES_SOCIALES", new { estatus });
                }
                if (estatus == "no fotos")
                {
                    return RedirectToAction("Edit", "AspNetUsers", new { estatus });
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
        public ActionResult Edit([Bind(Include = "id_horario,id_usuario,semanales_inicio,semanales_cierre,sabados_inicio,sabados_cierre,domingo_inicio,domingo_cierre,estado")] HORARIOS hORARIOS)
        {
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
