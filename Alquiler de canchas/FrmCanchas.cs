using BLL;
using Entidades;
using Servicios;
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
    public partial class FrmCanchas : Form, IObserver
    {
        public FrmCanchas()
        {
            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();
        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private BLL_Cancha ocancha = new BLL_Cancha();

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            if (!ValidarControles())
            {
                return;
            }

            List<Canchas> canchas = new BLL_Cancha().Listar();
            foreach (Canchas cancha in canchas)
            {
                if (cancha.Num_cancha == long.Parse(txtNum.Text))
                {
                    MessageBox.Show("Esta cancha ya está registrada.");
                    return;
                }
            }

            Canchas nuevaCancha = new Canchas
            {
                Num_cancha = long.Parse(txtNum.Text),
                Tamaño = comboTamaño.Text,
                Cesped = comboCesped.Text,
                Estado = comboEstado.Text,
                Precio = Convert.ToInt64(txtPrecio.Text),
                
            };

            ocancha.AgregarCancha(nuevaCancha);

            CargarDatos();

            MessageBox.Show("La cancha se ha añadido correctamente");
        }

        private void CargarDatos()
        {
            List<Canchas> canchas = ocancha.Listar();
            dataGridView1.DataSource = canchas;
        }

        private void FrmCanchas_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                long num_cancha = Convert.ToInt64(dataGridView1.SelectedRows[0].Cells["Num_cancha"].Value);
                ocancha.EliminarCancha(num_cancha);
                CargarDatos();
                MessageBox.Show("La cancha se ha eliminado correctamente");
            }
            else
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                Canchas canchaSeleccionada = (Canchas)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                txtNum.Text = canchaSeleccionada.Num_cancha.ToString();
                comboTamaño.Text = canchaSeleccionada.Tamaño;
                comboCesped.Text = canchaSeleccionada.Cesped;
                comboEstado.Text = canchaSeleccionada.Estado;
                txtPrecio.Text = canchaSeleccionada.Precio.ToString();
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                Canchas canchaModificada = new Canchas
                {

                    Num_cancha = long.Parse(txtNum.Text),
                    Tamaño = comboTamaño.Text,
                    Cesped = comboCesped.Text,
                    Estado = comboEstado.Text,
                    Precio = Convert.ToInt64(txtPrecio.Text),

                };


                ocancha.ActualizarCancha(canchaModificada);

                CargarDatos();
                MessageBox.Show("La cancha se ha modificado correctamente");
            }
        }

        public bool ValidarControles()
        {
            if (string.IsNullOrWhiteSpace(txtNum.Text) || string.IsNullOrWhiteSpace(comboTamaño.Text) || string.IsNullOrWhiteSpace(comboEstado.Text) || string.IsNullOrWhiteSpace(comboCesped.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Complete los datos para realizar esta accion");
                return false;
            }

            return true;
        }
    }
}
