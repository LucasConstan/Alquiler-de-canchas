using DAL;
using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Evento
    {
        private DAL_Evento objeto_evento = new DAL_Evento();

        public void InsertarEvento(Evento evento)
        {
            objeto_evento.InsertarEvento(evento);
        }

        public List<Evento> Listar()
        {
            return objeto_evento.Listar();
        }

        public Usuario ObtenerUsuarioPorDNI(long dni)
        {
            return objeto_evento.ObtenerUsuarioPorDNI(dni);
        }
    }
}
