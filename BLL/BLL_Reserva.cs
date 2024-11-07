using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using DAL;

namespace BLL
{
    public class BLL_Reserva
    {
        private DAL_Reserva objeto_Reserva = new DAL_Reserva();
      

        public void GuardarReserva(Reservas reserva)
        {
            objeto_Reserva.GuardarReserva(reserva);
        }

        public List<Reservas> Listar()
        {
            return objeto_Reserva.Listar();
        }

        public List<Reservas> ObtenerReservasPorCanchaYFecha(long canchaId, DateTime fecha) 
        {
            return objeto_Reserva.ObtenerReservasPorCanchaYFecha(canchaId, fecha);
        }

        public void CancelarReserva(long canchaId, DateTime fecha, TimeSpan hora)
        {
            objeto_Reserva.CancelarReserva(canchaId,fecha,hora);
        }
    }
}
