using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Entidades;
using System.Data;
using Servicios;

namespace DAL
{
    public class DAL_Cancha
    {
        public List<Canchas> Listar()
        {
            List<Canchas> lista = new List<Canchas>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "select Num_cancha,Tamaño,Cesped,Estado,Precio from Canchas";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Canchas()
                            {
                                Num_cancha = Convert.ToInt64(dr["Num_cancha"]),
                                Tamaño = dr["Tamaño"].ToString(),
                                Cesped = dr["Cesped"].ToString(),
                                Estado = dr["Estado"].ToString(),
                                Precio = Convert.ToInt64(dr["Precio"]),
                               
                            });

                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<Canchas>();
                }
            }

            return lista;
        }

        public void InsertarCancha(Canchas cancha)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {

                conn.Open();
                string query = "INSERT INTO Canchas (Num_cancha,Tamaño,Cesped,Estado,Precio) VALUES (@Num_cancha,@Tamaño,@Cesped,@Estado,@Precio)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Num_cancha", cancha.Num_cancha);
                    cmd.Parameters.AddWithValue("@Tamaño", cancha.Tamaño);
                    cmd.Parameters.AddWithValue("@Cesped", cancha.Cesped);
                    cmd.Parameters.AddWithValue("@Estado", cancha.Estado);
                    cmd.Parameters.AddWithValue("@Precio", cancha.Precio);
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public void EliminarCancha(long num_cancha)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "DELETE FROM Canchas WHERE Num_cancha = @Num_cancha";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Num_cancha", num_cancha);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarCancha(Canchas cancha)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "UPDATE Canchas SET Tamaño = @Tamaño, Cesped = @Cesped, Estado = @Estado, Precio = @Precio WHERE Num_cancha = @Num_cancha";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Num_cancha", cancha.Num_cancha);
                    cmd.Parameters.AddWithValue("@Tamaño", cancha.Tamaño);
                    cmd.Parameters.AddWithValue("@Cesped", cancha.Cesped);
                    cmd.Parameters.AddWithValue("@Estado", cancha.Estado);
                    cmd.Parameters.AddWithValue("@Precio", cancha.Precio);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<long> ObtenerNumCancha()
        {
            List<long> lista = new List<long>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT Num_cancha FROM Canchas";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(Convert.ToInt64(dr["Num_cancha"]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción (podrías registrarla o lanzarla nuevamente)
                    // Aquí se deja la lista vacía en caso de error, pero quizás quieras manejarlo de otra forma
                    lista = new List<long>();
                }
            }
            return lista;
        }
    }
}
