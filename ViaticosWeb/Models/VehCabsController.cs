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
    public class VehCabsController : Controller
    {
        private ViaticosEntities4 db = new ViaticosEntities4();

        // GET: VehCabs
        public ActionResult Index()
        {
            return View(db.VehCab.ToList());
        }

        // GET: VehCabs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehCab vehCab = db.VehCab.Find(id);
            if (vehCab == null)
            {
                return HttpNotFound();
            }
            return View(vehCab);
        }

        // GET: VehCabs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehCabs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "solcod,tipo,codveh,kmant,kmact,km,tipman,observaciones,codgrifo,usureg,fegreg,estado,anno,gercod,arecod,fecaut,usuaut,fecimp,usuimp,tipcom,fecrev,usurev,usuanu,fecanu,codgrupo")] VehCab vehCab)
        {
            if (ModelState.IsValid)
            {
                db.VehCab.Add(vehCab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehCab);
        }

        // GET: VehCabs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehCab vehCab = db.VehCab.Find(id);
            if (vehCab == null)
            {
                return HttpNotFound();
            }
            return View(vehCab);
        }

        // POST: VehCabs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "solcod,tipo,codveh,kmant,kmact,km,tipman,observaciones,codgrifo,usureg,fegreg,estado,anno,gercod,arecod,fecaut,usuaut,fecimp,usuimp,tipcom,fecrev,usurev,usuanu,fecanu,codgrupo")] VehCab vehCab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehCab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehCab);
        }

        // GET: VehCabs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehCab vehCab = db.VehCab.Find(id);
            if (vehCab == null)
            {
                return HttpNotFound();
            }
            return View(vehCab);
        }

        // POST: VehCabs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehCab vehCab = db.VehCab.Find(id);
            db.VehCab.Remove(vehCab);
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
