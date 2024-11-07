using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entidades
{
    //[Serializable]
    public class Clientes
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public long DNI { get; set; }

        public long Num_tel { get; set; }

        public string Mail { get; set; }

        public string Localidad { get; set; }
    }
}
