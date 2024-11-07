using DAL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Perfil
    {
        private DALPerfil objeto_perfil = new DALPerfil();


        public List<Familia> ListarFamilias()
        {
            return objeto_perfil.ListarFamilias();
        }

        public List<Permiso> ListarPermisos()
        {
            return objeto_perfil.ListarPermisos();
        }

        public List<String> ObtenerPermisosPorNombreFamilia(string nombre)
        {
            return objeto_perfil.ObtenerPermisosPorNombreFamilia(nombre);
        }

        public List<String> ObtenerPermisosFamilia(string codfamilia)
        {
            return objeto_perfil.ObtenerPermisosFamilia(codfamilia);
        }

        public List<int> ObtenerCodPermisosFamilia(string codfamilia)
        {
            return objeto_perfil.ObtenerCodPermisosFamilia(codfamilia);
        }

        public List<String> ObtenerFamiliasPerfil(string codfamilia)
        {
            return objeto_perfil.ObtenerFamiliasPerfil(codfamilia);
        }

        public List<int> ObtenerCodFamiliasPerfil(string codfamilia)
        {
            return objeto_perfil.ObtenerCodFamiliasPerfil(codfamilia);
        }

        public void CrearComponente(int codFamilia, string nombre, bool tipo)
        {
            objeto_perfil.CrearComponente(codFamilia, nombre, tipo);
        }

        public int ObtenerSiguienteCodigo()
        {
            return objeto_perfil.ObtenerSiguienteCodigo();
        }

        public List<Perfil> ListarPerfiles()
        {
            return objeto_perfil.ListarPerfiles();
        }

        public void EliminarPerfil(int codPerfil)
        {
            objeto_perfil.EliminarPerfil(codPerfil);
        }

        public void GuardarPerfil(NuevoPerfil perfil)
        {
            objeto_perfil.GuardarNuevoPerfil(perfil);
        }

        public void GuardarFamilia(Familia familia)
        {
            objeto_perfil.GuardarPermisosEnFamilias(familia);
        }

        public string ObtenerCodporNombre(string nombre)
        {
             return objeto_perfil.ObtenerCodFamiliaPorNombre(nombre);
        }




    }
}
