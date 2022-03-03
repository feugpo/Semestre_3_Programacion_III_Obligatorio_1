using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Repositorios;
using MVC.Models;
using System.Configuration;
using System.IO;



namespace MVC.Controllers
{
    public class HomeController : Controller
    {




        public ActionResult Home(string mensaje)
        {
            
            if(Session["rol"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.mensaje = mensaje;
            return View();
        }

        public ActionResult misProyectos()
        {
            if (Session["rol"] == null || Session["rol"].ToString() != "Solicitante")
            {
                return RedirectToAction("Login", "Usuario");
            }
        

            string usuario = Session["id"].ToString();

            int buscarusuario = int.Parse(usuario);

            ViewBag.jpg = ".jpg";
            List<Proyecto> Pro = FachadaExpress.TraerProyectos(buscarusuario);
            if (Pro == null)
            {
                ViewBag.Mensaje = "Usuario o contraseña incorrectos";
                return View();
            }

            return View(Pro);
        }


        #region Alta Proyecto Personal
        public ActionResult AltaProyectoPersonal(string mensaje)
        {
  
            if (Session["rol"] == null || Session["rol"].ToString() != "Solicitante")
            {
                return RedirectToAction("Login", "Usuario");
            }
           
            ViewBag.Mensaje = mensaje;
            return View();

            //  return RedirectToAction("home", new { mensaje = textoMensaje });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AltaProyectoPersonal(ViewModelProyecto nuevoProyecto)
        {
            
            string errormensaje = "Hubo un problema, intentelo de nuevo mas tarde.";
            ViewBag.mensaje = errormensaje;

            string cedula = Session["cedula"].ToString();
            if (FachadaExpress.ProyectosPendientes(cedula))
            {
                errormensaje = "Aún tienes un Proyecto pendiente";
                return RedirectToAction("home", new { mensaje = errormensaje });

            }

            #region GuardarImagen
            string rutaAplicacion = HttpRuntime.AppDomainAppPath;
            string nombreCarpetaImagenes = ConfigurationManager.AppSettings["ImagenesGuardadas"];
            string rutaDirectorioImagenes = Path.Combine(rutaAplicacion, nombreCarpetaImagenes);
            string rutaCompletaArchivo = Path.Combine(rutaDirectorioImagenes, nuevoProyecto.Titulo + ".jpg");
            if (nuevoProyecto.Imagen != null)
            {
                nuevoProyecto.Imagen.SaveAs(rutaCompletaArchivo);
            }
            else if (nuevoProyecto.Imagen == null)
            {
                return View(nuevoProyecto);
            }

            #endregion

            nuevoProyecto.TasaInteres = FachadaExpress.ObtenerTasa(nuevoProyecto.Cuotas);
            nuevoProyecto.Integrantes = 1;
            nuevoProyecto.Tipo = "Personal";
            nuevoProyecto.Estado = "Pendiente de revisión";
            nuevoProyecto.FechaCreacion = DateTime.Today;



            if (ModelState.IsValid)
            {

                if (!FachadaExpress.ValidarMontoPersonal(nuevoProyecto.Monto))
                {
                    errormensaje = "El Monto esta fuera del rango";
                    return RedirectToAction("AltaProyectoPersonal", new { mensaje = errormensaje });
                }
                if (!FachadaExpress.ValidarCuotas(nuevoProyecto.Cuotas))
                {
                    errormensaje = "Las cuotas estan fuera del rango";
                    return RedirectToAction("AltaProyectoPersonal", new { mensaje = errormensaje });
                }
                if (!FachadaExpress.NuevoTitulo(nuevoProyecto.Titulo))
                {
                    errormensaje = "El Titulo ya existe";
                    return RedirectToAction("AltaProyectoPersonal", new { mensaje = errormensaje });
                }
             
                return RedirectToAction("confirmarProyecto", nuevoProyecto);
            }

            //return RedirectToAction("AltaProyectoPersonal", new { mensaje = errormensaje });
            return View(nuevoProyecto);
        }
        #endregion

        #region Alta Proyecto Cooperativo
        public ActionResult AltaProyectoCooperativo()
        {
        
            if (Session["rol"] == null || Session["rol"].ToString() != "Solicitante")
            {
                return RedirectToAction("Login", "Usuario");
            }
      
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AltaProyectoCooperativo(ViewModelProyectoCoop nuevoProyectoCoop)
        {
            string errormensaje = "Hubo un problema, intentelo de nuevo mas tarde.";
            ViewBag.mensaje = errormensaje;

            string cedula = Session["cedula"].ToString();
            if (FachadaExpress.ProyectosPendientes(cedula))
            {
                errormensaje = "Aún tienes un Proyecto pendiente";
                return RedirectToAction("home", new { mensaje = errormensaje });

            }


            string rutaAplicacion = HttpRuntime.AppDomainAppPath;
            string nombreCarpetaImagenes = ConfigurationManager.AppSettings["ImagenesGuardadas"];
            string rutaDirectorioImagenes = Path.Combine(rutaAplicacion, nombreCarpetaImagenes);
            string rutaCompletaArchivo = Path.Combine(rutaDirectorioImagenes, nuevoProyectoCoop.Titulo + ".jpg");

            nuevoProyectoCoop.Imagen.SaveAs(rutaCompletaArchivo);

            nuevoProyectoCoop.TasaInteres = FachadaExpress.ObtenerTasa(nuevoProyectoCoop.Cuotas);
            ViewModelProyectoCoop pro = nuevoProyectoCoop;
            pro.Tipo = "Cooperativo";
            pro.Experiencia = "--";
            pro.Estado = "Pendiente de revisión";
            pro.FechaCreacion = DateTime.Today;

            if (ModelState.IsValid)
            {
                if (!FachadaExpress.ValidarMontoCooperativo(pro.Integrantes, pro.Monto))
                {
                    errormensaje = "El Monto esta fuera del rango";
                    return RedirectToAction("AltaProyectoCooperativo", new { mensaje = errormensaje });
                }
                if (!FachadaExpress.ValidarCuotas(pro.Cuotas))
                {
                    errormensaje = "Las cuotas estan fuera del rango";
                    return RedirectToAction("AltaProyectoCooperativo", new { mensaje = errormensaje });
                }
                if (!FachadaExpress.NuevoTitulo(pro.Titulo))
                {
                    errormensaje = "El Titulo ya existe";
                    return RedirectToAction("AltaProyectoCooperativo", new { mensaje = errormensaje });
                }
            

                return RedirectToAction("confirmarProyecto", pro);
            }

            return RedirectToAction("AltaProyectoCooperativo", new { mensaje = errormensaje });

        }
        #endregion

        #region Confirmacion y Alta de Proyectos 
        public ActionResult confirmarProyecto(ViewModelProyecto pro)
        {
            #region calcular los montos y cuotas
            decimal traerinteres = FachadaExpress.ObtenerTasa(pro.Cuotas); // trae el calculo de la tasa de interes %%
            traerinteres = 1 + traerinteres / 100;
            decimal montototal = pro.Monto * traerinteres;     // Calcula el % de interes que se le va a sumar al monto
            ViewBag.montoconinteres = montototal;                               // se le pasa el monto total con interes a la vista

            montototal = decimal.Round(montototal, 2);
            decimal montoporcuota = montototal / pro.Cuotas;
            montoporcuota = decimal.Round(montoporcuota, 2);
            ViewBag.montoconinteres = montototal;                               // se le pasa el monto total con interes a la vista
            ViewBag.montoporcuota = montoporcuota;
            #endregion
            // calcular los montos y cuotas
            ViewBag.imagen = "/Imagenes/" + pro.Titulo + ".jpg";
            if (pro.Solicitante == 0)
            {

                return View(pro);
            }
          
            return RedirectToAction("AltaProyecto", pro);
            //  return RedirectToAction("home", new { mensaje = textoMensaje });
        }
        public ActionResult AltaProyecto(Proyecto Alta)
        {
            string txtmensaje = "Se Confirmo el Proyecto con Exito";
        

            string usuario = Session["id"].ToString();
            int buscarusuario = int.Parse(usuario);
            Alta.Solicitante = buscarusuario;
           
            if (!FachadaExpress.AltaProyectoParcial(Alta))
            {
                txtmensaje = "Ocurrio un error, verifica los datos";
                return RedirectToAction("home", new { mensaje = txtmensaje });
            }
            return RedirectToAction("home", new { mensaje = txtmensaje });
        }
        #endregion

        public ActionResult VerProyecto(string titulo)
        {
            if (Session["rol"] == null || Session["rol"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }
            Proyecto verProyecto = FachadaExpress.buscarProyecto(titulo);


            #region calcular los montos y cuotas
            decimal traerinteres = FachadaExpress.ObtenerTasa(verProyecto.Cuotas); // trae el calculo de la tasa de interes %%
            traerinteres = 1 + traerinteres / 100;
            decimal montototal = verProyecto.Monto * traerinteres;     // Calcula el % de interes que se le va a sumar al monto
            ViewBag.montoconinteres = montototal;                               // se le pasa el monto total con interes a la vista

            montototal = decimal.Round(montototal, 2);
            decimal montoporcuota = montototal / verProyecto.Cuotas;
            montoporcuota = decimal.Round(montoporcuota, 2);
            ViewBag.montoconinteres = montototal;                               // se le pasa el monto total con interes a la vista
            ViewBag.montoporcuota = montoporcuota;
            #endregion
            ViewBag.imagen = "/Imagenes/" + verProyecto.Titulo + ".jpg";

            return View(verProyecto);
            //  return RedirectToAction("home", new { mensaje = textoMensaje });
        }

        [HttpPost]
        public ActionResult CambiarEstado(int id, int votacion)
        {
            string errormensaje = "ocurrio un error, intentelo de nuevo mas tarde";
            string estado = "";
            if (votacion > 10 || votacion < 0)
            {
                return RedirectToAction("Home", new { mensaje = errormensaje });
            }
            else if (votacion >= 0 && votacion < 6)
            {
                estado = "Rechazado";
            }
            else if (votacion >= 6 && votacion <= 10)
            {
                estado = "Aprobado";
            }
           if(FachadaExpress.Confirmarproyecto(estado, id))
            {
                string Admin = Session["cedula"].ToString();
                DateTime fecha = DateTime.Today;
                if (FachadaExpress.Guardarconfirmacion(id, votacion, Admin, fecha))
                {
                    errormensaje = " El proyecto fue evaluado, su estado es : " + estado;
                    return RedirectToAction("Home", new { mensaje = errormensaje });
                }
            }
            return RedirectToAction("Home", new { mensaje = errormensaje });
        }




        #region Filtrar Proyecto ADMIN
        public ActionResult FiltrarProyectos()
        {
            if (Session["rol"] == null || Session["rol"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }
            return View();
        }

        [HttpPost]
        public ActionResult FiltrarProyectos(string cedula, string estado, string palabra, DateTime? fecha)
        {
            string fe = "";
            if (fecha != null)
            {
                DateTime nueva = Convert.ToDateTime(fecha);

                string anio = nueva.Day.ToString();
                string mes = nueva.Month.ToString();
                string dia = nueva.Year.ToString();
                fe = dia + "-" + mes + "-" + anio;
            }
            List<Proyecto> lista = FachadaExpress.FiltrarProyectos(cedula, fe, estado, palabra);
            ViewBag.jpg = ".jpg";
            return View(lista);

        }
        #endregion

        #region Armar Texto
        public ActionResult CrearTexto()
        {
            if (Session["rol"] == null || Session["rol"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }
            ViewBag.Nombres = FachadaExpress.BuscarNombresTablas();

            return View();
        }

        [HttpPost]
        public ActionResult CrearTexto(string tablas)
        {
            string mensajedevuelto = "imposible exportar las tablas. intentelo de nuevo.";
            if (FachadaExpress.EscribirTabla(tablas))
            {
                mensajedevuelto = "la tabla se ha impreso correctamente";
                return RedirectToAction("Home", new { mensaje = mensajedevuelto });
            }

            return RedirectToAction("Home", new { mensaje = mensajedevuelto });
        }

        #endregion





    }
}
