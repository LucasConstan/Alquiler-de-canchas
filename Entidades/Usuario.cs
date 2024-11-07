using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contraseña { get; set; }
        public long DNI { get; set; }
        public string Mail { get; set; }
        public string Rol { get; set; }
        public bool Bloqueado { get; set; } 
        public bool Activo { get; set; }
        public string Salt { get; set; }
    }
}
