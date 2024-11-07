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
    public class DAL_Evento
    {
        public void InsertarEvento(Evento evento)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {

                conn.Open();
                string query = "INSERT INTO Eventos (Usuario, Fecha, Horario, Modulo, Evento, Criticidad) VALUES (@Usuario, @Fecha, @Horario, @Modulo, @Evento, @Criticidad)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", evento.Usuario);
                    cmd.Parameters.AddWithValue("@Fecha", evento.Fecha.Date); 
                    cmd.Parameters.AddWithValue("@Horario", evento.Horario);
                    cmd.Parameters.AddWithValue("@Modulo", evento.Modulo);
                    cmd.Parameters.AddWithValue("@Evento", evento.NomEvento);
                    cmd.Parameters.AddWithValue("@Criticidad", evento.Criticidad);

                    cmd.ExecuteNonQuery();
                }

            }
        }

        public List<Evento> Listar()
        {
            List<Evento> lista = new List<Evento>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "select Usuario, Fecha, Horario, Modulo, Evento, Criticidad from Eventos";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Evento()
                            {
                                Usuario = Convert.ToInt64(dr["Usuario"]),
                                Fecha = Convert.ToDateTime(dr["Fecha"]),
                                Horario = (TimeSpan)dr["Horario"],
                                Modulo = dr["Modulo"].ToString(),
                                NomEvento = dr["Evento"].ToString(),
                                Criticidad = Convert.ToInt16(dr["Criticidad"]),

                            });

                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<Evento>();
                }
            }

            return lista;
        }

        public Usuario ObtenerUsuarioPorDNI(long dni)
        {
            Usuario usuario = null;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT DNI, Nombre, Apellido, Mail, Rol, Contraseña, Bloqueado, Activo FROM Usuarios WHERE DNI = @DNI";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new Usuario()
                            {
                                DNI = Convert.ToInt64(dr["DNI"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Mail = dr["Mail"].ToString(),
                                Rol = dr["Rol"].ToString(),
                                Contraseña = dr["Contraseña"].ToString(),
                                Bloqueado = Convert.ToBoolean(dr["Bloqueado"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    usuario = null; 
                }
            }

            return usuario;
        }
    }

    



}
