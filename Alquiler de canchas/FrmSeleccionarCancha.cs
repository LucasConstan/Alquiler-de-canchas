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
    public partial class FrmSeleccionarCancha : Form, IObserver
    {
        public FrmSeleccionarCancha()
        {
            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();
        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void FrmReservas_Load(object sender, EventArgs e)
        {
            CargarDatosCan();
            CargarReservas();
            ConfigurarDataGridViewHorarios();
            dataGridViewCanchas.SelectionChanged += dataGridViewCanchas_SelectionChanged;
            dataGridViewHorarios.SelectionChanged += dataGridViewHorarios_SelectionChanged;
        }

        private BLL_Cliente ocliente = new BLL_Cliente();

        private BLL_Cancha ocancha = new BLL_Cancha();

        private BLL_Reserva oreserva = new BLL_Reserva();

        private void CargarDatosClie()
        {
            List<Clientes> clientes = ocliente.Listar();
            dataGridViewCanchas.DataSource = clientes;
        }

        private void CargarDatosCan()
        {
            List<Canchas> canchas = ocancha.Listar();
            dataGridViewCanchas.DataSource = canchas;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }

        private void ConfigurarDataGridViewHorarios()
        {
            dataGridViewHorarios.AutoGenerateColumns = false;
            dataGridViewHorarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Hora",
                DataPropertyName = "Hora",
                ReadOnly = true
            });
            dataGridViewHorarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Disponible",
                DataPropertyName = "Disponible",
                ReadOnly = true
            });
        }

        private void CargarHorarios(DateTime fecha)
        {
            if (dataGridViewCanchas.SelectedRows.Count > 0)
            {
                Canchas canchaSeleccionada = (Canchas)dataGridViewCanchas.SelectedRows[0].DataBoundItem;
                List<Horario> horarios = GenerarHorarios(canchaSeleccionada.Num_cancha, fecha); // CAMBIO
                dataGridViewHorarios.DataSource = horarios;
            }
        }

        
        private void CargarReservas()
        {
            List<Reservas> reservas = oreserva.Listar();
            dataGridView1.DataSource = reservas;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (horarioSeleccionado != null)
            {
                long canchaForm2 = canchaSeleccionada.Num_cancha;
                DateTime fechaForm2 = fechaSeleccionada;
                TimeSpan HoraForm2 = horarioSeleccionado.Hora;
                long precioForm2 = canchaSeleccionada.Precio;
                FrmCobrar frmco = new FrmCobrar(canchaForm2, fechaForm2, HoraForm2, precioForm2);
                frmco.Show();
                this.Hide();
            }

            else
            {
                MessageBox.Show("Seleccione el horario de la reserva");
            }
           
        }

        private void dataGridViewCanchas_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarDatos();
            if (dataGridViewCanchas.SelectedRows.Count > 0)
            {
                DateTime fechaSeleccionada = dateTimePicker1.Value;
                CargarHorarios(fechaSeleccionada);
                
            }

            
        }

        private List<Horario> GenerarHorarios(long canchaId, DateTime fecha)
        {
            List<Horario> horarios = new List<Horario>();
            List<Reservas> reservas = oreserva.ObtenerReservasPorCanchaYFecha(canchaId, fecha);

            for (int hora = 8; hora < 20; hora++)
            {
                TimeSpan horaActual = new TimeSpan(hora, 0, 0);
                bool disponible = !reservas.Any(r => r.Hora == horaActual);
                horarios.Add(new Horario() { Hora = horaActual, Disponible = disponible });
            }
            return horarios;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewCanchas.Rows[e.RowIndex].Selected = true;

            }
        }

        private void dataGridViewHorarios_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarDatos();
        }

        private void dataGridViewHorarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewHorarios.Rows[e.RowIndex].Selected = true;

            }
        }

        Canchas canchaSeleccionada;
        DateTime fechaSeleccionada;
        Horario horarioSeleccionado;

        private void ActualizarDatos()
        {
            if (dataGridViewCanchas.SelectedRows.Count > 0 && dataGridViewHorarios.SelectedRows.Count > 0)
            {
                canchaSeleccionada = (Canchas)dataGridViewCanchas.SelectedRows[0].DataBoundItem;
                fechaSeleccionada = dateTimePicker1.Value;
                horarioSeleccionado = (Horario)dataGridViewHorarios.SelectedRows[0].DataBoundItem;

                label1.Text = $"Cancha: {canchaSeleccionada.Num_cancha}, Fecha: {fechaSeleccionada.ToShortDateString()}, Hora: {horarioSeleccionado.Hora}";
            }
        }

        private void dataGridViewHorarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (dataGridViewHorarios.DataSource != null && dataGridViewHorarios.Rows.Count > 0)
            {
                if (dataGridViewHorarios.Columns[e.ColumnIndex].DataPropertyName == "Disponible")
                {
                    if (e.Value != null && e.Value != DBNull.Value)
                    {
                        bool disponible = (bool)e.Value;
                        if (!disponible)
                        {
                           
                            e.CellStyle.BackColor = Color.Red;
                            e.CellStyle.ForeColor = Color.White;
                        }
                        else
                        {
                        
                            e.CellStyle.BackColor = Color.White;
                            e.CellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewCanchas.SelectedRows.Count > 0 && dataGridViewHorarios.SelectedRows.Count > 0)
            {
                Canchas canchaSeleccionada = (Canchas)dataGridViewCanchas.SelectedRows[0].DataBoundItem;
                DateTime fechaSeleccionada = dateTimePicker1.Value;
                Horario horarioSeleccionado = (Horario)dataGridViewHorarios.SelectedRows[0].DataBoundItem;

                try
                {
                    oreserva.CancelarReserva(canchaSeleccionada.Num_cancha, fechaSeleccionada, horarioSeleccionado.Hora);
                    MessageBox.Show("Reserva cancelada exitosamente.");

                    CargarHorarios(fechaSeleccionada);
                    CargarReservas();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al cancelar la reserva: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una cancha y un horario para cancelar la reserva.");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dataGridViewCanchas.SelectedRows.Count > 0)
            {
                DateTime fechaSeleccionada = dateTimePicker1.Value;
                CargarHorarios(fechaSeleccionada);
            }
        }

        
    }
}
