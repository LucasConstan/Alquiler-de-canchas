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
    public class DAL_Usuario
    {
        public Encriptacion encriptacion = new Encriptacion();

        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "select DNI,Nombre,Apellido,Mail,Rol,Contraseña,Bloqueado,Activo from Usuarios";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                DNI = Convert.ToInt64(dr["DNI"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),                               
                                Mail = dr["Mail"].ToString(),
                                Rol = dr["Rol"].ToString(),
                                Contraseña = dr["Contraseña"].ToString(),
                                Bloqueado = Convert.ToBoolean(dr["Bloqueado"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });

                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<Usuario>();
                }
            }

            return lista;
        }

        public void InsertarUsuario(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {

                conn.Open();
                string query = "INSERT INTO Usuarios (DNI,Nombre,Apellido,Mail, Rol, Contraseña, Bloqueado, Activo) VALUES (@DNI,@Nombre,@Apellido,@Mail,@Rol,@Contraseña,@Bloqueado, @Activo)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", usuario.DNI);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Mail", usuario.Mail);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Contraseña",encriptacion.GetSHA256( usuario.Contraseña));
                    cmd.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);
                    cmd.Parameters.AddWithValue("@Activo", usuario.Activo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarUsuario(long dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "DELETE FROM Usuarios WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Mail = @Mail, Rol = @Rol, Contraseña = @Contraseña, Bloqueado = @Bloqueado WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", usuario.DNI);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Mail", usuario.Mail);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Contraseña", encriptacion.GetSHA256(usuario.Contraseña));
                    cmd.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEstadoBloqueo(long dni, bool bloqueado)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuarios SET Bloqueado = @Bloqueado WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.Parameters.AddWithValue("@Bloqueado", bloqueado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEstadoActivo(long dni, bool activo)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuarios SET Activo = @Activo WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.Parameters.AddWithValue("@Activo", activo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CambiarContraseña(long dni, string contraseña)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuarios SET Contraseña = @Contraseña WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.Parameters.AddWithValue("@Contraseña",contraseña);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
