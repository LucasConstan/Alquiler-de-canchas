using BLL;
using Entidades;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Alquiler_de_canchas
{
    public partial class FrmCrearFamilia : Form
    {
        public FrmCrearFamilia()
        {
            InitializeComponent();
        }

        private BLL_Perfil operfil = new BLL_Perfil();

        private Familia familia = new Familia();

        private void FrmCrearFamilia_Load(object sender, EventArgs e)
        {
            LlenarCombo();
        }

        public void LlenarCombo()
        {

            List<Familia> familias = new BLL_Perfil().ListarFamilias();
            CBFamilias.DataSource = familias;
            CBFamilias.DisplayMember = "Nombre";
            CBFamilias.ValueMember = "CodFamilia";

            List<Permiso> permisos = new BLL_Perfil().ListarPermisos();
            CBPermisos.DataSource = permisos;
            CBPermisos.DisplayMember = "Nombre";
            CBPermisos.ValueMember = "CodPermiso";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmGestionPerfiles frmgp = new FrmGestionPerfiles();
            frmgp.Show();
            this.Hide();
        }

        private void CBFamilias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count == 0 || treeView1.Nodes[0].Text != CBFamilias.Text)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(CBFamilias.Text);

                var familiaSeleccionada = CBFamilias.SelectedItem as Entidades.Familia;

                if (familiaSeleccionada != null)
                {
                    string Nfamilia = familiaSeleccionada.Nombre;
                    string codfamilia = familiaSeleccionada.CodFamilia.ToString();

                    familia.listaCodPermisosF= operfil.ObtenerCodPermisosFamilia(codfamilia);

                    List<string> permisosFamilia = operfil.ObtenerPermisosPorNombreFamilia(Nfamilia);
                    foreach (string permisoFamilia in permisosFamilia)
                    {
                        treeView1.Nodes[0].Nodes.Add(permisoFamilia);
                    }


                }

                familia.CodFamilia = familiaSeleccionada.CodFamilia;
                familia.Nombre = familiaSeleccionada.Nombre;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool PermisoYaAgregado = false;

            try
            {
                foreach (TreeNode n in treeView1.Nodes[0].Nodes)
                {
                    if (n.Text == CBPermisos.Text)
                    {
                        PermisoYaAgregado = true;
                    }
                    else if (n.Nodes.Count != 0)
                    {
                        foreach (TreeNode n1 in n.Nodes)
                        {
                            if (n1.Text == CBPermisos.Text)
                            {
                                PermisoYaAgregado = true;
                            }
                        }
                    }
                }

                if (PermisoYaAgregado == false)
                {
                    treeView1.Nodes[0].Nodes.Add(CBPermisos.Text);
                    familia.listaCodPermisosF.Add(Convert.ToInt32(CBPermisos.SelectedValue));
                    
                }
                else
                {
                    MessageBox.Show("Permiso Ya Seleccionado");
                }
            }
            catch (Exception) { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var familiasExistentes = operfil.ListarFamilias();

            if (familiasExistentes.Any(familia => familia.Nombre.Equals(CBFamilias.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("El nombre de la familia ya existe");
                return;
            }

            int CodFamilia = operfil.ObtenerSiguienteCodigo();

            operfil.CrearComponente(CodFamilia, CBFamilias.Text, false);
            LlenarCombo();

            MessageBox.Show("Familia Creada");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            operfil.GuardarFamilia(familia);
            MessageBox.Show("Familia guardada correctamente");
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Parent != null)
                    {
                        string nombrePermiso = treeView1.SelectedNode.Text;
                        int codigoPermiso = Convert.ToInt32(CBPermisos.SelectedValue);
                        if (familia.listaCodPermisosF.Contains(codigoPermiso))
                        {
                            familia.listaCodPermisosF.Remove(codigoPermiso);
                            treeView1.SelectedNode.Remove();
                        }
                        else
                        {
                            MessageBox.Show("El permiso no está en la lista.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No puedes eliminar la familia. Selecciona un permiso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un permiso para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al eliminar el permiso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
    }
}
