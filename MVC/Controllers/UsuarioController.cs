using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Repositorios;
using MVC.Models;

namespace MVC.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Login(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            Session.Abandon();
            return View();
            
        }

        [HttpPost]
        public ActionResult Login(string cedula, string password)
        {
            string errormensaje = "";
            Usuario unU = FachadaExpress.ObtenerUsuario(cedula, password);
            if (unU.Id == 0)
            {
                errormensaje = "Usuario o contraseña incorrectos";
                return RedirectToAction("Login", new { mensaje = errormensaje });

            }

            Session["rol"] = unU.Rol;
            Session["cedula"] = unU.Cedula;
            Session["id"] = unU.Id;


            return Redirect("/Home/home");
        }


        public ActionResult AltaUsuario(string mensaje)
        {
            ViewBag.mensaje = mensaje;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AltaUsuario(ViewModelUsuario nuevoUsuario, string pass)
        {
            if (ModelState.IsValid) { 
                nuevoUsuario.Rol = "Solicitante";
               if (pass == nuevoUsuario.Password)
                    {
                        return RedirectToAction("Nuevo", nuevoUsuario);
                        // bool ret = FachadaExpress.AltaUsuario(nuevoUsuario);
                    }
            }
            return View(nuevoUsuario);

        }

        public ActionResult Nuevo(Usuario nuevoUsuario)
        {
            nuevoUsuario.Rol = "Solicitante";
           if(FachadaExpress.AltaUsuario(nuevoUsuario))
          { return RedirectToAction("Login", new { mensaje = "Se dio de alta con Exito" }); }

            return RedirectToAction("AltaUsuario", new { mensaje = "Verifique los datos" });
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("login");
        }
    }
}
