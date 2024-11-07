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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace Alquiler_de_canchas
{
    public partial class FrmClientes : Form, IObserver
    {
        public FrmClientes()
        {
            InitializeComponent();

            LanguageManager.ObtenerInstancia().Agregar(this);

            LanguageManager.ObtenerInstancia().Notificar();
        }

        public void ActualizarIdioma()
        {
            LanguageManager.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private BLL_Cliente ocliente = new BLL_Cliente();
        private BLL_Evento oEvento = new BLL_Evento();

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            if (!ValidarControles())
            {
                return;
            }
            List<Clientes> clientes = new BLL_Cliente().Listar();
            foreach (Clientes cliente in clientes)
            {
                if (cliente.DNI == long.Parse(txtDNI.Text))
                {
                    MessageBox.Show("El DNI ya está registrado.");
                    return;
                }
            }

            Clientes nuevoCliente = new Clientes
            {
                DNI = long.Parse(txtDNI.Text),
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Mail = txtMail.Text,
                Num_tel = Convert.ToInt64(txtTelefono.Text),
                Localidad = txtLocalidad.Text


            };

            Evento evento = new Evento
            {
                Usuario = SessionManager.GetInstance().Usuario.DNI,
                Fecha = DateTime.Now,
                Horario = DateTime.Now.TimeOfDay,
                Modulo = "Clientes",
                NomEvento = "Añadir cliente",
                Criticidad = 2
            };
            oEvento.InsertarEvento(evento);

            ocliente.AgregarCliente(nuevoCliente);

            CargarDatos();

            MessageBox.Show("El cliente se ha añadido correctamente");
            
        }

        private void CargarDatos()
        {
            List<Clientes> clientes = ocliente.Listar();
            dataGridView1.DataSource = clientes;
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                long dni = Convert.ToInt64(dataGridView1.SelectedRows[0].Cells["DNI"].Value);
                ocliente.EliminarCliente(dni);
                CargarDatos();
                MessageBox.Show("El usuario se ha eliminado correctamente");
            }
            else
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (!ValidarControles())
                {
                    return;
                }

                Clientes clienteModificado = new Clientes
                {

                    DNI = Convert.ToInt64(txtDNI.Text),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Mail = txtMail.Text,
                    Num_tel = Convert.ToInt64(txtTelefono.Text),
                    Localidad = txtLocalidad.Text

                };


                ocliente.ActualizarCliente(clienteModificado);

                CargarDatos();
                MessageBox.Show("El usuario se ha modificado correctamente");
            }
        }

        

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                Clientes clienteSeleccionado = (Clientes)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                txtDNI.Text = clienteSeleccionado.DNI.ToString();
                txtNombre.Text = clienteSeleccionado.Nombre;
                txtApellido.Text = clienteSeleccionado.Apellido;
                txtMail.Text = clienteSeleccionado.Mail;
                txtTelefono.Text = clienteSeleccionado.Num_tel.ToString();
                txtLocalidad.Text = clienteSeleccionado.Localidad;


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }

        public bool ValidarControles()
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text) || string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtMail.Text) || string.IsNullOrWhiteSpace(txtLocalidad.Text))
            {
                MessageBox.Show("Complete los datos para realizar esta accion");
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmBackUpRestore frmb = new FrmBackUpRestore();
            frmb.Show();
            this.Hide();
        }

        private void SerializarXML(List<Clientes> clientes, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Clientes>));
                serializer.Serialize(fs, clientes);
            }
        }

        private void SerializarJSON(List<Clientes> clientes, string path)
        {
            string jsonString = JsonConvert.SerializeObject(clientes, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, jsonString);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cmbFormato.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona un formato de serialización.");
                return;
            }

            listBox1.Items.Clear();

            List<Clientes> clientes = ocliente.Listar();

            saveFileDialog1.Filter = $"{cmbFormato.SelectedItem} Files|*.{cmbFormato.SelectedItem.ToString().ToLower()}";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string formato = cmbFormato.SelectedItem.ToString();
                switch (formato)
                {
                    case "XML":
                        SerializarXML(clientes, saveFileDialog1.FileName);
                        MessageBox.Show("Datos serializados en XML con éxito.");
                        MostrarArchivoSerializado(saveFileDialog1.FileName);
                        break;
                    case "JSON":
                        SerializarJSON(clientes, saveFileDialog1.FileName);
                        MessageBox.Show("Datos serializados en JSON con éxito.");
                        MostrarArchivoSerializado(saveFileDialog1.FileName);
                        break;
                }
            }
        }

        private List<Clientes> DeserializarXML(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Clientes>));
                return (List<Clientes>)serializer.Deserialize(fs);
            }
        }

        private List<Clientes> DeserializarJSON(string path)
        {
            string jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Clientes>>(jsonString);
        }



        private void MostrarArchivoSerializado(string path)
        {
            listBox1.Items.Clear();

            string[] lineas = File.ReadAllLines(path);

            foreach (string linea in lineas)
            {
                listBox1.Items.Add(linea);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cmbFormato.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona un formato de serialización.");
                return;
            }

            listBox2.Items.Clear();

            openFileDialog1.Filter = $"{cmbFormato.SelectedItem} Files|*.{cmbFormato.SelectedItem.ToString().ToLower()}";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string formato = cmbFormato.SelectedItem.ToString();
                List<Clientes> clientes = null;

                switch (formato)
                {
                    case "XML":
                        clientes = DeserializarXML(openFileDialog1.FileName);
                        break;
                    case "JSON":
                        clientes = DeserializarJSON(openFileDialog1.FileName);
                        break;
                }

                if (clientes != null)
                {
                    foreach (var cliente in clientes)
                    {
                        listBox2.Items.Add($"DNI: {cliente.DNI} | Nombre: {cliente.Nombre} | Apellido: {cliente.Apellido} | Teléfono: {cliente.Num_tel} | Localidad: {cliente.Localidad} | Email: {cliente.Mail}");
                    }
                    MessageBox.Show("Datos deserializados con éxito.");
                }
            }
        }
    }


}
