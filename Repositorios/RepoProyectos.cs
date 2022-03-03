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
    class RepoProyectos : IRepositorio<Proyecto>
    {

        public string StringConexion { get; set; } = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;


        public bool Alta(Proyecto obj)
        {
            bool ret = false;




            SqlConnection con = new SqlConnection(this.StringConexion);
            string imagenBD = obj.Titulo + ".jpg";
            string sql = "insert into Proyecto(Solicitante, Titulo, Fecha_Creacion, Descripcion, Monto_Solicitado, Cuotas, Imagen, Tasa_Interes, Estado, Integrantes, Experiencia, Tipo) values(@sol, @tit, @fecre, @des, @mon, @cuo, @img, @tas, @est, @inte, @exp, @tipo);";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@sol", obj.Solicitante);
            com.Parameters.AddWithValue("@tit", obj.Titulo);
            com.Parameters.AddWithValue("@fecre", obj.FechaCreacion);
            com.Parameters.AddWithValue("@des", obj.Descripcion);
            com.Parameters.AddWithValue("@mon", obj.Monto);
            com.Parameters.AddWithValue("@cuo", obj.Cuotas);
            com.Parameters.AddWithValue("@img", imagenBD);
            com.Parameters.AddWithValue("@tas", obj.TasaInteres);
            com.Parameters.AddWithValue("@est", obj.Estado);
            com.Parameters.AddWithValue("@inte", obj.Integrantes);
            com.Parameters.AddWithValue("@exp", obj.Experiencia);
            com.Parameters.AddWithValue("@tipo", obj.Tipo);


            try
            {
                con.Open();
                int afectadas = com.ExecuteNonQuery();
                con.Close();

                ret = afectadas == 1;
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

        public bool ProyectosPendientes(string cedula)  //Si es true => tiene Pendientes
        {
            bool pendiente = false;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT * FROM Usuario u, Proyecto p WHERE u.Cedula = @cedula AND u.UsuarioId = p.Solicitante AND p.Estado = 'Pendiente de revisión'";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@cedula", cedula);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    pendiente = true;
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
            return pendiente;
        }


        public int CalcularTasa(int Cuotas)
        {
            int tasa =0;
            
            SqlConnection con = new SqlConnection(this.StringConexion);

            string sql = " SELECT Interes FROM Tasa_Interes WHERE @cuotas >= Desde AND @cuotas <= Hasta";
            SqlCommand com = new SqlCommand(sql, con);
           com.Parameters.AddWithValue("@cuotas", Cuotas);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    string t = reader.GetValue(0).ToString();
                    tasa = int.Parse(t);
                 
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

            return tasa;
        }
   

        public List<Proyecto> BuscarProyectos (int id)
        {
            List<Proyecto> aux = new List<Proyecto>();

            SqlConnection con = new SqlConnection(this.StringConexion);

            string sql = "select * from Proyecto WHERE Solicitante = @id;";

            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@id", id);


            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Proyecto pro = AltaAux(reader);
                    aux.Add(pro);
                }

                con.Close();
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

                return aux;
        }


   
        private Proyecto AltaAux(SqlDataReader reader)
        {
            string monto = reader["Monto_Solicitado"].ToString();
            string cuotas = reader["Cuotas"].ToString();
            string tasa = reader["Tasa_Interes"].ToString();
            string integrantes = reader["Integrantes"].ToString();
            string solicitante = reader["Solicitante"].ToString();
            string id = reader["ProyectoId"].ToString();
            DateTime nueva = Convert.ToDateTime(reader["Fecha_Creacion"].ToString());

            Proyecto pro = new Proyecto();

            pro.Id = int.Parse(id);
            pro.Titulo = reader["Titulo"].ToString();
            pro.Descripcion = reader["Descripcion"].ToString();
            pro.Imagen = reader["Imagen"].ToString();
            pro.Estado = reader["Estado"].ToString();
            pro.Monto = decimal.Parse(monto);
            pro.Cuotas = int.Parse(cuotas);
            pro.FechaCreacion = nueva;

            //Estado = reader.GetString(3),
            pro.Experiencia = reader["Experiencia"].ToString();
            pro.Integrantes = int.Parse(integrantes);
            pro.Solicitante = int.Parse(solicitante);
            pro.Tipo = reader["Tipo"].ToString();
            pro.TasaInteres = decimal.Parse(tasa);


            return pro;
        }


        public Proyecto VerProyecto(string Titulo)
        {
            SqlConnection con = new SqlConnection(this.StringConexion);
        
            string sql = "SELECT * FROM Proyecto WHERE Titulo = @tit ;";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@tit", Titulo);
            Proyecto pro = new Proyecto();

            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                   pro = AltaAux(reader);
                   
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

            return pro;
        
    }
        // ADMIN



        public List<Proyecto> FiltrarProyectos(int IdUsuario, string Fecha, string Estado, string Texto)
        {
            List<Proyecto> aux = new List<Proyecto>();
            SqlConnection con = new SqlConnection(this.StringConexion);
            string where = "WHERE ";
            if(IdUsuario == 0 && Fecha == "" && Estado == "" && Texto == "")
            {
                where = "";
            }
            string c = "";
            if (IdUsuario != 0)
            {   
                c = "Solicitante = "+IdUsuario;
            }

            if (Fecha != "")
            {   if(c != "") { c += " AND "; };
                c += "Fecha_Creacion = '"+Fecha+"'";
            }
            if (Estado != "")
            {
                if (c != "") { c += " AND "; };
                c += "Estado = '" + Estado + "'";
            }
            if (Texto != "")
            {
                if (c != "") { c += " AND "; };
                c += "Titulo LIKE '%" + Texto+ "%' OR Descripcion LIKE '%" + Texto + "%'";
            }
            if (Fecha != "")
            {
                c += "order by Convert(DATE, Fecha_Creacion)";
            }
   

            string sql = " SELECT * FROM Proyecto "+where + c;

            SqlCommand com = new SqlCommand(sql, con);
          

            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Proyecto pro = AltaAux(reader);
                    aux.Add(pro);
                }

                con.Close();
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

            return aux;
        }

        public bool Confirmarproyecto(string Estado, int Id)
        {
            bool ret = false;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "UPDATE Proyecto SET Estado = '"+Estado+"' WHERE ProyectoId = @Id";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@Id", Id);
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

        public bool GuardarAprobacion (int id, int votacion, string Admin, DateTime fecha)
        {
            bool ret = false;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "insert into Aprobacion(ProyectoId, Puntaje, Administrador, Fecha_Aprobacion) values(@id, @vot, @adm, @fecha);";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@id", id);
            com.Parameters.AddWithValue("@vot", votacion);
            com.Parameters.AddWithValue("@adm", Admin);
            com.Parameters.AddWithValue("@fecha", fecha);
            try
            {
                con.Open();
                int afectadas = com.ExecuteNonQuery();
                con.Close();

                ret = afectadas == 1;
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
   


        public bool TituloUnico(string titulo) //Consulta Si existe Titulo ingresado en la columna Titulo de tabla Proyecto
        {
            bool unico = false;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Titulo FROM Proyecto WHERE Titulo = @titulo";
            SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.AddWithValue("@titulo", titulo);
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

        #region Validar Montos // Monto max/min y comparacion con monto ingresado
        public decimal ObtenerMontoMax() //Consulta sql trae monto maximo
        {
            decimal max = 0;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Monto_Max FROM Financiamiento";
            SqlCommand com = new SqlCommand(sql, con);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    string t = reader.GetValue(0).ToString();
                    max = decimal.Parse(t);
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
            return max;
        }
        public decimal ObtenerMontoMin() //Consulta sql trae monto minimo
        {
            decimal min = 0;
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Monto_Min FROM Financiamiento";
            SqlCommand com = new SqlCommand(sql, con);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    string t = reader.GetValue(0).ToString();
                    min = decimal.Parse(t);
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
            return min;
        }

        #endregion


        #region Validar Cuotas // rango cuota min/max y comparacion con cuotas ingresadas
        public int ObtenerCuotaMin() // Primer valor en columna Desde
        {
            int min = 0;
            List<int> aux = new List<int>();
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Desde FROM Tasa_Interes";
            SqlCommand com = new SqlCommand(sql, con);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    string t = reader.GetValue(0).ToString();
                    int valor = int.Parse(t);
                    aux.Add(valor);
                }
                con.Close();
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
            min = aux[0];
            return min;
        }
        public int ObtenerCuotaMax() // Ultimo valor en la columna Hasta
        {
            int max = 0;
            List<int> aux = new List<int>();
            SqlConnection con = new SqlConnection(this.StringConexion);
            string sql = "SELECT Hasta FROM Tasa_Interes";
            SqlCommand com = new SqlCommand(sql, con);
            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    string t = reader.GetValue(0).ToString();
                    int valor = int.Parse(t);
                    aux.Add(valor);
                }
                con.Close();
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
            max = aux[aux.Count - 1];
            return max;
        }

        #endregion
        






















        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(Proyecto obj)
        {
            throw new NotImplementedException();
        }

        public List<Proyecto> TraerTodo()
        {
            throw new NotImplementedException();
        }

     
    }
}
