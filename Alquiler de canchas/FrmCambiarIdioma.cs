using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Servicios;

namespace Alquiler_de_canchas
{
    public partial class FrmCambiarIdioma : Form
    {
        public FrmCambiarIdioma()
        {
            InitializeComponent();
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            SessionManager.GetInstance().IdiomaActual = "Español";

            MessageBox.Show("Idioma cambiado a español");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SessionManager.GetInstance().IdiomaActual = "English";

            MessageBox.Show("Language changed to English");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SessionManager.GetInstance().IdiomaActual = "Griego";

            MessageBox.Show("Η γλώσσα άλλαξε στα ελληνικά");
        }
    }
}
