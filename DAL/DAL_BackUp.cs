using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Entidades;
using System.Data;
using Servicios;

namespace DAL
{
    public class DAL_BackUp
    {
        private string connectionString = Conexion.cadena; 

        public void RealizarBackup(string backupPath)
        {
            string nombreArchivo = $"MiSistema.BCK_{DateTime.Now:ddMMyy_HHmm}.bak";
            string rutaCompleta = System.IO.Path.Combine(backupPath, nombreArchivo);
            string comandoBackup = $"BACKUP DATABASE AlquilerCanchas TO DISK = '{rutaCompleta}'";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(comandoBackup, conn);
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void RealizarRestore(string backupFilePath)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand setMaster = new SqlCommand("USE master;", conn))
                    {
                        setMaster.ExecuteNonQuery();
                    }

                    using (SqlCommand setSingleUser = new SqlCommand("ALTER DATABASE AlquilerCanchas SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", conn))
                    {
                        setSingleUser.ExecuteNonQuery();
                    }

                    string query = $"RESTORE DATABASE AlquilerCanchas FROM DISK = '{backupFilePath}' WITH REPLACE;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand setMultiUser = new SqlCommand("ALTER DATABASE AlquilerCanchas SET MULTI_USER;", conn))
                    {
                        setMultiUser.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
