using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class NuevoPerfil
    {
        public int CodRol { get; set; }
        public string NombreRol { get; set; }

        public List<string> listaFamilias = new List<string>();
        public List<string> listaPermisos = new List<string>();

        public List<int> listaCodFamilias = new List<int>();
        public List<int> listaCodPermisos = new List<int>();
    }
}
