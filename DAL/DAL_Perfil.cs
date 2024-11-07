using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Entidades;
using System.Data;
using Servicios;

namespace DAL
{
    public class DALPerfil
    {
        public List<Familia> ListarFamilias()
        {
            List<Familia> lista = new List<Familia>();
            HashSet<string> codigosUnicos = new HashSet<string>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT CodFamilia, Nombre FROM Familia WHERE Tipo = 0";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string codFamilia = dr["CodFamilia"].ToString();

                            if (!codigosUnicos.Contains(codFamilia))
                            {
                                lista.Add(new Familia()
                                {
                                    CodFamilia = Convert.ToInt32(codFamilia),
                                    Nombre = dr["Nombre"].ToString(),
                                });
                                codigosUnicos.Add(codFamilia);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Familia>();
                }
            }

            return lista;
        }

        public List<Perfil> ListarPerfiles()
        {
            List<Perfil> lista = new List<Perfil>();
            HashSet<string> codigosUnicos = new HashSet<string>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT CodFamilia, Nombre FROM Familia WHERE Tipo = 1";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string codFamilia = dr["CodFamilia"].ToString();

          
                            if (!codigosUnicos.Contains(codFamilia))
                            {
                                lista.Add(new Perfil()
                                {
                                    CodPerfil = codFamilia,
                                    Nombre = dr["Nombre"].ToString(),
                                });
                                codigosUnicos.Add(codFamilia); 
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Perfil>();
                }
            }

            return lista;
        }

        

        public List<Permiso> ListarPermisos()
        {
            List<Permiso> lista = new List<Permiso>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT CodPermiso, Nombre FROM Permiso";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Permiso()
                            {
                                CodPermiso = Convert.ToInt32(dr["CodPermiso"]),
                                Nombre = dr["Nombre"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Permiso>();
                }
            }

            return lista;
        }

        public List<string> ObtenerPermisosPorNombreFamilia(string Nombre)
        {
            List<string> permisos = new List<string>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = $"SELECT Permiso.Nombre FROM Familia INNER JOIN Permiso ON Familia.CodPermiso = Permiso.CodPermiso WHERE Familia.Nombre = '{Nombre}'";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            permisos.Add(dr["Nombre"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    permisos = new List<string>();
                }
            }
            return permisos;
        }

        public void CrearComponente(int CodFamilia, string Nombre, bool Tipo)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    
                    string query = "INSERT INTO Familia (CodFamilia, Nombre, Tipo, CodPermiso, CodComp) " +
                                   "VALUES (@CodFamilia, @Nombre, @Tipo, @CodPermiso, @CodComp)";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@CodFamilia", CodFamilia);
                    cmd.Parameters.AddWithValue("@Nombre", Nombre);
                    cmd.Parameters.AddWithValue("@Tipo", Tipo);
                    cmd.Parameters.AddWithValue("@CodPermiso", (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodComp", (object)DBNull.Value);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al crear el componente: " + ex.Message);
                }
            }
        }

        public int ObtenerSiguienteCodigo()
        {
            int siguienteCodigo = 100;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT ISNULL(MAX(CodFamilia), 99) + 1 FROM Familia";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        siguienteCodigo = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el siguiente código: " + ex.Message);
                }
            }

            return siguienteCodigo;
        }

        public List<string> ObtenerPermisosFamilia(string CodFamilia)
        {
            List<string> permisos = new List<string>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = $"SELECT Permiso.Nombre FROM Familia INNER JOIN Permiso ON Familia.CodPermiso = Permiso.CodPermiso WHERE CodFamilia = '{CodFamilia}'";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            permisos.Add(dr["Nombre"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    permisos = new List<string>();
                }
            }
            return permisos;
        }

        public List<int> ObtenerCodPermisosFamilia(string CodFamilia)
        {
            List<int> permisos = new List<int>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = $"SELECT Permiso.CodPermiso FROM Familia INNER JOIN Permiso ON Familia.CodPermiso = Permiso.CodPermiso WHERE CodFamilia = @CodFamilia";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@CodFamilia", CodFamilia);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            permisos.Add(Convert.ToInt32(dr["CodPermiso"]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    permisos = new List<int>();
                }
            }
            return permisos;
        }


        public List<string> ObtenerFamiliasPerfil(string CodFamilia)
        {
            List<string> familias = new List<string>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = $"SELECT F2.Nombre FROM Familia INNER JOIN Familia F2 ON Familia.CodComp = F2.CodFamilia WHERE Familia.CodFamilia = '{CodFamilia}' GROUP BY F2.Nombre";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@CodFamilia", CodFamilia);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            familias.Add(dr["Nombre"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return familias;
        }

        public List<int> ObtenerCodFamiliasPerfil(string codFamilia)
        {
            List<int> codFamilias = new List<int>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = @"SELECT F2.CodFamilia FROM Familia INNER JOIN Familia F2 ON Familia.CodComp = F2.CodFamilia WHERE Familia.CodFamilia = @CodFamilia GROUP BY F2.CodFamilia";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@CodFamilia", codFamilia);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            codFamilias.Add(Convert.ToInt32(dr["CodFamilia"]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener los códigos de familia: " + ex.Message);
                    codFamilias = new List<int>();
                }
            }

            return codFamilias;
        }



        public void EliminarPerfil(int CodPerfil)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                string query = "DELETE FROM Familia WHERE CodFamilia = @CodPerfil";
                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.Parameters.AddWithValue("@CodPerfil", CodPerfil);
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void GuardarNuevoPerfil(NuevoPerfil perfil)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                connection.Open();


                string eliminarQuery = "DELETE FROM Familia WHERE CodFamilia = @CodFamilia";
                using (SqlCommand cmdEliminar = new SqlCommand(eliminarQuery, connection))
                {
                    cmdEliminar.Parameters.AddWithValue("@CodFamilia", perfil.CodRol);
                    cmdEliminar.ExecuteNonQuery();
                }


                foreach (int codFamilia in perfil.listaCodFamilias)
                {
                    string insertarQuery = "INSERT INTO Familia (CodFamilia, Nombre, CodComp, Tipo) VALUES (@CodFamilia, @Nombre, @CodComp, @Tipo)";
                    using (SqlCommand cmdInsertar = new SqlCommand(insertarQuery, connection))
                    {
                        cmdInsertar.Parameters.AddWithValue("@CodFamilia", perfil.CodRol);
                        cmdInsertar.Parameters.AddWithValue("@Nombre", perfil.NombreRol);
                        cmdInsertar.Parameters.AddWithValue("@CodComp", codFamilia);
                        cmdInsertar.Parameters.AddWithValue("@Tipo", true);
                        cmdInsertar.ExecuteNonQuery();
                    }
                }


                foreach (int codPermiso in perfil.listaCodPermisos)
                {
                    string insertarQuery = "INSERT INTO Familia (CodFamilia, Nombre, CodPermiso, Tipo) VALUES (@CodFamilia, @Nombre, @CodPermiso, @Tipo)";
                    using (SqlCommand cmdInsertar = new SqlCommand(insertarQuery, connection))
                    {
                        cmdInsertar.Parameters.AddWithValue("@CodFamilia", perfil.CodRol);
                        cmdInsertar.Parameters.AddWithValue("@Nombre", perfil.NombreRol);
                        cmdInsertar.Parameters.AddWithValue("@CodPermiso", codPermiso);
                        cmdInsertar.Parameters.AddWithValue("@Tipo", true);
                        cmdInsertar.ExecuteNonQuery();
                    }
                }
            }

        }

        public void GuardarPermisosEnFamilias(Familia familia)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.cadena))
            {
                connection.Open();


                string eliminarQuery = "DELETE FROM Familia WHERE CodFamilia = @CodFamilia";
                using (SqlCommand cmdEliminar = new SqlCommand(eliminarQuery, connection))
                {
                    cmdEliminar.Parameters.AddWithValue("@CodFamilia", familia.CodFamilia);
                    cmdEliminar.ExecuteNonQuery();
                }


                foreach (int codPermiso in familia.listaCodPermisosF)
                {
                    string insertarQuery = "INSERT INTO Familia (CodFamilia, Nombre, CodPermiso, Tipo) VALUES (@CodFamilia, @Nombre, @CodPermiso, @Tipo)";
                    using (SqlCommand cmdInsertar = new SqlCommand(insertarQuery, connection))
                    {
                        cmdInsertar.Parameters.AddWithValue("@CodFamilia", familia.CodFamilia);
                        cmdInsertar.Parameters.AddWithValue("@Nombre", familia.Nombre);
                        cmdInsertar.Parameters.AddWithValue("@CodPermiso", codPermiso);
                        cmdInsertar.Parameters.AddWithValue("@Tipo", false); 
                        cmdInsertar.ExecuteNonQuery();
                    }
                }
            }
        }


        public string ObtenerCodFamiliaPorNombre(string nombreFamilia)
        {
            string codFamilia = null;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT CodFamilia FROM Familia WHERE Nombre = @NombreFamilia and Tipo = 1";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@NombreFamilia", nombreFamilia);
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            codFamilia = dr["CodFamilia"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    codFamilia = null;
                }
            }
            return codFamilia;
        }




    }
}
