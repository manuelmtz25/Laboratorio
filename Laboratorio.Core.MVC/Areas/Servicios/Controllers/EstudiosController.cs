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
    public class EstudiosController : Controller
    {
        private mmadatabaseEntities db = new mmadatabaseEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string CrearEstudio(string nombre, decimal costo, string grupo, string idPerfil)
        {
            try
            {
                var estudio = new lab_Estudios
                {
                    Id = Guid.NewGuid(),
                    Nombre = nombre,
                    Costo = costo,
                    Grupo = grupo
                };

                db.lab_Estudios.Add(estudio);
                db.SaveChanges();

                if (idPerfil!="null")
                {
                    var guidIdPerfil = Guid.Parse(idPerfil);
                    var perfil = db.lab_Perfiles.Find(guidIdPerfil);
                    perfil.lab_Estudios.Add(estudio);
                    db.SaveChanges();
                }

                return "Ok";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }

        [HttpPost]
        public string CrearRelacion(string selected, string idPerfil)
        {
            try
            {
                if (idPerfil != "null")
                {
                    var guidIdPerfil = Guid.Parse(idPerfil);
                    var perfil = db.lab_Perfiles.Find(guidIdPerfil);

                    var guidIdEstudios = Guid.Parse(selected);
                    var estudio = db.lab_Estudios.Find(guidIdEstudios);

                    perfil.lab_Estudios.Add(estudio);
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
        public string ObtenerEstudios(string idPerfil)
        {
            if (idPerfil == null || idPerfil == "undefined" || idPerfil == "null")
            {
                var estudios = db.lab_Estudios.Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).OrderBy(o => o.Nombre);

                var obj = JsonConvert.SerializeObject(new {estudios});

                return obj;
            }
            else
            {
                var guidIdPerfil = Guid.Parse(idPerfil);

                var estudios = db.lab_Estudios.Where(e =>e.lab_Perfiles.Contains(e.lab_Perfiles.FirstOrDefault(p => p.Id == guidIdPerfil))).Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).ToList();

                var obj_x_perfil = JsonConvert.SerializeObject(new { estudios });

                return obj_x_perfil;
            }
        }

        [HttpGet]
        public string ObtenerEstudiosDisponibles(string idPerfil)
        {
            if (idPerfil == null || idPerfil == "undefined" || idPerfil == "null")
            {
                //aquí no se debería meter, no puede venir idPerfil ya que la consulta es con base en el idPerfil
                var estudios = db.lab_Estudios.Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).OrderBy(o => o.Nombre);

                var obj = JsonConvert.SerializeObject(new {estudios});

                return obj;
            }
            else
            {
                var guidIdPerfil = Guid.Parse(idPerfil);

                var estudiosComp = db.lab_Estudios.Where(e =>e.lab_Perfiles.Contains(e.lab_Perfiles.FirstOrDefault(p => p.Id == guidIdPerfil))).Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).ToList();

                var estudiosAll = db.lab_Estudios.Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).ToList();

                var estudios = estudiosAll.Except(estudiosComp).Select(a => new
                {
                    a.Id,
                    a.Nombre,
                    a.Costo,
                    a.Grupo
                }).ToList();

                var obj_x_perfil = JsonConvert.SerializeObject(new { estudios });

                return obj_x_perfil;
            }
        }

        [HttpPost]
        public string BorrarEstudio(string value)
        {
            try
            {
                var guidValue = Guid.Parse(value);
                var estudio = db.lab_Estudios.Find(guidValue);
                db.lab_Estudios.Remove(estudio);
                db.SaveChanges();

                return "Ok";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }

        [HttpPost]
        public string BorrarRelacion(string value, string idPerfil)
        {
            try
            {
                if (idPerfil != "null")
                {
                    var guidIdEstudio = Guid.Parse(value);
                    var estudio = db.lab_Estudios.Find(guidIdEstudio);
                    var guidIdPerfil = Guid.Parse(idPerfil);
                    var perfil = db.lab_Perfiles.Find(guidIdPerfil);
                    perfil.lab_Estudios.Remove(estudio);
                    db.SaveChanges();
                }

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
            lab_Estudios lab_Estudios = db.lab_Estudios.Find(id);
            if (lab_Estudios == null)
            {
                return HttpNotFound();
            }
            return View(lab_Estudios);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Costo")] lab_Estudios lab_Estudios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lab_Estudios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lab_Estudios);
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
