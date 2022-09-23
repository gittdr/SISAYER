using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Models
{
    public class storedProcedure
    {
        private string Conexion;
        public SqlConnection conexion;
        public SqlCommand comando = null;
        public SqlDataReader resultados = null;
        private string conn;
        private SqlDataAdapter objAdapter;

        public storedProcedure(string conn)
        {
            this.conn = conn;
        }
        public void establecerConexion()
        {
            conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[conn].ConnectionString);
        }


        #region ejecutaSQL
        public bool ejecutaSQL(string query)
        {
            establecerConexion();
            comando = new SqlCommand(query, conexion);
            comando.CommandType = CommandType.Text;

            try
            {
                conexion.Open();
                comando.ExecuteReader();
                conexion.Close();

                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }

        }
        #endregion

        public string recuperaValor(string query)
        {
            establecerConexion();
            comando = new SqlCommand(query, conexion);
            comando.CommandType = CommandType.Text;
            string resultado = "";
            try
            {
                conexion.Open();
                resultados = comando.ExecuteReader();
                if (resultados.HasRows)
                {
                    while (resultados.Read())
                    {
                        resultado = resultados.GetValue(0).ToString();
                    }
                }
                conexion.Close();

                return resultado;
            }
            catch
            {

                return "-1";
            }
            finally
            {
                conexion.Dispose();
                conexion.Close();
            }
        }
        public List<string> recuperaRegistros(string query)
        {
            List<string> resultado = new List<string>();
            establecerConexion();
            comando = new SqlCommand(query, conexion);
            comando.CommandType = CommandType.Text;
            try
            {
                conexion.Open();
                resultados = comando.ExecuteReader();
                if (resultados.HasRows)
                {
                    while (resultados.Read())
                    {
                        for (int i = 0; i < resultados.FieldCount; i++)
                        {
                            resultado.Add(resultados.GetValue(i).ToString());
                        }
                    }
                }
                conexion.Close();

                return resultado;
            }
            catch
            {

                return null;
            }
            finally
            {
                conexion.Dispose();
                conexion.Close();
            }
        }


        #region ObtieneTabla
        public DataTable ObtieneTabla(string query)
        {
            DataTable dt = new DataTable();
            establecerConexion();
            try
            {
                comando = new SqlCommand(query, conexion);
                comando.CommandType = CommandType.Text;
                comando.CommandTimeout = 30000;
                conexion.Open();

                SqlDataAdapter da = new SqlDataAdapter(comando);

                da.Fill(dt);
            }
            catch
            {

            }
            return dt;
        }
        #endregion
        #region Obtienedataset
        public DataSet ObtieneDataSet(string query)
        {
            DataSet dt = new DataSet();
            establecerConexion();
            try
            {
                comando = new SqlCommand(query, conexion);
                comando.CommandType = CommandType.Text;
                comando.CommandTimeout = 30000;
                conexion.Open();

                SqlDataAdapter da = new SqlDataAdapter(comando);

                da.Fill(dt);
            }
            catch
            {

            }
            return dt;
        }
        #endregion


        #region Procedimientos 

        public bool restablecerCon(string claveUs, string nuevaConEn)
        {
            establecerConexion();
            comando = new SqlCommand("Proc_cambiarpass", conexion);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@correo", SqlDbType.VarChar).Value = claveUs;
            comando.Parameters.Add("@nuevaContra", SqlDbType.VarChar).Value = nuevaConEn;

            try
            {
                conexion.Open();
                comando.ExecuteReader();
                conexion.Close();
                return true;
            }
            catch (SqlException ex)
            {
                Console.Write(ex);
                return false;
            }
        }

        public bool MetRegUsuario(string nombre, string correo, string telefono, string pass)
        {
            establecerConexion();
            comando = new SqlCommand("Proc_InserLogUsu", conexion);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
            comando.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;
            comando.Parameters.Add("@telefono", SqlDbType.VarChar).Value = telefono;
            comando.Parameters.Add("@password", SqlDbType.VarChar).Value = pass;
            try
            {
                conexion.Open();
                comando.ExecuteReader();
                conexion.Close();
                return true;
            }
            catch (SqlException ex)
            {
                Console.Write(ex);
                return false;
            }
        }

        public List<string> logvalidausu(string correo)
        {
            List<string> resultado = new List<string>();
            establecerConexion();
            comando = new SqlCommand("Prog_ValUsu", conexion);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@email", SqlDbType.VarChar).Value = correo;
            try
            {
                conexion.Open();
                resultados = comando.ExecuteReader();
                if (resultados.HasRows)
                {
                    while (resultados.Read())
                    {
                        for (int i = 0; i < resultados.FieldCount; i++)
                        {
                            resultado.Add(resultados.GetValue(i).ToString());
                        }
                    }
                }
                conexion.Close();
                return resultado;
            }
            catch (SqlException ex)
            {
                Console.Write(ex);
                return null;
            }
        }

        public string MetCarrito(int op, string folio, int idusu, int idart, string talla, int num, float total, string cupon, string token, string paypalid)
        {
            string resultado = "";
            establecerConexion();
            comando = new SqlCommand("Proc_Carrito", conexion);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@op", SqlDbType.Int).Value = op;
            comando.Parameters.Add("@folio", SqlDbType.VarChar).Value = folio;
            comando.Parameters.Add("@idusu", SqlDbType.Int).Value = idusu;
            comando.Parameters.Add("@idart", SqlDbType.Int).Value = idart;
            comando.Parameters.Add("@talla", SqlDbType.VarChar).Value = talla;
            comando.Parameters.Add("@num", SqlDbType.Int).Value = num;
            comando.Parameters.Add("@total", SqlDbType.Decimal).Value = total;
            comando.Parameters.Add("@cupon", SqlDbType.VarChar).Value = cupon;
            comando.Parameters.Add("@token", SqlDbType.VarChar).Value = token;
            comando.Parameters.Add("@paypalid", SqlDbType.VarChar).Value = paypalid;
            try
            {
                conexion.Open();
                resultados = comando.ExecuteReader();
                if (resultados.HasRows)
                {
                    while (resultados.Read())
                    {
                        resultado = resultados.GetValue(0).ToString();
                    }
                }
                conexion.Close();
                return resultado;
            }
            catch (SqlException ex)
            {
                Console.Write(ex);
                return null;
            }
        }

        public string MetAlmacen(int op, int codigo, string foto1, string foto2, string foto3, string foto4, string foto5, string genero, string marca, string material, string talla1, string talla2, string talla3, string talla4, string talla5, string talla6, string articulo, string descrip, int unidad, float precio, int usu)
        {
            string resultado = "";
            establecerConexion();
            comando = new SqlCommand("ProcAlmacen", conexion);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@op", SqlDbType.Int).Value = op;
            comando.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;
            comando.Parameters.Add("@foto1", SqlDbType.VarChar).Value = foto1;
            comando.Parameters.Add("@foto2", SqlDbType.VarChar).Value = foto2;
            comando.Parameters.Add("@foto3", SqlDbType.VarChar).Value = foto3;
            comando.Parameters.Add("@foto4", SqlDbType.VarChar).Value = foto4;
            comando.Parameters.Add("@foto5", SqlDbType.VarChar).Value = foto5;
            comando.Parameters.Add("@genero", SqlDbType.VarChar).Value = genero;
            comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = marca;
            comando.Parameters.Add("@material", SqlDbType.VarChar).Value = material;
            comando.Parameters.Add("@talla1", SqlDbType.VarChar).Value = talla1;
            comando.Parameters.Add("@talla2", SqlDbType.VarChar).Value = talla2;
            comando.Parameters.Add("@talla3", SqlDbType.VarChar).Value = talla3;
            comando.Parameters.Add("@talla4", SqlDbType.VarChar).Value = talla4;
            comando.Parameters.Add("@talla5", SqlDbType.VarChar).Value = talla5;
            comando.Parameters.Add("@talla6", SqlDbType.VarChar).Value = talla6;
            comando.Parameters.Add("@articulo", SqlDbType.VarChar).Value = articulo;
            comando.Parameters.Add("@descrip", SqlDbType.VarChar).Value = descrip;
            comando.Parameters.Add("@unidad", SqlDbType.VarChar).Value = unidad;
            comando.Parameters.Add("@precio", SqlDbType.Float).Value = precio;
            comando.Parameters.Add("@idusu", SqlDbType.Int).Value = usu;
            try
            {
                conexion.Open();
                resultados = comando.ExecuteReader();
                if (resultados.HasRows)
                {
                    while (resultados.Read())
                    {
                        resultado = resultados.GetValue(0).ToString();
                    }
                }
                conexion.Close();
                return resultado;
            }
            catch (SqlException ex)
            {
                Console.Write(ex);
                return null;
            }
        }


        public string MetCompras(int op, string folio, string token, string paypalid, string status, string paqueteria, string numseg, string nomcom, string correocom, string telcom, string estado, string muni, int cp, string dir, string refe, string nomrec, string telrec, int idcom, int idven, string nomven, string correoven, string telven)
        {
            string resultado = "";
            establecerConexion();
            comando = new SqlCommand("ProcCompra", conexion);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@op", SqlDbType.Int).Value = op;
            comando.Parameters.Add("@folio", SqlDbType.VarChar).Value = folio;
            comando.Parameters.Add("@token", SqlDbType.VarChar).Value = token;
            comando.Parameters.Add("@paypalid", SqlDbType.VarChar).Value = paypalid;
            comando.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
            comando.Parameters.Add("@paqueteria", SqlDbType.VarChar).Value = paqueteria;
            comando.Parameters.Add("@numseg", SqlDbType.VarChar).Value = numseg;
            comando.Parameters.Add("@nomcom", SqlDbType.VarChar).Value = nomcom;
            comando.Parameters.Add("@correocom", SqlDbType.VarChar).Value = correocom;
            comando.Parameters.Add("@telcom", SqlDbType.VarChar).Value = telcom;
            comando.Parameters.Add("@estado", SqlDbType.VarChar).Value = estado;
            comando.Parameters.Add("@mun", SqlDbType.VarChar).Value = muni;
            comando.Parameters.Add("@cp", SqlDbType.Int).Value = cp;
            comando.Parameters.Add("@dir", SqlDbType.VarChar).Value = dir;
            comando.Parameters.Add("@ref", SqlDbType.VarChar).Value = refe;
            comando.Parameters.Add("@nomrec", SqlDbType.VarChar).Value = nomrec;
            comando.Parameters.Add("@telrec", SqlDbType.VarChar).Value = telrec;
            comando.Parameters.Add("@idcom", SqlDbType.Int).Value = idcom;
            comando.Parameters.Add("@idven", SqlDbType.Int).Value = idven;
            comando.Parameters.Add("@nomven", SqlDbType.VarChar).Value = nomven;
            comando.Parameters.Add("@correoven", SqlDbType.VarChar).Value = correoven;
            comando.Parameters.Add("@telven", SqlDbType.VarChar).Value = telven;
            try
            {
                conexion.Open();
                resultados = comando.ExecuteReader();
                if (resultados.HasRows)
                {
                    while (resultados.Read())
                    {
                        resultado = resultados.GetValue(0).ToString();
                    }
                }
                conexion.Close();
                return resultado;
            }
            catch (SqlException ex)
            {
                Console.Write(ex);
                return null;
            }
        }

        public string MetEnvio(int idusu, string estado, string muni, int cp, string dir, string refe, string nombre, string tel)
        {
            string resultado = "";
            establecerConexion();
            comando = new SqlCommand("ProcEnvio", conexion);

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@idusu", SqlDbType.Int).Value = idusu;
            comando.Parameters.Add("@estado", SqlDbType.VarChar).Value = estado;
            comando.Parameters.Add("@munici", SqlDbType.VarChar).Value = muni;
            comando.Parameters.Add("@cp", SqlDbType.Int).Value = cp;
            comando.Parameters.Add("@direccion", SqlDbType.VarChar).Value = dir;
            comando.Parameters.Add("@refe", SqlDbType.VarChar).Value = refe;
            comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
            comando.Parameters.Add("@tel", SqlDbType.VarChar).Value = tel;
            try
            {
                conexion.Open();
                resultados = comando.ExecuteReader();
                if (resultados.HasRows)
                {
                    while (resultados.Read())
                    {
                        resultado = resultados.GetValue(0).ToString();
                    }
                }
                conexion.Close();
                return resultado;
            }
            catch (SqlException ex)
            {
                Console.Write(ex);
                return null;
            }
        }
        #endregion
    }
}
