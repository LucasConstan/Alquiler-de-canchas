using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class BackUpRestore
    {
        public bool VerificarRutaBackup(string ruta)
        {
            return Directory.Exists(Path.GetDirectoryName(ruta));
        }

        public bool VerificarArchivoRestore(string rutaArchivo)
        {
            return File.Exists(rutaArchivo) && Path.GetExtension(rutaArchivo).Equals(".bak", StringComparison.OrdinalIgnoreCase);
        }
    }
}
