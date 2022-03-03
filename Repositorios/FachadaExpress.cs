using Dominio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repositorios
{
    public static class FachadaExpress
    {
        #region USUARIO

        public static bool AltaUsuario(Usuario nuevoUsuario)
        {
            IRepositorio<Usuario> repoUsu = new RepoUsuarios();

            bool ret = false;
            Usuario usu = new Usuario()
            {
                Cedula = nuevoUsuario.Cedula,
                Nombre = nuevoUsuario.Nombre,
                Apellido = nuevoUsuario.Apellido,
                FechaNacimiento = nuevoUsuario.FechaNacimiento,
                Celular = nuevoUsuario.Celular,
                Email = nuevoUsuario.Email,
                Password = nuevoUsuario.Password,
                Rol = nuevoUsuario.Rol
            };
            if (usu.Password.Length >= 6 && ValidarPassword(usu.Password) && CelularValido(usu.Celular))
            {
                ret = repoUsu.Alta(usu);
            }

            return ret;
        } // el alta de usuario

        public static Usuario ObtenerUsuario(string cedula, string password)

        {

            RepoUsuarios repoUsu = new RepoUsuarios();
            Usuario usu = new Usuario();

            usu = repoUsu.ObtenerUsuario(cedula, password);
            return usu;


        } // el login

        public static bool ValidarPassword(string password)
        {
            bool esValido = false;

            if (password.Length >= 6)
            {

                bool Mayuscula = false;
                bool Minuscula = false;
                bool Numero = false;

                int i = 0;

                while (!esValido && i < password.Length)
                {
                    char charActual = password[i];

                    if (charActual == char.ToUpper(charActual) && !Char.IsNumber(charActual))
                    {
                        Mayuscula = true;
                    }
                    if (charActual == char.ToLower(charActual) && !Char.IsNumber(charActual))
                    {
                        Minuscula = true;
                    }
                    if (Char.IsNumber(charActual))
                    {
                        Numero = true;
                    }

                    if (Mayuscula && Minuscula && Numero)
                    {
                        esValido = true;
                    }
                    i++;
                }
            }
            return esValido;
        }

        public static bool CelularValido(string celular)
        {
            bool valido = false;
            char ch = celular[2];
            int intVal = (int)Char.GetNumericValue(ch);
            if (celular[0] == '0' && celular[1] == '9' && intVal > 0)
            {
                valido = true;
            }
            return valido;
        }

        public static bool NuevoCelular(string celular)
        {
            bool valido = false;
            bool correcto = CelularValido(celular);
            RepoUsuarios usu = new RepoUsuarios();
            bool unico = usu.CelularUnico(celular);
            if (correcto && unico)
            {
                valido = true;
            }
            return valido;
        }

        public static bool NuevoMail(string mail)
        {
            bool valido = false;
            RepoUsuarios usu = new RepoUsuarios();
            valido = usu.MailUnico(mail);
            return valido;
        }

        public static bool ValidarCedula(string cedula)
        {
            bool unica = false;
            RepoUsuarios usu = new RepoUsuarios();
            unica = usu.CedulaUnico(cedula);
            return unica;
        }
        #endregion


        public static List<Proyecto> TraerProyectos(int id)
        {

            RepoProyectos repoPro = new RepoProyectos();

            return repoPro.BuscarProyectos(id);

        } // trae todos los proyectos del USUARIO

        public static List<Proyecto> FiltrarProyectos(string IdUsuario, string Fecha, string Estado, string Texto)
        {
           RepoUsuarios usuario = new RepoUsuarios();
            int idUsuario = usuario.ObtenerId(IdUsuario);
            RepoProyectos repoPro = new RepoProyectos();

            return repoPro.FiltrarProyectos(idUsuario, Fecha, Estado, Texto);
        } // ADMIN puede filtrar y hacer un listado de proyectos



      
        // ALTA DE PROYECTO
        public static bool AltaProyectoParcial(Proyecto nuevoProyecto)
        {

            {
                RepoProyectos RepoProyecto = new RepoProyectos();
               int tasa = RepoProyecto.CalcularTasa(nuevoProyecto.Cuotas);
                bool ret = false;
                Proyecto nuevo = new Proyecto()
                {
                    Solicitante = nuevoProyecto.Solicitante,
                    Titulo = nuevoProyecto.Titulo,
                    Descripcion = nuevoProyecto.Descripcion,
                    Monto = nuevoProyecto.Monto,
                    Cuotas = nuevoProyecto.Cuotas,
                    Imagen = nuevoProyecto.Imagen,
                    TasaInteres = nuevoProyecto.TasaInteres,
                    Integrantes = nuevoProyecto.Integrantes,
                    Experiencia = nuevoProyecto.Experiencia,
                    Tipo = nuevoProyecto.Tipo,
                    Estado = nuevoProyecto.Estado,
                    FechaCreacion = nuevoProyecto.FechaCreacion
                    

                };// como hacemos el alta de cada proyecto 
                
                
              
                ret = RepoProyecto.Alta(nuevo);

                return ret;
            }
        } // el controlador llama a este metodo para dar de alta un proyecto, ya sea COOP o PERSONAL.

      

 

        public static Proyecto buscarProyecto(string titulo)
        {
            RepoProyectos repo = new RepoProyectos();
            Proyecto pro = repo.VerProyecto(titulo);

            return pro;
        } // es cuando el admin entra para ver "detalladamente" un proyecto



        #region TEXTO
        public static bool EscribirTabla(string tabla)//Recibe un String con el nombre de la tabla, retorna un bool cuando termina
        {
            bool termino = true;
            RepoUsuarios usu = new RepoUsuarios();
            List<string> datos = usu.ObtenerDatosTabla(tabla);
            string rutaAplicacion = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = tabla + ".txt";
            string rutaCompleta = Path.Combine(rutaAplicacion, nombreArchivo);
            FileStream fs = new FileStream(rutaCompleta, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (string linea in datos)
            {
                sw.Write(linea);
            }
            sw.Close();
            return termino;
        }
        public static List<string> BuscarNombresTablas()
        {
            RepoUsuarios usu = new RepoUsuarios();
            List<string> nombres = usu.ObtenerNombreTabla();
            return nombres;
        }
        #endregion





        #region Validaciones
     
        public static bool ProyectosPendientes (string cedula)
        {
            RepoProyectos RepoProyecto = new RepoProyectos();

            return RepoProyecto.ProyectosPendientes(cedula);
        }
        public static int ObtenerTasa(int cuotas) {
            int tasa;
            RepoProyectos RepoProyecto = new RepoProyectos();
        tasa = RepoProyecto.CalcularTasa(cuotas);

            return tasa;
        }           

        public static bool NuevoTitulo(string titulo)
        {
            bool valido = false;
            RepoProyectos pro = new RepoProyectos();
            valido = pro.TituloUnico(titulo);
            return valido;
        }     
        public static decimal TraerMontoMax()
        {
            decimal max = 0;
            RepoProyectos pro = new RepoProyectos();
            max = pro.ObtenerMontoMax();
            return max;
        }
        public static decimal TraerMontoMin()
        {
            decimal min = 0;
            RepoProyectos pro = new RepoProyectos();
            min = pro.ObtenerMontoMin();
            return min;
        }
        public static int TraerCuotaMin()
        {
            int min = 0;
            RepoProyectos pro = new RepoProyectos();
            min = pro.ObtenerCuotaMin();
            return min;
        }
        public static int TraerCuotaMax()
        {
            int max = 0;
            RepoProyectos pro = new RepoProyectos();
            max = pro.ObtenerCuotaMax();
            return max;
        }
        public static bool TienePendiente(string cedula)
        {
            bool pendiente = false;
            RepoProyectos pro = new RepoProyectos();
            pendiente = pro.ProyectosPendientes(cedula);
            return pendiente;
        }
        public static bool ValidarMontoPersonal(decimal montosolicitado)
        {
            int bono = 20;
          
            decimal maximo = TraerMontoMax();
            decimal montoMax = maximo - (maximo * bono) / 100;

            return montosolicitado < montoMax && montosolicitado > TraerMontoMin();
        }
        public static bool ValidarMontoCooperativo(int integrantes, decimal montosolicitado)
        {
            int bono = 20;
           
            if (integrantes <= 10)
            {
                bono = 2 * integrantes;

            }
            decimal montoMax = TraerMontoMax();
            decimal montoMin = TraerMontoMin();
            decimal nuevoMontoMax = montoMax + (montoMax * bono) / 100;
            return montosolicitado < nuevoMontoMax && montosolicitado > montoMin;

        }

        public static bool Confirmarproyecto(string estado, int id)
        {
            RepoProyectos repo = new RepoProyectos();
            return repo.Confirmarproyecto(estado,id);
        }
        public static bool ValidarCuotas(int cuotas)
        {
            return cuotas > TraerCuotaMin() && cuotas < TraerCuotaMax();
        }
        
        public static bool Guardarconfirmacion(int id, int votacion, string Admin,  DateTime fecha)
        {
            RepoProyectos repo = new RepoProyectos();

            return repo.GuardarAprobacion(id, votacion, Admin, fecha);
        }
        #endregion
    }
}
