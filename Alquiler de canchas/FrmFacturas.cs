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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Diagnostics;
using System.IO;



namespace Alquiler_de_canchas
{
    public partial class FrmFacturas : Form
    {
        public FrmFacturas()
        {
            InitializeComponent();
        }

        BLL_Factura oFactura = new BLL_Factura();

        private void FrmFacturas_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        public void CargarDatos()
        {
            List<Factura> facturas = oFactura.ListarFacturas();
            dataGridView1.DataSource = facturas;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Factura> facturas = oFactura.ListarFacturas();
            string filePath = GenerarReportePDF(facturas);
            MessageBox.Show("Reporte PDF generado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

          
            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("No se pudo encontrar el archivo PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarReportePDF(List<Factura> facturas)
        {
            string filePath = "Factura.pdf";

            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                   
                    document.Add(new Paragraph("Reporte de Facturas")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20)
                        .SetBold());

                    foreach (var factura in facturas)
                    {
                       
                        document.Add(new Paragraph($"Número de factura: {factura.Id_Factura}"));
                        document.Add(new Paragraph($"DNI: {factura.DNI}"));
                        document.Add(new Paragraph($"Fecha de Emisión: {factura.Fecha_Emision:dd/MM/yyyy}"));
                        document.Add(new Paragraph($"Monto: {factura.Monto:C}"));
                        document.Add(new Paragraph($"Método de Pago: {factura.MetodoPago}"));
                        document.Add(new Paragraph($"Número de Tarjeta: {factura.Num_tarjeta}"));
                        document.Add(new Paragraph("\n")); 
                    }
                }
            }

            return filePath;
        }
    }
}

