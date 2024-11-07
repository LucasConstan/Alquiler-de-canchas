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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Diagnostics;
using System.IO;

namespace Alquiler_de_canchas
{
    public partial class FrmEventos : Form
    {
        public FrmEventos()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }

        private BLL_Evento oEvento = new BLL_Evento();

        private void CargarDatos()
        {
            List<Evento> eventos = oEvento.Listar();
            dataGridView1.DataSource = eventos;
        }

        private void FrmEventos_Load(object sender, EventArgs e)
        {
            CargarDatos();
            LlenarCombobox();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Horario")
            {
                if (e.Value != null && e.Value is TimeSpan timeSpan)
                {
                    e.Value = timeSpan.ToString(@"hh\:mm\:ss");
                    e.FormattingApplied = true;
                }
            }
        }

        private void LlenarCombobox()
        {
            List<Evento> eventos = oEvento.Listar();
            List<string> modulosUnicos = new List<string>();
            foreach (var evento in eventos)
            {
                if (!modulosUnicos.Contains(evento.Modulo))
                {
                    modulosUnicos.Add(evento.Modulo);
                }
            }
            comboBoxModulo.DataSource = modulosUnicos;
            comboBoxModulo.SelectedIndex = -1; 


            List<long> usuariosUnicos = new List<long>();
            foreach (var evento in eventos)
            {
                if (!usuariosUnicos.Contains(evento.Usuario))
                {
                    usuariosUnicos.Add(evento.Usuario);
                }
            }
            comboBoxUsuario.DataSource = usuariosUnicos;
            comboBoxUsuario.SelectedIndex = -1;

            List<string> eventosUnicos = new List<string>();
            foreach (var evento in eventos)
            {
                if (!eventosUnicos.Contains(evento.NomEvento))
                {
                    eventosUnicos.Add(evento.NomEvento);
                }
            }
            comboBoxEvento.DataSource = eventosUnicos;
            comboBoxEvento.SelectedIndex = -1;

            List<int> criticidadUnica = new List<int>();
            foreach (var evento in eventos)
            {
                if (!criticidadUnica.Contains(evento.Criticidad))
                {
                    criticidadUnica.Add(evento.Criticidad);
                }
            }
            comboBoxCriticidad.DataSource = criticidadUnica;
            comboBoxCriticidad.SelectedIndex = -1;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            List<Evento> eventos = oEvento.Listar();
            List<Evento> eventosFiltrados = new List<Evento>(eventos);

            if (comboBoxModulo.SelectedIndex != -1)
            {
                string moduloSeleccionado = comboBoxModulo.SelectedItem.ToString();
                eventosFiltrados = eventosFiltrados
                    .Where(ev => ev.Modulo == moduloSeleccionado)
                    .ToList();
            }

            if (comboBoxUsuario.SelectedIndex != -1)
            {
                long usuarioSeleccionado = (long)comboBoxUsuario.SelectedItem;
                eventosFiltrados = eventosFiltrados
                    .Where(ev => ev.Usuario == usuarioSeleccionado)
                    .ToList();
            }

            if (comboBoxEvento.SelectedIndex != -1)
            {
                string eventoSeleccionado = comboBoxEvento.SelectedItem.ToString();
                eventosFiltrados = eventosFiltrados
                    .Where(ev => ev.NomEvento == eventoSeleccionado)
                    .ToList();
            }

            if (comboBoxCriticidad.SelectedIndex != -1)
            {
                int criticidadSeleccionada = (int)comboBoxCriticidad.SelectedItem;
                eventosFiltrados = eventosFiltrados
                    .Where(ev => ev.Criticidad == criticidadSeleccionada)
                    .ToList();
            }

            DateTime fechaInicio = dateTimePicker1.Value.Date;
            eventosFiltrados = eventosFiltrados
                .Where(ev => ev.Fecha >= fechaInicio)
                .ToList();

           
            DateTime fechaFin = dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1); 
            eventosFiltrados = eventosFiltrados
                .Where(ev => ev.Fecha <= fechaFin)
                .ToList();

            dataGridView1.DataSource = eventosFiltrados;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBoxUsuario.SelectedIndex = -1;
            comboBoxModulo.SelectedIndex = -1;
            comboBoxEvento.SelectedIndex = -1;
            comboBoxCriticidad.SelectedIndex = -1;
            CargarDatos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files|*.pdf";
            saveFileDialog.Title = "Guardar como PDF";
            saveFileDialog.FileName = "ReporteEventos.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfWriter writer = new PdfWriter(saveFileDialog.FileName);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);


                    Paragraph title = new Paragraph("Reporte de Eventos")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18);
                    document.Add(title);

                    document.Add(new Paragraph(" ")); 


                    Table table = new Table(dataGridView1.ColumnCount);
                    table.SetWidth(UnitValue.CreatePercentValue(100)); 


                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        table.AddHeaderCell(new Cell().Add(new Paragraph(column.HeaderText)));
                    }


                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(cell.Value.ToString())));
                            }
                        }
                    }


                    document.Add(table);


                    document.Close();

                    MessageBox.Show("PDF generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                long dni = Convert.ToInt64(dataGridView1.Rows[e.RowIndex].Cells["Usuario"].Value);

                Usuario usuarioSeleccionado = new BLL_Evento().ObtenerUsuarioPorDNI(dni);
                if (usuarioSeleccionado != null)
                {
                    txtNombre.Text = usuarioSeleccionado.Nombre;
                    txtApellido.Text = usuarioSeleccionado.Apellido;
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado.");
                }
            }
    
      
        
    

        }
    }
}
