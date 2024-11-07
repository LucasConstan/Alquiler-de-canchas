using DAL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using DAL;

namespace BLL
{
    public class BLL_Cancha
    {

        private DAL_Cancha objeto_cancha = new DAL_Cancha();

        public List<Canchas> Listar()
        {
            return objeto_cancha.Listar();
        }

        public void AgregarCancha(Canchas cancha)
        {

            objeto_cancha.InsertarCancha(cancha);
        }

        public void EliminarCancha(long num_cancha)
        {
            objeto_cancha.EliminarCancha(num_cancha);
        }

        public void ActualizarCancha(Canchas cancha)
        {
            objeto_cancha.ActualizarCancha(cancha);
        }

        public List<long> ObtenerNumCancha()
        {
            return objeto_cancha.ObtenerNumCancha();
        }
    }
}
