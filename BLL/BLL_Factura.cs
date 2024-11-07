using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using DAL;

namespace BLL
{
    public class BLL_Factura
    {
        private DAL_Factura oFactura = new DAL_Factura();

        public void GuardarFactura(Factura factura)
        {
            oFactura.GuardarFactura(factura);
        }

        public List<Factura> ListarFacturas()
        {
            return oFactura.ListarFacturas();
        }
    }
}
