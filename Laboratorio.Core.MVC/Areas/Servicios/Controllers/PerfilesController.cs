using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laboratorio.Core.DAL.Data;
using Newtonsoft.Json;

namespace Laboratorio.Core.MVC.Areas.Servicios.Controllers
{
    public class PerfilesController : Controller
    {
        private mmadatabaseEntities db = new mmadatabaseEntities();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string CrearPerfil(string nombre, decimal costo, string grupo)
        {
            try
            {
                var perfil = new lab_Perfiles
                {
                    Id = Guid.NewGuid(),
                    Nombre = nombre,
                    Costo = costo,
                    Grupo = grupo
                };


                db.lab_Perfiles.Add(perfil);
                db.SaveChanges();

                return "Ok";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }

        [HttpGet]
        public string ObtenerPerfiles(string idEstudios)
        {
            if (idEstudios == null || idEstudios == "undefined")
            {
                var perfiles = db.lab_Perfiles.Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).OrderBy(o => o.Nombre);

                var obj = JsonConvert.SerializeObject(new {perfiles});

                return obj;
            }
            else
            {
                var guidIdEstudio = Guid.Parse(idEstudios);

                var perfiles = db.lab_Perfiles.Where(p => p.lab_Estudios.Contains(p.lab_Estudios.FirstOrDefault(e => e.Id == guidIdEstudio))).Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).ToList();

                var obj_x_estudio = JsonConvert.SerializeObject(new { perfiles });

                return obj_x_estudio;
            }

        }

        [HttpPost]
        public string BorrarPerfil(string value)
        {
            try
            {
                var guidValue = Guid.Parse(value);
                var perfil = db.lab_Perfiles.Find(guidValue);
                db.lab_Perfiles.Remove(perfil);
                db.SaveChanges();

                return "Ok";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }
        
        
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lab_Perfiles lab_Perfiles = db.lab_Perfiles.Find(id);
            if (lab_Perfiles == null)
            {
                return HttpNotFound();
            }
            return View(lab_Perfiles);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Costo")] lab_Perfiles lab_Perfiles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lab_Perfiles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lab_Perfiles);
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
