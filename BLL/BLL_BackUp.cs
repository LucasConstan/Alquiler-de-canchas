using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_BackUp
    {
        private DAL_BackUp objeto_backup = new DAL_BackUp();

        public void RealizarBackup(string backupPath)
        {
            objeto_backup.RealizarBackup(backupPath);
        }

        public void RealizarRestore(string restorePath)
        {
            objeto_backup.RealizarRestore(restorePath);
        }
    }
}
