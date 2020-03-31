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
    public class ElementosController : Controller
    {
        private mmadatabaseEntities db = new mmadatabaseEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string CrearElemento(string nombre, string unidadMedida, string notas, string idEstudio)
        {
            try
            {
                var elemento = new lab_Elementos
                {
                    Id = Guid.NewGuid(),
                    Nombre = nombre,
                    UnidadMedida = unidadMedida,
                    Notas = notas
                };


                db.lab_Elementos.Add(elemento);
                db.SaveChanges();

                if (idEstudio != "null")
                {
                    var guidIdEstudio = Guid.Parse(idEstudio);
                    var estudio = db.lab_Estudios.Find(guidIdEstudio);
                    estudio.lab_Elementos.Add(elemento);
                    db.SaveChanges();
                }

                return "Ok";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }

        [HttpGet]
        public string ObtenerElementos(string idEstudios)
        {
            if (idEstudios == null || idEstudios == "undefined" || idEstudios == "null")
            {
                var elementos = db.lab_Elementos.Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.UnidadMedida,
                    a.Notas
                }).OrderBy(o => o.Nombre);

                var obj = JsonConvert.SerializeObject(new { elementos });

                return obj;
            }
            else
            {
                var guidIdEstudio = Guid.Parse(idEstudios);

                var elementos = db.lab_Elementos.Where(p => p.lab_Estudios.Contains(p.lab_Estudios.FirstOrDefault(e => e.Id == guidIdEstudio))).Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.UnidadMedida,
                    a.Notas
                }).ToList();

                var obj_x_estudio = JsonConvert.SerializeObject(new { elementos });

                return obj_x_estudio;
            }

        }

        [HttpPost]
        public string BorrarElemento(string value,string idEstudios)
        {
            try
            {
                if (idEstudios != null)
                {
                    var guidIdEstudio = Guid.Parse(idEstudios);
                    var estudio = db.lab_Estudios.Find(guidIdEstudio);

                    var guidValue = Guid.Parse(value);
                    var elemento = db.lab_Elementos.Find(guidValue);

                    estudio.lab_Elementos.Remove(elemento);
                    db.lab_Elementos.Remove(elemento);
                    db.SaveChanges();
                    return "Ok";
                }

                return "Inner Error";
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
            lab_Elementos lab_Elementos = db.lab_Elementos.Find(id);
            if (lab_Elementos == null)
            {
                return HttpNotFound();
            }
            return View(lab_Elementos);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,UnidadMedida,Notas")] lab_Elementos lab_Elementos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lab_Elementos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lab_Elementos);
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
