using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using DAL;

namespace BLL
{
    public class BLL_Cliente
    {
        private DAL_Cliente objeto_cliente = new DAL_Cliente();


        public List<Clientes> Listar()
        {
            return objeto_cliente.Listar();
        }

        public void AgregarCliente(Clientes cliente)
        {

            objeto_cliente.InsertarCliente(cliente);
        }

        public void EliminarCliente(long dni) 
        {
            objeto_cliente.EliminarCliente(dni);
        }

        public void ActualizarCliente(Clientes cliente)
        {
            objeto_cliente.ActualizarCliente(cliente);
        }
    }
}
