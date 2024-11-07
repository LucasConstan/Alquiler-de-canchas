using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alquiler_de_canchas
{
    public partial class FrmBackUpRestore : Form
    {
        public FrmBackUpRestore()
        {
            InitializeComponent();
        }

        private BLL_BackUp obackup = new BLL_BackUp();

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBackupPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnRealizarBackUp_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBackupPath.Text))
            {
                try
                {
                    obackup.RealizarBackup(txtBackupPath.Text);
                    MessageBox.Show("Backup realizado con éxito.");
                    txtBackupPath.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al realizar el backup: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Seleccione una ubicación para el backup.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SQL Backup Files (*.bak)|*.bak";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtRestorePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnRealizarRestore_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRestorePath.Text))
            {
                try
                {
                    obackup.RealizarRestore(txtRestorePath.Text);
                    MessageBox.Show("Restauración realizada con éxito.");
                    txtRestorePath.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al restaurar la base de datos: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un archivo de restore.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmClientes frmc = new FrmClientes();
            frmc.Show();
            this.Hide();
        }
    }
}
