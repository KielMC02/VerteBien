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
    public class PUNTUACION_SERVICIOSController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();

        // GET: PUNTUACION_SERVICIOS
        public ActionResult Index()
        {
            var pUNTUACION_SERVICIOS = db.PUNTUACION_SERVICIOS.Include(p => p.AspNetUsers).Include(p => p.CITAS).Include(p => p.SERVICIOS);
            return View(pUNTUACION_SERVICIOS.ToList());
        }

        // GET: PUNTUACION_SERVICIOS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PUNTUACION_SERVICIOS pUNTUACION_SERVICIOS = db.PUNTUACION_SERVICIOS.Find(id);
            if (pUNTUACION_SERVICIOS == null)
            {
                return HttpNotFound();
            }
            return View(pUNTUACION_SERVICIOS);
        }

        // GET: PUNTUACION_SERVICIOS/Create
        public ActionResult Create(int id_Cita)
        {
            CITAS citaPuntuacion = db.CITAS.Find(id_Cita);

            List<PUNTUACION_SERVICIOS> comprobar = new List<PUNTUACION_SERVICIOS>();
            comprobar = (from comprobarPuntuacion in db.PUNTUACION_SERVICIOS where comprobarPuntuacion.id_cita == id_Cita select comprobarPuntuacion).ToList();
            if(comprobar.Count != 0)
            {
                ViewBag.comprobar = "Listo";
            }


            ViewBag.id_usuario = citaPuntuacion.id_usuario;
            ViewBag.id_cita = citaPuntuacion.id_cita;
            ViewBag.id_servicio = citaPuntuacion.id_servicio;

            //ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            //ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_cita");
            //ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "nombre_servicio");
            return View();
        }

        // POST: PUNTUACION_SERVICIOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_puntuacion_servicio,id_usuario,id_servicio,id_cita,comentario,estrellas,fecha_creacion,estado")] PUNTUACION_SERVICIOS pUNTUACION_SERVICIOS)
        {
            //var id = "vacio";
            //var estaAutenticado = User.Identity.IsAuthenticated;
            //if (estaAutenticado)
            //{
            //    id = User.Identity.GetUserId();
            //}
            //pUNTUACION_SERVICIOS.id_usuario = id;
            pUNTUACION_SERVICIOS.fecha_creacion = DateTime.Today;
            pUNTUACION_SERVICIOS.estado = "activo";
            if (ModelState.IsValid)
            {
                db.PUNTUACION_SERVICIOS.Add(pUNTUACION_SERVICIOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", pUNTUACION_SERVICIOS.id_usuario);
            ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_usuario", pUNTUACION_SERVICIOS.id_cita);
            ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "id_usuario", pUNTUACION_SERVICIOS.id_servicio);
            return View(pUNTUACION_SERVICIOS);
        }

        // GET: PUNTUACION_SERVICIOS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PUNTUACION_SERVICIOS pUNTUACION_SERVICIOS = db.PUNTUACION_SERVICIOS.Find(id);
            if (pUNTUACION_SERVICIOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", pUNTUACION_SERVICIOS.id_usuario);
            ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_usuario", pUNTUACION_SERVICIOS.id_cita);
            ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "id_usuario", pUNTUACION_SERVICIOS.id_servicio);
            return View(pUNTUACION_SERVICIOS);
        }

        // POST: PUNTUACION_SERVICIOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_puntuacion_servicio,id_usuario,id_servicio,id_cita,comentario,estrellas,fecha_creacion,estado")] PUNTUACION_SERVICIOS pUNTUACION_SERVICIOS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pUNTUACION_SERVICIOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", pUNTUACION_SERVICIOS.id_usuario);
            ViewBag.id_cita = new SelectList(db.CITAS, "id_cita", "id_usuario", pUNTUACION_SERVICIOS.id_cita);
            ViewBag.id_servicio = new SelectList(db.SERVICIOS, "id_servicio", "id_usuario", pUNTUACION_SERVICIOS.id_servicio);
            return View(pUNTUACION_SERVICIOS);
        }

        // GET: PUNTUACION_SERVICIOS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PUNTUACION_SERVICIOS pUNTUACION_SERVICIOS = db.PUNTUACION_SERVICIOS.Find(id);
            if (pUNTUACION_SERVICIOS == null)
            {
                return HttpNotFound();
            }
            return View(pUNTUACION_SERVICIOS);
        }

        // POST: PUNTUACION_SERVICIOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PUNTUACION_SERVICIOS pUNTUACION_SERVICIOS = db.PUNTUACION_SERVICIOS.Find(id);
            db.PUNTUACION_SERVICIOS.Remove(pUNTUACION_SERVICIOS);
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
