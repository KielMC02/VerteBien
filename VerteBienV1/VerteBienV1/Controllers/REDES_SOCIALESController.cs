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
    [Authorize(Roles ="expres,preferencial,vip,administrador")]
    public class REDES_SOCIALESController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();

        // GET: REDES_SOCIALES
        public ActionResult Index()
        {
            ViewBag.comprobar = "";
            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
            }

            List<REDES_SOCIALES> verificar = new List<REDES_SOCIALES>();
            verificar = (from busqueda in db.REDES_SOCIALES where busqueda.id_usuario == id select busqueda).ToList();
            if (verificar.Count == 0)
            {
                ViewBag.comprobar = "True";
            }

            return View(verificar);
            //var rEDES_SOCIALES = db.REDES_SOCIALES.Include(r => r.AspNetUsers);
            //return View(rEDES_SOCIALES.ToList());
        }

        // GET: REDES_SOCIALES/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REDES_SOCIALES rEDES_SOCIALES = db.REDES_SOCIALES.Find(id);
            if (rEDES_SOCIALES == null)
            {
                return HttpNotFound();
            }
            return View(rEDES_SOCIALES);
        }

        // GET: REDES_SOCIALES/Create
        public ActionResult Create()
        {
            var id = User.Identity.GetUserId();
            List<REDES_SOCIALES> verificar = new List<REDES_SOCIALES>();
            verificar = (from busqueda in db.REDES_SOCIALES where busqueda.id_usuario == id select busqueda).ToList();
            if (verificar.Count == 1) {
                return RedirectToAction("Edit", "REDES_SOCIALES");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: REDES_SOCIALES/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_redes,id_usuario,whatsapp,instagram,facebook,web_app,estado")] REDES_SOCIALES rEDES_SOCIALES)
        {
       
            var id = User.Identity.GetUserId();
            rEDES_SOCIALES.id_usuario = id;
            rEDES_SOCIALES.estado = "activo";

            if (ModelState.IsValid)
            {
                db.REDES_SOCIALES.Add(rEDES_SOCIALES);
                db.SaveChanges();
                return RedirectToAction("Index", "SERVICIOS");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", rEDES_SOCIALES.id_usuario);
            return View(rEDES_SOCIALES);
        }

        // GET: REDES_SOCIALES/Edit/5
        public ActionResult Edit()
        {
            REDES_SOCIALES rEDES_SOCIALES = new REDES_SOCIALES(); 
            var idUser = User.Identity.GetUserId();

            List<REDES_SOCIALES> verificar = new List<REDES_SOCIALES>();
            verificar = (from busqueda in db.REDES_SOCIALES where busqueda.id_usuario == idUser select busqueda).ToList();
            
            if(verificar.Count == 1) 
            { 
                foreach (var item in verificar)
                {
                    rEDES_SOCIALES = db.REDES_SOCIALES.Find(item.id_redes);
                }


                if (rEDES_SOCIALES == null)
                {
                    return RedirectToAction("Create");
                }

                ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", rEDES_SOCIALES.id_usuario);

                return View(rEDES_SOCIALES);
            }
            else
            {
                return RedirectToAction("Create");
            }

        }

        // POST: REDES_SOCIALES/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_redes,id_usuario,whatsapp,instagram,facebook,web_app,estado")] REDES_SOCIALES rEDES_SOCIALES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rEDES_SOCIALES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", rEDES_SOCIALES.id_usuario);
            return View(rEDES_SOCIALES);
        }

        // GET: REDES_SOCIALES/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REDES_SOCIALES rEDES_SOCIALES = db.REDES_SOCIALES.Find(id);
            if (rEDES_SOCIALES == null)
            {
                return HttpNotFound();
            }
            return View(rEDES_SOCIALES);
        }

        // POST: REDES_SOCIALES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            REDES_SOCIALES rEDES_SOCIALES = db.REDES_SOCIALES.Find(id);
            db.REDES_SOCIALES.Remove(rEDES_SOCIALES);
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
