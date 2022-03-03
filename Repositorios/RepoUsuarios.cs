using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Repositorios
{
    public class RepoUsuarios : IRepositorio<Usuario>
    {
        public string StringConexion { get; set; } = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

        public bool Alta(Usuario obj)
        {
            bool ret = false;

            SqlConnection con = new SqlConnection(this.StringConexion);

            string sql = "insert into Usuario(Cedula, Nombre, Apellido, Pass, Fecha_Nacimiento, Celular, Email, Rol) values(@ced, @nom, @ape, @pass, @nac, @cel, @mail, @rol);";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@ced", obj.Cedula);
            com.Parameters.AddWithValue("@nom", obj.Nombre);
            com.Parameters.AddWithValue("@ape", obj.Apellido);
            com.Parameters.AddWithValue("@pass", obj.Password);
            com.Parameters.AddWithValue("@nac", obj.FechaNacimiento);
            com.Parameters.AddWithValue("@cel", obj.Celular);
            com.Parameters.AddWithValue("@mail", obj.Email);
            com.Parameters.AddWithValue("@rol", obj.Rol);
            try
            {
                con.Open();
                int afectadas = com.ExecuteNonQuery();
                con.Close();

                ret = afectadas == 1;
            }
            catch
            {
                con.Close();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }

            return ret;
        }

        public Usuario ObtenerUsuario(string cedula, string password)
        {
            Usuario ret = new Usuario();

            SqlConnection con = new SqlConnection(this.StringConexion);

            string sql = " SELECT * FROM Usuario WHERE @cedula = Cedula AND @pass = Pass";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@cedula", cedula);
            com.Parameters.AddWithValue("@pass", password);

            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    DateTime fecha = reader.GetDateTime(5);
                    
                  
                    int id = int.Parse(reader["UsuarioId"].ToString());

                        ret.Id = id;
                        ret.Cedula = reader["Cedula"].ToString();
                        ret.Nombre = reader["Nombre"].ToString();
                        ret.Apellido = reader["Apellido"].ToString();
                        ret.FechaNacimiento = fecha;
                        ret.Celular = reader["Celular"].ToString(); ;
                        ret.Email = reader["Email"].ToString();
                        ret.Password = reader["Pass"].ToString();
                        ret.Rol = reader["Rol"].ToString();
                    


                   // Console.WriteLine("Nombre:  "+ret.Nombre +"    Id:  "+ reader["Id"].ToString());
                    
                }

                con.Close();

            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }

            return ret;
        }

        public int ObtenerId(string cedula)
        {
            int idusuario = 0;

            SqlConnection con = new SqlConnection(this.StringConexion);

            string sql = " SELECT UsuarioId FROM Usuario WHERE @cedula = Cedula";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@cedula", cedula);

            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    idusuario = int.Parse(reader["UsuarioId"].ToString()); 
                }

                con.Close();

            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }

            return idusuario;
        }

        #region TEXTO
        public List<string> ObtenerDatosTabla(string tabla) //Recibe una string nombre tabla y devuelve una lista de strings
        {
            List<string> lista = new List<string>();
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT * FROM " + tabla;
            SqlCommand com = new SqlCommand(sql, con);

            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    string linea = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        linea += reader.GetValue(i).ToString();
                        if (i != reader.FieldCount - 1)
                        {
                            linea += " | ";
                        }
                        else
                        {
                            linea += "\r\n";
                        }

                    }
                    lista.Add(linea);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return lista;
        }
        public List<string> ObtenerNombreTabla()
        {
            List<string> lista = new List<string>();
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT TABLE_NAME FROM PrestamosExpress.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME != 'sysdiagrams';";
            SqlCommand com = new SqlCommand(sql, con);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    string linea = reader.GetValue(0).ToString();

                    lista.Add(linea);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return lista;
        }
        #endregion

        public bool MailUnico(string mail) //Consulta Si existe mail ingresado en la columna email de tabla usuario
        {
            bool unico = false;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Email FROM Usuario WHERE Email = @mail";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@mail", mail);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                if (!reader.HasRows)
                {
                    unico = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return unico;
        }
        public bool UsuarioMayor(string cedula) //Compara fecha nacimiento usuario con fecha de hoy - 21 años
        {
            bool mayor = false;
            DateTime hoy = DateTime.Today;
            DateTime limite = hoy.AddYears(-21);
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Fecha_Nacimiento FROM Usuario WHERE Cedula = @cedula";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@cedula", cedula);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DateTime t = reader.GetDateTime(0);
                    if (t <= limite)
                    {
                        mayor = true;
                    }

                }
                con.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }

            return mayor;
        }
        public bool CelularUnico(string celular) //Consulta Si existe Celular ingresado en la columna Celular de tabla usuario
        {
            bool unico = false;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Celular FROM Usuario WHERE Celular = @celular";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@celular", celular);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                if (!reader.HasRows)
                {
                    unico = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return unico;
        }
        public bool CedulaUnico(string cedula) //Consulta Si existe Cedula ingresada en la columna Cedula de tabla usuario
        {
            bool unico = false;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Cedula FROM Usuario WHERE Cedula = @cedula";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@cedula", cedula);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                if (!reader.HasRows)
                {
                    unico = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return unico;
        }



        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> TraerTodo()
        {
            throw new NotImplementedException();
        }
    }
}
