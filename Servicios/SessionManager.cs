using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Servicios
{
    public class SessionManager
    {

        private static SessionManager instancia = null;
        private Usuario usuario;

        private SessionManager() { }

        public static SessionManager GetInstance()
        {
            if (instancia == null)
            {
                instancia = new SessionManager();
            }
            return instancia;
        }

        public void Login(Usuario nuevoUsuario)
        {
            usuario = nuevoUsuario;
        }

        public void Logout()
        {
            usuario = null;
        }

        
        public Usuario Usuario
        {
            get { return usuario; }
        }

        public string idiomaActual;

        public string IdiomaActual
        {
            get { return idiomaActual; }
            set
            {
                idiomaActual = value;
                LanguageManager.ObtenerInstancia().CargarIdioma();
                LanguageManager.ObtenerInstancia().Notificar();
            }
        }

    }
}
