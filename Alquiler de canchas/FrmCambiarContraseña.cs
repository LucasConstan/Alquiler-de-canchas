using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;
using BLL;
using Servicios;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Alquiler_de_canchas
{
    public partial class FrmCambiarContraseña : Form, IObserver
    {
        public FrmCambiarContraseña()
        {
            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();
        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        FrmMenu frm = new FrmMenu();

        Usuario usuarioLogueado = SessionManager.GetInstance().Usuario;

        private void button2_Click(object sender, EventArgs e)
        {
            
            SessionManager.GetInstance().Logout();
            frm.Show();
            this.Hide();
        }

        private BLL_Usuario ousuario = new BLL_Usuario();

        private Encriptacion oencriptacion = new Encriptacion();

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtContra.Text == txtContra2.Text)
            {
                long dni = Convert.ToInt64(txtDNI.Text);
                string nuevaContraseña = oencriptacion.GetSHA256(txtContra2.Text);
                BLL_Usuario bllUsuario = new BLL_Usuario();
                bllUsuario.CambiarContraseña(dni, nuevaContraseña);

                MessageBox.Show("La contraseña se ha modificado correctamente");
            }

            else
            {
                MessageBox.Show("Verifique los datos ingresados");
            }
            
        }

        private void FrmCambiarContraseña_Load(object sender, EventArgs e)
        {
            txtDNI.Text = usuarioLogueado.DNI.ToString();
        }

        private void FrmCambiarContraseña_VisibleChanged(object sender, EventArgs e)
        {
           // FrmMenu frmm = this.MdiParent as FrmMenu;
            
        }
    }
}
