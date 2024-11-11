using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ViaticosWeb.Models
{
    public class VehMaesController : Controller
    {
        private ViaticosEntities4 db = new ViaticosEntities4();

        // GET: VehMaes
        public ActionResult Index()
        {
            return View(db.VehMae.ToList());
        }

        // GET: VehMaes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehMae vehMae = db.VehMae.Find(id);
            if (vehMae == null)
            {
                return HttpNotFound();
            }
            return View(vehMae);
        }

        // GET: VehMaes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehMaes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Item,Codact,VehPla,AreCod,MarCod,TipCod,Vo2Cod,VehKm,VehEst,VehAno,Asignado,FecReg,estado,codemp,celular,fecini,fecfin,fecbaj,VehUnid")] VehMae vehMae)
        {
            if (ModelState.IsValid)
            {
                db.VehMae.Add(vehMae);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehMae);
        }

        // GET: VehMaes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehMae vehMae = db.VehMae.Find(id);
            if (vehMae == null)
            {
                return HttpNotFound();
            }
            return View(vehMae);
        }

        // POST: VehMaes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Item,Codact,VehPla,AreCod,MarCod,TipCod,Vo2Cod,VehKm,VehEst,VehAno,Asignado,FecReg,estado,codemp,celular,fecini,fecfin,fecbaj,VehUnid")] VehMae vehMae)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehMae).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehMae);
        }

        // GET: VehMaes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehMae vehMae = db.VehMae.Find(id);
            if (vehMae == null)
            {
                return HttpNotFound();
            }
            return View(vehMae);
        }

        // POST: VehMaes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehMae vehMae = db.VehMae.Find(id);
            db.VehMae.Remove(vehMae);
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
