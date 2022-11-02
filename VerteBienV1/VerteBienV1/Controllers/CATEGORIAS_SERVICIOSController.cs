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
    [Authorize(Roles = "administrador")]
    public class CATEGORIAS_SERVICIOSController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();

        // GET: CATEGORIAS_SERVICIOS
        public ActionResult Index()
        {
            return View(db.CATEGORIAS_SERVICIOS.ToList());
        }

        // GET: CATEGORIAS_SERVICIOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORIAS_SERVICIOS cATEGORIAS_SERVICIOS = db.CATEGORIAS_SERVICIOS.Find(id);
            if (cATEGORIAS_SERVICIOS == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORIAS_SERVICIOS);
        }

        // GET: CATEGORIAS_SERVICIOS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CATEGORIAS_SERVICIOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_categoria_servicio,nombre_categoria,descripcion,estado")] CATEGORIAS_SERVICIOS cATEGORIAS_SERVICIOS)
        {
            cATEGORIAS_SERVICIOS.estado = "activo";
            if (ModelState.IsValid)
            {
                db.CATEGORIAS_SERVICIOS.Add(cATEGORIAS_SERVICIOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cATEGORIAS_SERVICIOS);
        }

        // GET: CATEGORIAS_SERVICIOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORIAS_SERVICIOS cATEGORIAS_SERVICIOS = db.CATEGORIAS_SERVICIOS.Find(id);
            if (cATEGORIAS_SERVICIOS == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORIAS_SERVICIOS);
        }

        // POST: CATEGORIAS_SERVICIOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_categoria_servicio,nombre_categoria,descripcion,estado")] CATEGORIAS_SERVICIOS cATEGORIAS_SERVICIOS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cATEGORIAS_SERVICIOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cATEGORIAS_SERVICIOS);
        }

        // GET: CATEGORIAS_SERVICIOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CATEGORIAS_SERVICIOS cATEGORIAS_SERVICIOS = db.CATEGORIAS_SERVICIOS.Find(id);
            if (cATEGORIAS_SERVICIOS == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORIAS_SERVICIOS);
        }

        // POST: CATEGORIAS_SERVICIOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CATEGORIAS_SERVICIOS cATEGORIAS_SERVICIOS = db.CATEGORIAS_SERVICIOS.Find(id);
            db.CATEGORIAS_SERVICIOS.Remove(cATEGORIAS_SERVICIOS);
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
