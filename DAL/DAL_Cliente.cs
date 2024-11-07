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
    public class DAL_Cliente
    {
        private Encriptacion oEncriptar = new Encriptacion();

        public List<Clientes> Listar()
        {
            List<Clientes> lista = new List<Clientes>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "select DNI,Nombre,Apellido,Mail,Num_tel,Localidad from Clientes";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Clientes()
                            {
                                DNI = Convert.ToInt64(dr["DNI"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Mail = oEncriptar.DesencriptarAES256 (dr["Mail"].ToString()),
                                Num_tel = Convert.ToInt64(dr["Num_tel"]),
                                Localidad = dr["Localidad"].ToString(),
                            });

                        }
                    }

                }
                catch (Exception ex)
                {
                    lista = new List<Clientes>();
                }
            }

            return lista;
        }

        public void InsertarCliente(Clientes cliente)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {

                conn.Open();
                string query = "INSERT INTO Clientes (DNI,Nombre,Apellido,Mail,Num_tel,Localidad) VALUES (@DNI,@Nombre,@Apellido,@Mail,@Num_tel,@Localidad)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", cliente.DNI);
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    cmd.Parameters.AddWithValue("@Mail", oEncriptar.GetAES256(cliente.Mail));
                    cmd.Parameters.AddWithValue("@Num_tel", cliente.Num_tel);
                    cmd.Parameters.AddWithValue("@Localidad", cliente.Localidad);
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public void EliminarCliente(long dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "DELETE FROM Clientes WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarCliente(Clientes cliente)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = "UPDATE Clientes SET Nombre = @Nombre, Apellido = @Apellido, Mail = @Mail, Num_tel = @Num_tel, Localidad = @Localidad WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", cliente.DNI);
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    cmd.Parameters.AddWithValue("@Mail", oEncriptar.GetAES256(cliente.Mail));
                    cmd.Parameters.AddWithValue("@Num_tel", cliente.Num_tel);
                    cmd.Parameters.AddWithValue("@Localidad", cliente.Localidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}