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



namespace Alquiler_de_canchas
{
    public partial class FrmLogin : Form, IObserver
    {
        public FrmLogin()
        {
            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();

        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        long dni;
        private BLL_Evento oEvento = new BLL_Evento();
        FrmMenu frm = new FrmMenu();
        public Encriptacion encriptacion = new Encriptacion();
        private static int contadorIntentos = 0;
        private long ultimoDniIntentado = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (SessionManager.GetInstance().Usuario != null)
            {
                MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmLogin.Etiquetas.SesionIngresada"));
                return;
            }
            
            if (!long.TryParse(textBox1.Text, out dni))
            {
                MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmLogin.Etiquetas.DNIInvalido"));
                return;
            }

            BLL_Usuario bllUsuario = new BLL_Usuario();
            Usuario ousuario = new BLL_Usuario().Listar().FirstOrDefault(u => u.DNI == dni );

            if (ousuario != null)
            {
                if (ousuario.Bloqueado)
                {
                    MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmLogin.Etiquetas.UsuarioBloqueado"));
                    return;
                }

                if (!ousuario.Activo)
                {
                    MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmLogin.Etiquetas.UsuarioDesactivado"));
                    return;
                }

                if (ousuario.Contraseña == encriptacion.GetSHA256(textBox2.Text))
                {
                    SessionManager.GetInstance().Login(ousuario);
                    Evento evento = new Evento
                    {
                        Usuario = ousuario.DNI,
                        Fecha = DateTime.Now,
                        Horario = DateTime.Now.TimeOfDay,
                        Modulo = "Login",
                        NomEvento = "Inicio sesion",
                        Criticidad = 1
                    };
                    oEvento.InsertarEvento(evento);
                    frm.Show();
                    this.Hide();
                    
                }
                else
                {
                    
                    if (ultimoDniIntentado != dni)
                    {
                        contadorIntentos = 0;
                        ultimoDniIntentado = dni;
                    }

                    contadorIntentos++;
                    if (contadorIntentos >= 3)
                    {
                        bllUsuario.ActualizarEstadoBloqueo(dni, true);
                        MessageBox.Show("Ha excedido el número de intentos permitidos. El usuario ha sido bloqueado");
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta. Intento " + contadorIntentos + " de 3.");
                    }
                }
            }
            else
            {
                MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmLogin.Etiquetas.DNInoEncontrado"));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm.Show();
            this.Hide();
        }

       

        
    }
}
