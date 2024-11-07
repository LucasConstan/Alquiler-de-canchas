using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Factura
    {
        public int Id_Factura { get; set; }
        public long DNI { get; set; }
        public DateTime Fecha_Emision { get; set; }
        public long Monto { get; set; }
        public string MetodoPago { get; set; }
        public string Num_tarjeta { get; set; }
    }
}
