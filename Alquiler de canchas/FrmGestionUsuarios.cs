using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Entidades;
using Servicios;

namespace Alquiler_de_canchas
{
    public partial class FrmGestionUsuarios : Form, IObserver
    {
        public FrmGestionUsuarios()
        {
            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();
        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private BLL_Usuario ousuario = new BLL_Usuario();

        private BLL_Evento oEvento = new BLL_Evento();

        private Encriptacion oencriptacion = new Encriptacion();

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            btnAñadir.Enabled = true;
            btnDesbloquear.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnCancelar.Enabled = false;
            label6.Text = "Modo consulta";
        }

        private void FrmGestionUsuarios_Load(object sender, EventArgs e)
        {
            btnCancelar.Enabled = false;
            label6.Text = "Modo consulta";
            CargarDatos();
            List<Perfil> perfiles = new BLL_Perfil().ListarPerfiles();
            comboBox1.DataSource = perfiles;
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "CodPerfil";

        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            label6.Text = "Modo Añadir";
            btnCancelar.Enabled = true;

           
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            label6.Text = "Modo Eliminar";
            btnAñadir.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnModificar.Enabled = false;
            btnCancelar.Enabled = true;

           
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            label6.Text = "Modo Modificar";
            btnAñadir.Enabled = false;
            btnEliminar.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnCancelar.Enabled = true;
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            label6.Text = "Modo Desbloquear";
            btnAñadir.Enabled = false;
            btnEliminar.Enabled = false;
            btnModificar.Enabled = false;
            btnCancelar.Enabled = true;

            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
           
        }

        private void CargarDatos()
        {
            List<Usuario> usuarios = ousuario.Listar();
            dataGridView1.DataSource = usuarios;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                Usuario usuarioSeleccionado = (Usuario)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                txtDNI.Text = usuarioSeleccionado.DNI.ToString();
                txtNombre.Text = usuarioSeleccionado.Nombre;
                txtApellido.Text = usuarioSeleccionado.Apellido;
                txtMail.Text = usuarioSeleccionado.Mail;
                comboBox1.Text = usuarioSeleccionado.Rol;
                label5.Text = usuarioSeleccionado.Contraseña;
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label6.Text == "Modo Añadir")
            {
                if (!ValidarControles())
                {
                    return;
                }
                List<Usuario> usuarios = new BLL_Usuario().Listar();
                foreach (Usuario usuario in usuarios)
                {
                    if (usuario.DNI == long.Parse(txtDNI.Text))
                    {
                        MessageBox.Show("El DNI ya está registrado.");
                        return;
                    }
                }

                Usuario nuevoUsuario = new Usuario
                {
                    DNI = long.Parse(txtDNI.Text),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Mail = txtMail.Text,
                    Rol = comboBox1.Text,
                    Contraseña = "12345",
                    Bloqueado = checkBox1.Checked,
                

                };

                ousuario.AgregarUsuario(nuevoUsuario);

                CargarDatos();

                MessageBox.Show("El usuario se ha añadido correctamente");

                Evento evento = new Evento
                {
                    Usuario = SessionManager.GetInstance().Usuario.DNI,
                    Fecha = DateTime.Now,
                    Horario = DateTime.Now.TimeOfDay,
                    Modulo = "GestionUsuarios",
                    NomEvento = "Añadir Usuario",
                    Criticidad = 4
                };
                oEvento.InsertarEvento(evento);
            }

            else if (label6.Text == "Modo Eliminar")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    long dni = Convert.ToInt64(dataGridView1.SelectedRows[0].Cells["DNI"].Value);
                    ousuario.EliminarUsuario(dni);
                    CargarDatos();
                    MessageBox.Show("El usuario se ha eliminado correctamente");

                    Evento evento = new Evento
                    {
                        Usuario = SessionManager.GetInstance().Usuario.DNI,
                        Fecha = DateTime.Now,
                        Horario = DateTime.Now.TimeOfDay,
                        Modulo = "GestionUsuarios",
                        NomEvento = "Eliminar Usuario",
                        Criticidad = 4
                    };
                    oEvento.InsertarEvento(evento);
                }
                else
                {
                    MessageBox.Show("Seleccione una fila para eliminar.");
                }

            }

            else if (label6.Text == "Modo Modificar")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (!ValidarControles())
                    {
                        return;
                    }
                    Usuario usuarioModificado = new Usuario
                    {

                        DNI = Convert.ToInt64(txtDNI.Text),
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Mail = txtMail.Text,
                        Rol = comboBox1.Text,
                        Contraseña = "12345",
                        Bloqueado = checkBox1.Checked,
                        Activo = true
                        
                       
                    };

                  
                    ousuario.ActualizarUsuario(usuarioModificado);

                    CargarDatos();
                    MessageBox.Show("El usuario se ha modificado correctamente");
                }
                else
                {
                    MessageBox.Show("Seleccione una fila para modificar.");
                }
            }

            else if (label6.Text == "Modo Desbloquear")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    long dni = Convert.ToInt64(dataGridView1.SelectedRows[0].Cells["DNI"].Value);
                    bool nuevoEstadoBloqueo = false;
                    BLL_Usuario bllUsuario = new BLL_Usuario();
                    bllUsuario.ActualizarEstadoBloqueo(dni, nuevoEstadoBloqueo);

                    CargarDatos();
                    MessageBox.Show("El usuario se ha desbloqueado correctamente");

                    Evento evento = new Evento
                    {
                        Usuario = SessionManager.GetInstance().Usuario.DNI,
                        Fecha = DateTime.Now,
                        Horario = DateTime.Now.TimeOfDay,
                        Modulo = "GestionUsuarios",
                        NomEvento = "Desbloquear Usuario",
                        Criticidad = 4
                    };
                    oEvento.InsertarEvento(evento);
                }
                else
                {
                    MessageBox.Show("Seleccione una fila para desbloquear.");
                }
               
            }

            else if (label6.Text == "Modo Act / Desact")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    long dni = Convert.ToInt64(dataGridView1.SelectedRows[0].Cells["DNI"].Value);
                    bool estadoActual = Convert.ToBoolean(dataGridView1.SelectedRows[0].Cells["Activo"].Value);
                    bool nuevoEstado = !estadoActual; 

                    BLL_Usuario bllUsuario = new BLL_Usuario();
                    bllUsuario.ActualizarEstadoActivo(dni, nuevoEstado);

                    CargarDatos();

                    string mensaje = nuevoEstado ? "El usuario se ha activado correctamente" : "El usuario se ha desactivado correctamente";
                    MessageBox.Show(mensaje);
                }
                else
                {
                    MessageBox.Show("Seleccione una fila para desbloquear.");
                }

            }

            else if (label6.Text == "Modo consulta")
            {
                MessageBox.Show("Elija la accion que desea realizar");
                    
            }


        }

        public bool ValidarControles()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtDNI.Text) || string.IsNullOrWhiteSpace(txtMail.Text) || string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Complete los datos para realizar esta accion");
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label6.Text = "Modo Act / Desact";
            btnAñadir.Enabled = false;
            btnEliminar.Enabled = false;
            btnModificar.Enabled = false;
            btnDesbloquear.Enabled = false;
            btnCancelar.Enabled = true;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Activo")
            {
                bool activo = (bool)dataGridView1.Rows[e.RowIndex].Cells["Activo"].Value;
                if (!activo)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            
        }
    }
}
