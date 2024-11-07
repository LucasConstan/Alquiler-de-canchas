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
    public partial class FrmCambios : Form
    {
        public FrmCambios()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }
    }
}
