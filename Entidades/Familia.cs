using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Familia
    {
        public int CodFamilia { get; set; }
        public string Nombre { get; set; }

        public List<int> listaCodPermisosF = new List<int>();
    }
}
