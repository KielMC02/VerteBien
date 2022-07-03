using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VerteBienV1.Models;

namespace VerteBienV1.Controllers
{
    public class SUSCRIPCIONsController : Controller
    {
        private VERTEBIENEntities db = new VERTEBIENEntities();

        // GET: SUSCRIPCIONs
        public ActionResult Index()
        {
            var sUSCRIPCION = db.SUSCRIPCION.Include(s => s.AspNetUsers);
            return View(sUSCRIPCION.ToList());
        }

        //Recordatorio para mitad de mes
        public ActionResult RecordatorioMitad() 
        {
            var today = DateTime.Today;
            var mesactual = new DateTime(today.Year, today.Month, 1);
            var mesanterior = mesactual.AddMonths(-1);
            var quincena = mesanterior.AddDays(14);
            AspNetUsers usuario = new AspNetUsers();
            List<SUSCRIPCION> suscripciones = new List<SUSCRIPCION>();
            suscripciones = (from busqueda in db.SUSCRIPCION where busqueda.fecha_suscripcion == quincena select busqueda).ToList();
            if (suscripciones.Count > 0) 
            { 
            
                foreach(var item in suscripciones) 
                {
                    usuario = db.AspNetUsers.Find(item.id_usuario);
                    string from = "kielcuentas@gmail.com";
                    MailMessage mailUser = new MailMessage(from, usuario.Email);
                    {

                        mailUser.Subject = "Recordatorio de Pago";
                        mailUser.Body = "Hola " + usuario.nombre +" te recordamos que el 15 del mes en curso debes renovar tu suscripcion Verte Bien.";
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
            
            }

            return RedirectToAction("Index", "AspNetUsers");
        }

        //Recordatorio para mitad de mes
        public ActionResult RecordatorioFin()
        {
            ViewBag.notificacion = 4;
            var today = DateTime.Today;
            var mesactual = new DateTime(today.Year, today.Month, 1);
            //var mesanterior = mesactual.AddMonths(-1);
            var finMes = mesactual.AddDays(-1);
            AspNetUsers usuario = new AspNetUsers();
            List<SUSCRIPCION> suscripciones = new List<SUSCRIPCION>();
            suscripciones = (from busqueda in db.SUSCRIPCION where busqueda.fecha_suscripcion == finMes select busqueda).ToList();
            if (suscripciones.Count > 0)
            {

                foreach (var item in suscripciones)
                {
                    usuario = db.AspNetUsers.Find(item.id_usuario);
                    string from = "kielcuentas@gmail.com";
                    MailMessage mailUser = new MailMessage(from, usuario.Email);
                    {

                        mailUser.Subject = "Recordatorio de Pago";
                        mailUser.Body = "Hola " + usuario.nombre + " te recordamos que el "+Convert.ToString(finMes.Day) +" del mes en curso debes renovar tu suscripcion Verte Bien.";
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
                ViewBag.notificacion = 5;
                return RedirectToAction("Index", "AspNetUsers");
            }

            return RedirectToAction("Index", "AspNetUsers");
        }



        // GET: SUSCRIPCIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            if (sUSCRIPCION == null)
            {
                return HttpNotFound();
            }
            return View(sUSCRIPCION);
        }

        // GET: SUSCRIPCIONs/Create
        public ActionResult Create()
        {
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: SUSCRIPCIONs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_suscripcion,id_usuario,estado,id_suscripcion_kushki,comentario,fecha_suscripcion")] SUSCRIPCION sUSCRIPCION)
        {
            if (ModelState.IsValid)
            {
                db.SUSCRIPCION.Add(sUSCRIPCION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
            return View(sUSCRIPCION);
        }

        // GET: SUSCRIPCIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            if (sUSCRIPCION == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
            return View(sUSCRIPCION);
        }

        // POST: SUSCRIPCIONs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_suscripcion,id_usuario,estado,id_suscripcion_kushki,comentario,fecha_suscripcion")] SUSCRIPCION sUSCRIPCION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sUSCRIPCION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.AspNetUsers, "Id", "Email", sUSCRIPCION.id_usuario);
            return View(sUSCRIPCION);
        }

        // GET: SUSCRIPCIONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            if (sUSCRIPCION == null)
            {
                return HttpNotFound();
            }
            return View(sUSCRIPCION);
        }

        // POST: SUSCRIPCIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUSCRIPCION sUSCRIPCION = db.SUSCRIPCION.Find(id);
            db.SUSCRIPCION.Remove(sUSCRIPCION);
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
