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
    public class PUNTUACION_PELUQUERIAController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();
        [Authorize(Roles = "administrador")]
        // GET: PUNTUACION_PELUQUERIA
        public ActionResult Index()
        {
            var pUNTUACION_PELUQUERIA = db.PUNTUACION_PELUQUERIA.Include(p => p.AspNetUsers).Include(p => p.CITAS);
            return View(pUNTUACION_PELUQUERIA.ToList());
        }

        // GET: PUNTUACION_PELUQUERIA/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PUNTUACION_PELUQUERIA pUNTUACION_PELUQUERIA = db.PUNTUACION_PELUQUERIA.Find(id);
            if (pUNTUACION_PELUQUERIA == null)
            {
                return HttpNotFound();
            }
            return View(pUNTUACION_PELUQUERIA);
        }

        // GET: PUNTUACION_PELUQUERIA/Create
        public ActionResult Create(int id_cita)
        {
            CITAS citaPuntuacion = db.CITAS.Find(id_cita);

            List<PUNTUACION_PELUQUERIA> comprobar = new List<PUNTUACION_PELUQUERIA>();
            comprobar = (from comprobarPuntuacion in db.PUNTUACION_PELUQUERIA where comprobarPuntuacion.id_cita == id_cita select comprobarPuntuacion).ToList();
            if (comprobar.Count != 0)
            {
                ViewBag.comprobar = "Listo";
            }

            ViewBag.id_usuario = citaPuntuacion.id_usuario;
            ViewBag.id_cita = citaPuntuacion.id_cita;
            //ViewBag.peluqueria = citaPuntuacion.SERVICIOS.id_usuario;


            //ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            //ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_cita");
            return View();
        }

        // POST: PUNTUACION_PELUQUERIA/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_puntuacion_peluqueria,id_usuario,id_cita,comentario,estrellas,fecha_creacion,estado")] PUNTUACION_PELUQUERIA pUNTUACION_PELUQUERIA)
        {
            var id = "vacio";
            var estaAutenticado = User.Identity.IsAuthenticated;
            if (estaAutenticado)
            {
                id = User.Identity.GetUserId();
            }
            pUNTUACION_PELUQUERIA.id_usuario = id;
            pUNTUACION_PELUQUERIA.fecha_creacion = DateTime.Today;
            pUNTUACION_PELUQUERIA.estado = "activo";
            if (ModelState.IsValid)
            {
                db.PUNTUACION_PELUQUERIA.Add(pUNTUACION_PELUQUERIA);
                db.SaveChanges();
                return RedirectToAction("Index","SERVICIOS");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", pUNTUACION_PELUQUERIA.id_usuario);
            ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_usuario", pUNTUACION_PELUQUERIA.id_cita);
            return View(pUNTUACION_PELUQUERIA);
        }
        [Authorize(Roles = "administrador")]
        // GET: PUNTUACION_PELUQUERIA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PUNTUACION_PELUQUERIA pUNTUACION_PELUQUERIA = db.PUNTUACION_PELUQUERIA.Find(id);
            if (pUNTUACION_PELUQUERIA == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", pUNTUACION_PELUQUERIA.id_usuario);
            ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_usuario", pUNTUACION_PELUQUERIA.id_cita);
            return View(pUNTUACION_PELUQUERIA);
        }

        // POST: PUNTUACION_PELUQUERIA/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador")]
        public ActionResult Edit([Bind(Include = "id_puntuacion_peluqueria,id_usuario,id_cita,comentario,estrellas,fecha_creacion,estado")] PUNTUACION_PELUQUERIA pUNTUACION_PELUQUERIA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pUNTUACION_PELUQUERIA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", pUNTUACION_PELUQUERIA.id_usuario);
            ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_usuario", pUNTUACION_PELUQUERIA.id_cita);
            return View(pUNTUACION_PELUQUERIA);
        }
        [Authorize(Roles = "administrador")]
        // GET: PUNTUACION_PELUQUERIA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PUNTUACION_PELUQUERIA pUNTUACION_PELUQUERIA = db.PUNTUACION_PELUQUERIA.Find(id);
            if (pUNTUACION_PELUQUERIA == null)
            {
                return HttpNotFound();
            }
            return View(pUNTUACION_PELUQUERIA);
        }

        // POST: PUNTUACION_PELUQUERIA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PUNTUACION_PELUQUERIA pUNTUACION_PELUQUERIA = db.PUNTUACION_PELUQUERIA.Find(id);
            db.PUNTUACION_PELUQUERIA.Remove(pUNTUACION_PELUQUERIA);
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
