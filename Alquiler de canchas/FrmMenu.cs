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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Alquiler_de_canchas
{
    public partial class FrmMenu : Form, IObserver
    {
        public FrmMenu()
        {
            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();
        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private BLL_Perfil operfil = new BLL_Perfil();

        private void gestionDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmGestionUsuarios frgu = new FrmGestionUsuarios();
            frgu.Show();
            this.Close();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLogin frml = new FrmLogin();
            frml.Show();
            this.Hide();
        }



        private void FrmMenu_Load(object sender, EventArgs e)
        {

            Usuario usuarioLogueado = SessionManager.GetInstance().Usuario;

            Dictionary<string, ToolStripMenuItem> controlPermisos = new Dictionary<string, ToolStripMenuItem>()
    {
        { "Admin", aDMINToolStripMenuItem },
        { "Maestros", uSUARIOToolStripMenuItem },
        { "GestionReservas", gestionDeReservasToolStripMenuItem },
        { "Reportes", reportesToolStripMenuItem },
        { "CambiarClave", cambiarClaveToolStripMenuItem },
        { "Logout", logoutToolStripMenuItem }
    };

            foreach (var control in controlPermisos.Values)
            {
                control.Visible = false;
            }


            

            if (usuarioLogueado != null)
            {
                string CodFamilia = operfil.ObtenerCodporNombre(usuarioLogueado.Rol);
                List<string> listaPermisos = operfil.ObtenerPermisosFamilia(CodFamilia);
                List<string> listaFamilias = operfil.ObtenerFamiliasPerfil(CodFamilia);
                foreach (string familia in listaFamilias)
                {
                    List<string> permisosFamilia = operfil.ObtenerPermisosPorNombreFamilia(familia);
                    foreach (string permisoFamilia in permisosFamilia)
                    {
                        listaPermisos.Add(permisoFamilia);
                    }
                }


                foreach (var permiso in listaPermisos)
                {
                    if (controlPermisos.ContainsKey(permiso))
                    {
                        controlPermisos[permiso].Visible = true;
                    }
                }

                //if (usuarioLogueado.Rol == "Administrador")
                //{
                //    aDMINToolStripMenuItem.Visible = true;
                //    uSUARIOToolStripMenuItem.Visible = true;
                //}
                //else
                //{
                //    aDMINToolStripMenuItem.Visible = false;
                //    uSUARIOToolStripMenuItem.Visible = false;
                //}

                //gestionDeReservasToolStripMenuItem.Visible = true;
                //reportesToolStripMenuItem.Visible = true;
                cambiarClaveToolStripMenuItem.Visible = true;
                logoutToolStripMenuItem.Visible = true;
            }

        }

        private BLL_Evento oEvento = new BLL_Evento();

       

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmMenu.Etiquetas.SesionCerrada2"), LanguageManager.ObtenerInstancia().ObtenerTexto("FrmMenu.Etiquetas.SesionCerrada3"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                Evento evento = new Evento
                {
                    Usuario = SessionManager.GetInstance().Usuario.DNI,
                    Fecha = DateTime.Now,
                    Horario = DateTime.Now.TimeOfDay,
                    Modulo = "Logout",
                    NomEvento = "Cerro sesion",
                    Criticidad = 1
                };
                oEvento.InsertarEvento(evento);

                SessionManager.GetInstance().Logout();
                FrmMenu frmm = new FrmMenu();
                frmm.Show();
                this.Close();

                aDMINToolStripMenuItem.Visible = false;
                uSUARIOToolStripMenuItem.Visible = false;
                gestionDeReservasToolStripMenuItem.Visible = false;
                reportesToolStripMenuItem.Visible = false;
                cambiarClaveToolStripMenuItem.Visible = false;
                logoutToolStripMenuItem.Visible = false;

                MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmMenu.Etiquetas.SesionCerrada"));

               // MessageBox.Show("La sesion se ha cerrado correctamente");
            }


        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCambiarContraseña frmcc = new FrmCambiarContraseña();
            frmcc.Show();
            this.Hide();
        }



        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClientes frmc = new FrmClientes();
            frmc.Show();
            this.Hide();
        }

        private void canchasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCanchas frmcan = new FrmCanchas();
            frmcan.Show();
            this.Hide();
        }

        private void gestionDeReservasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSeleccionarCancha frmr = new FrmSeleccionarCancha();
            frmr.Show();
            this.Hide();
        }

        private void gestionDePerfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmGestionPerfiles frgp = new FrmGestionPerfiles();
            frgp.Show();
            this.Hide();
        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCambiarIdioma frci = new FrmCambiarIdioma();
            frci.Show();
            this.Hide();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFacturas frmf = new FrmFacturas();
            frmf.Show();
            this.Hide();
        }

        private void eventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEventos frme = new FrmEventos();
            frme.Show();
            this.Hide();
        }

        private void canchasCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCambios frmc = new FrmCambios();
            frmc.Show();
            this.Hide();
        }
    }
}


