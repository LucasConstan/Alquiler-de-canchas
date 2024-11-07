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
    public partial class FrmCobrar : Form, IObserver
    {
        private long  precioForm1;

        private long canchaForm1;

        private TimeSpan horaForm1;

        private DateTime fechaForm1;

        public FrmCobrar(long cancha, DateTime fecha, TimeSpan hora, long precio)
        {

            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();

            canchaForm1 = cancha;
            fechaForm1 = fecha;
            horaForm1 = hora;
            precioForm1 = precio;
            label1.Text = "Cancha Nº " + cancha;
            label2.Text = "Fecha: " + fecha.ToShortDateString();
            label3.Text = "Hora: " + hora;
            label4.Text = "Precio: $ " + precio;

            
        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmSeleccionarCancha frmr = new FrmSeleccionarCancha();
            frmr.Show();
            this.Hide();
        }

        private BLL_Cliente ocliente = new BLL_Cliente();

        private void CargarDatos()
        {
            List<Clientes> clientes = ocliente.Listar();
            dataGridView1.DataSource = clientes;
        }

        BLL_Reserva oreserva = new BLL_Reserva();

        private BLL_Evento oEvento = new BLL_Evento();

        private BLL_Factura oFactura = new BLL_Factura();

        private Encriptacion oEncriptar = new Encriptacion();

        private void btnPagar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Reservas nuevaReserva = new Reservas
                {
                    Fecha = fechaForm1,
                    Hora = horaForm1,
                    Num_cancha = canchaForm1,
                    DNI = clienteSeleccionado.DNI
                };

                Factura nuevaFactura = new Factura
                {
                    DNI = clienteSeleccionado.DNI,
                    Fecha_Emision = DateTime.Now,
                    Monto = precioForm1,
                    MetodoPago = comboBox1.Text,
                    Num_tarjeta = oEncriptar.GetAES256(textBox1.Text)

                };
                oFactura.GuardarFactura(nuevaFactura);

                oreserva.GuardarReserva(nuevaReserva);

                MessageBox.Show(LanguageManager.ObtenerInstancia().ObtenerTexto("FrmCobrar.Etiquetas.ReservaRealizada"));

                MessageBox.Show("La reserva se a realizado correctamente ");

                Evento evento = new Evento
                {
                    Usuario = SessionManager.GetInstance().Usuario.DNI,
                    Fecha = DateTime.Now,
                    Horario = DateTime.Now.TimeOfDay,
                    Modulo = "Cobrar",
                    NomEvento = "Nueva reserva",
                    Criticidad = 3
                };
                oEvento.InsertarEvento(evento);
            }

            else
            {
                MessageBox.Show("Seleccione el cliente que solicito la reserva");
            }

            
        }

        private void FrmCobrar_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        Clientes clienteSeleccionado;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                clienteSeleccionado = (Clientes)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                label7.Text = clienteSeleccionado.Nombre + " " + clienteSeleccionado.Apellido;
              
            }
        }
    }
}
