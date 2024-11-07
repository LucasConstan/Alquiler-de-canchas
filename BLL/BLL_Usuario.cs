using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using DAL;

namespace BLL
{
    public class BLL_Usuario
    {
        private DAL_Usuario objeto_usuario = new DAL_Usuario();

        public List<Usuario> Listar()
        {
            return objeto_usuario.Listar();
        }

        public void AgregarUsuario(Usuario usuario)
        {
            
            objeto_usuario.InsertarUsuario(usuario);
        }

        public void EliminarUsuario(long dni)
        {
            objeto_usuario.EliminarUsuario(dni);
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            objeto_usuario.ActualizarUsuario(usuario);
        }

        public void ActualizarEstadoBloqueo(long dni, bool bloqueado)
        {
            objeto_usuario.ActualizarEstadoBloqueo(dni, bloqueado);
        }

        public void CambiarContraseña(long dni, string contraseña)
        {
            objeto_usuario.CambiarContraseña(dni, contraseña);
        }

        public void ActualizarEstadoActivo(long dni, bool activo)
        {
            objeto_usuario.ActualizarEstadoActivo(dni, activo);
        }
    }
}
