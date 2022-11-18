using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VerteBienV1.Models;

namespace VerteBienV1.Controllers
{
    public class CARDController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();
        [Authorize(Roles = "expres,preferencial,vip,administrador")]
        // GET: CARD
        public ActionResult Index()
        {
            var cARD = db.CARD.Include(c => c.AspNetUsers);
            return View(cARD.ToList());
        }

        // GET: CARD/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARD cARD = db.CARD.Find(id);
            if (cARD == null)
            {
                return HttpNotFound();
            }
            return View(cARD);
        }
        [Authorize(Roles = "administrador")]
        // GET: CARD/Create
        public ActionResult Create()
        {
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: CARD/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_card,id_usuario,estatus,token,trasaction_reference,digitos,fecha_expiracion,fecha_agregada,comentario")] CARD cARD)
        {
            if (ModelState.IsValid)
            {
                db.CARD.Add(cARD);
                db.SaveChanges();

                AspNetUsers aspNetUsers = db.AspNetUsers.Find(cARD.id_usuario);
                
                if (aspNetUsers.estado == "new" || aspNetUsers.estado == "suspendido") 
                { 

                   var resultadoProc = db.Database.ExecuteSqlCommand("SP_ActualizarEstado @id", new SqlParameter("@id", Convert.ToString(cARD.id_usuario)));
                }
                return RedirectToAction("Index","SERVICIOS");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", cARD.id_usuario);
            return View(cARD);
        }
        [Authorize(Roles = "administrador")]
        // GET: CARD/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARD cARD = db.CARD.Find(id);
            if (cARD == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", cARD.id_usuario);
            return View(cARD);
        }

        // POST: CARD/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador")]
        public ActionResult Edit([Bind(Include = "id_card,id_usuario,estatus,token,trasaction_reference,digitos,fecha_expiracion,fecha_agregada,comentario")] CARD cARD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cARD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", cARD.id_usuario);
            return View(cARD);
        }

        // GET: CARD/Delete/5
        [Authorize(Roles = "administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARD cARD = db.CARD.Find(id);
            if (cARD == null)
            {
                return HttpNotFound();
            }
            return View(cARD);
        }

        // POST: CARD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CARD cARD = db.CARD.Find(id);
            db.CARD.Remove(cARD);
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
