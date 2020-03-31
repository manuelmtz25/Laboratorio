using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio.Core.DAL.Data;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Laboratorio.Core.MVC.Areas.Servicios.Controllers
{
    public class GeneralController : Controller
    {
        private mmadatabaseEntities db = new mmadatabaseEntities();

        // GET: Servicios/General
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PruebaLaboratorio()
        {
            return View();
        }

        [HttpPost]
        public JObject ObtenerItemsPorPrueba(List<string> checkedNamesPerfiles, List<string> checkedNamesEstudios)
        {
            dynamic pruebaLaboratorio = new JObject();
            pruebaLaboratorio.Perfiles = new JArray();

            if (checkedNamesPerfiles!=null)
            {
                foreach (var strIdPerfil in checkedNamesPerfiles)
                {
                    var guidIdPerfil = Guid.Parse(strIdPerfil);
                    var perfil = db.lab_Perfiles.FirstOrDefault(e => e.Id == guidIdPerfil);

                    dynamic perfiles = new JObject();
                    perfiles.Id = perfil.Id;
                    perfiles.Nombre = perfil.Nombre;
                    perfiles.Estudios = new JArray();

                    var listIdEstudios = new List<string>();
                    foreach (var estudios in perfil.lab_Estudios)
                    {
                        listIdEstudios.Add(estudios.Id.ToString());
                    }

                    var itemsPorEstudios = ObtenerItemsPorEstudio(listIdEstudios);
                    perfiles.Estudios = itemsPorEstudios;

                    pruebaLaboratorio.Perfiles.Add(perfiles);
                }
            }

            if (checkedNamesEstudios!=null)
            {
                dynamic perfilX = new JObject();
                perfilX.Id = "-";
                perfilX.Nombre = "-";
                perfilX.Estudios = new JArray();
                perfilX.Estudios = ObtenerItemsPorEstudio(checkedNamesEstudios);
                pruebaLaboratorio.Perfiles.Add(perfilX);
            }

            return pruebaLaboratorio;
        }

        private JArray ObtenerItemsPorEstudio(List<string> checkedNamesEstudios)
        {
            dynamic Estudios = new JArray();


            foreach (var strIdEstudio in checkedNamesEstudios)
            {
                var guidIdEstudio = Guid.Parse(strIdEstudio);
                var estudio = db.lab_Estudios.FirstOrDefault(e => e.Id == guidIdEstudio);

                dynamic estudios = new JObject();
                estudios.Id = estudio.Id;
                estudios.Nombre = estudio.Nombre;
                estudios.Elementos = new JArray();

                foreach (var elemento in estudio.lab_Elementos)
                {
                    dynamic elementos = new JObject();
                    elementos.Id = elemento.Id;
                    elementos.Nombre = elemento.Nombre;

                    estudios.Elementos.Add(elementos);
                }

                Estudios.Add(estudios);
            }

            return Estudios;
        }
    }
}