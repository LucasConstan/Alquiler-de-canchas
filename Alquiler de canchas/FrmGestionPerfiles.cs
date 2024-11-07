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
    public partial class FrmGestionPerfiles : Form
    {
        public FrmGestionPerfiles()
        {
            InitializeComponent();


        }

        BLL_Usuario ousuario = new BLL_Usuario();

        private void FrmGestionPerfiles_Load(object sender, EventArgs e)
        {
            LlenarCombo();
            ActualizarListas();
        }

        private BLL_Perfil operfil = new BLL_Perfil();

        NuevoPerfil Nuevoperfil = new NuevoPerfil();

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

            List<Perfil> perfiles = new BLL_Perfil().ListarPerfiles();
            CBPerfiles.DataSource = perfiles;
            CBPerfiles.DisplayMember = "Nombre";
            CBPerfiles.ValueMember = "CodPerfil";


        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool FamiliaYaAgregada = false;
            bool PermisoYaAgregado = false;

            try
            {
                foreach (TreeNode n in treeView1.Nodes[0].Nodes)
                {
                    if (n.Text == CBFamilias.Text)
                    {
                        FamiliaYaAgregada = true;
                        break;
                    }
                }

                if (!FamiliaYaAgregada)
                {
                    TreeNode nodo = treeView1.Nodes[0].Nodes.Add(CBFamilias.Text);
                    Nuevoperfil.listaFamilias.Add(CBFamilias.Text);
                    Nuevoperfil.listaCodFamilias.Add(Convert.ToInt32(CBFamilias.SelectedValue));

                    List<string> permisosFamilia = operfil.ObtenerPermisosPorNombreFamilia(CBFamilias.Text);

                    foreach (string permiso in permisosFamilia)
                    {
                        foreach (TreeNode n in treeView1.Nodes[0].Nodes)
                        {
                            if (n.Text == permiso)
                            {
                                PermisoYaAgregado = true;
                                break;
                            }
                            else if (n.Nodes.Count != 0)
                            {
                                foreach (TreeNode n1 in n.Nodes)
                                {
                                    if (n1.Text == permiso)
                                    {
                                        PermisoYaAgregado = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!PermisoYaAgregado)
                        {
                            nodo.Nodes.Add(permiso);
                            

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Familia ya seleccionada.");
                }

                if (PermisoYaAgregado)
                {
                  
                    foreach (TreeNode n in treeView1.Nodes[0].Nodes)
                    {
                        if (n.Text == CBFamilias.Text)
                        {
                            treeView1.Nodes.Remove(n);
                            break;
                        }
                    }

                    MessageBox.Show("Permiso de la familia repetido.");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }

            ActualizarListas();
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
                    Nuevoperfil.listaPermisos.Add(CBPermisos.Text);
                    Nuevoperfil.listaCodPermisos.Add(Convert.ToInt32(CBPermisos.SelectedValue));
                }
                else
                {
                    MessageBox.Show("Permiso Ya Seleccionado");
                }
            }
            catch (Exception) { }

            ActualizarListas();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            var perfilesExistentes = operfil.ListarPerfiles();

            if (perfilesExistentes.Any(perfil => perfil.Nombre.Equals(CBPerfiles.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("El nombre del perfil ya existe");
                return;
            }

            int CodPerfil = operfil.ObtenerSiguienteCodigo();

            operfil.CrearComponente(CodPerfil, CBPerfiles.Text, true);
            LlenarCombo();

            MessageBox.Show("Perfil Creado");

        }

        private void CBPerfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count == 0 || treeView1.Nodes[0].Text != CBPerfiles.Text)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(CBPerfiles.Text);

                var perfilSeleccionado = CBPerfiles.SelectedItem as Entidades.Perfil;

                if (perfilSeleccionado != null)
                {
                    string codFamilia = perfilSeleccionado.CodPerfil;


                    Nuevoperfil.listaPermisos = operfil.ObtenerPermisosFamilia(codFamilia);
                    Nuevoperfil.listaCodPermisos = operfil.ObtenerCodPermisosFamilia(codFamilia);

                    foreach (string permiso in Nuevoperfil.listaPermisos)
                    {
                        treeView1.Nodes[0].Nodes.Add(permiso);
                    }


                    Nuevoperfil.listaFamilias = operfil.ObtenerFamiliasPerfil(codFamilia);
                    Nuevoperfil.listaCodFamilias = operfil.ObtenerCodFamiliasPerfil(codFamilia);
                    foreach (string familia in Nuevoperfil.listaFamilias)
                    {
                     
                        TreeNode nodoFamilia = treeView1.Nodes[0].Nodes.Add(familia);


                        List<string> permisosFamilia = operfil.ObtenerPermisosPorNombreFamilia(familia);
                        foreach (string permisoFamilia in permisosFamilia)
                        {
                            nodoFamilia.Nodes.Add(permisoFamilia);
                        }
                    }
           
                }

                Nuevoperfil.NombreRol = perfilSeleccionado.Nombre;
                Nuevoperfil.CodRol = int.Parse(perfilSeleccionado.CodPerfil);
                ActualizarListas();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmMenu frmM = new FrmMenu();
            frmM.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmCrearFamilia frmcf = new FrmCrearFamilia();
            frmcf.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            operfil.GuardarPerfil(Nuevoperfil);
            MessageBox.Show("Perfil guardado correctamente");
        }

        private void ActualizarListas()
        {
            LB_Familias.DataSource = null; 
            LB_Familias.DataSource = Nuevoperfil.listaCodFamilias;

            LB_Permisos.DataSource = null;
            LB_Permisos.DataSource = Nuevoperfil.listaPermisos;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            ActualizarListas();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null && treeView1.SelectedNode.Parent != null)
                {
                    TreeNode selectedNode = treeView1.SelectedNode;

                    if (Nuevoperfil.listaFamilias.Contains(selectedNode.Text))
                    {
                        treeView1.Nodes.Remove(selectedNode);

                        int index = Nuevoperfil.listaFamilias.IndexOf(selectedNode.Text);
                        Nuevoperfil.listaFamilias.RemoveAt(index);
                        Nuevoperfil.listaCodFamilias.RemoveAt(index);

                        MessageBox.Show("Familia eliminada");
                    }
                    
                    else if (Nuevoperfil.listaPermisos.Contains(selectedNode.Text))
                    {
                        treeView1.Nodes.Remove(selectedNode);
                        int index = Nuevoperfil.listaPermisos.IndexOf(selectedNode.Text);
                        Nuevoperfil.listaPermisos.RemoveAt(index);
                        Nuevoperfil.listaCodPermisos.RemoveAt(index);

                        MessageBox.Show("Permiso eliminado");
                    }

                    else
                    {
                        MessageBox.Show("Seleccione un elemento para eliminar.");
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un elemento para eliminar.");
                }

                ActualizarListas(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar : {ex.Message}");
            }
        }
    }
}
