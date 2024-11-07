using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Evento
    {
        //public int Id_Evento { get; set; }
        public long Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Horario { get; set; }
        public string Modulo { get; set; }
        public string NomEvento { get; set; }
        public int Criticidad { get; set; }

       
    }

    
}
