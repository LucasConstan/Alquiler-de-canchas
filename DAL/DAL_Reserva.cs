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
    public class DAL_Reserva
    {
        public void GuardarReserva(Reservas reserva)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "INSERT INTO Reservas (Fecha, Hora, Num_cancha, DNI) VALUES (@Fecha, @Hora, @Num_cancha, @DNI)";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@Fecha", reserva.Fecha.Date);
                    cmd.Parameters.AddWithValue("@Hora", reserva.Hora);
                    cmd.Parameters.AddWithValue("@Num_cancha", reserva.Num_cancha);
                    cmd.Parameters.AddWithValue("@DNI", reserva.DNI);
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocurrió un error al guardar la reserva: " + ex.Message);
                }
            }
        }

        public List<Reservas> Listar()
        {
            List<Reservas> lista = new List<Reservas>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT Id_Reserva, Fecha, Hora, Num_cancha, DNI FROM Reservas";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reservas()
                            {
                                Id = Convert.ToInt32(dr["Id_Reserva"]),
                                Fecha = Convert.ToDateTime(dr["Fecha"]),
                                Hora = (TimeSpan)dr["Hora"],
                                Num_cancha = Convert.ToInt32(dr["Num_cancha"]),
                                DNI = Convert.ToInt64(dr["DNI"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocurrió un error al listar las reservas: " + ex.Message);
                }
            }
            return lista;
        }

        public List<Reservas> ObtenerReservasPorCanchaYFecha(long canchaId, DateTime fecha) 
        {
            List<Reservas> lista = new List<Reservas>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT Id_Reserva, Fecha, Hora, Num_cancha, DNI FROM Reservas WHERE Num_cancha = @Num_cancha AND Fecha = @Fecha"; // CAMBIO
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@Num_cancha", canchaId);
                    cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reservas()
                            {
                                Id = Convert.ToInt32(dr["Id_Reserva"]),
                                Fecha = Convert.ToDateTime(dr["Fecha"]),
                                Hora = (TimeSpan)dr["Hora"],
                                Num_cancha = Convert.ToInt64(dr["Num_cancha"]),
                                DNI = Convert.ToInt64(dr["DNI"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocurrió un error al listar las reservas: " + ex.Message);
                }
            }
            return lista;
        }

        public void CancelarReserva(long canchaId, DateTime fecha, TimeSpan hora)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "DELETE FROM Reservas WHERE Num_cancha = @Num_cancha AND Fecha = @Fecha AND Hora = @Hora";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@Num_cancha", canchaId);
                    cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                    cmd.Parameters.AddWithValue("@Hora", hora);
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocurrió un error al cancelar la reserva: " + ex.Message);
                }
            }
        }
    }



}