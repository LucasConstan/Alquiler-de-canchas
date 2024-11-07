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
    public class DAL_Factura
    {
        private Encriptacion oEncriptar = new Encriptacion();

        public void GuardarFactura(Factura factura)
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "INSERT INTO Facturas (DNI, Fecha_Emision, Monto, MetodoPago, Num_tarjeta) VALUES (@DNI, @Fecha_Emision, @Monto, @MetodoPago, @Num_tarjeta)";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@DNI", factura.DNI);
                    cmd.Parameters.AddWithValue("@Fecha_Emision", factura.Fecha_Emision);
                    cmd.Parameters.AddWithValue("@Monto", factura.Monto);
                    cmd.Parameters.AddWithValue("@MetodoPago", factura.MetodoPago);
                    cmd.Parameters.AddWithValue("@Num_tarjeta", oEncriptar.GetAES256( factura.Num_tarjeta));
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocurrió un error al guardar la factura: " + ex.Message);
                }
            }
        }

        public List<Factura> ListarFacturas()
        {
            List<Factura> lista = new List<Factura>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT Id_Factura, DNI, Fecha_Emision, Monto, MetodoPago, Num_tarjeta FROM Facturas";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Factura()
                            {
                                Id_Factura = Convert.ToInt32(dr["Id_Factura"]),
                                DNI = Convert.ToInt64(dr["DNI"]),
                                Fecha_Emision = Convert.ToDateTime(dr["Fecha_Emision"]),
                                Monto = Convert.ToInt64(dr["Monto"]),
                                MetodoPago = dr["MetodoPago"].ToString(),
                                Num_tarjeta = dr["Num_tarjeta"].ToString()

                            }) ;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ocurrió un error al listar las facturas: " + ex.Message);
                }
            }
            return lista;
        }
    }
}
