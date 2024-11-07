using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Servicios;
using System.Text.Json;
using System.IO;


namespace Servicios
{
    public class LanguageManager : ISubject
    {
        private List<IObserver> ListaFormularios = new List<IObserver>();
        private Dictionary<string, string> Diccionario;

        

        private static LanguageManager instancia;

        private LanguageManager() { }

        public static LanguageManager ObtenerInstancia()
        {
            if (instancia == null)
            {
                instancia = new LanguageManager();
            }
            return instancia;
        }

        

        public void Agregar(IObserver observer)
        {
            ListaFormularios.Add(observer);
        }
        public void Quitar(IObserver observer)
        {
            ListaFormularios.Remove(observer);
        }

        public void Notificar()
        {
            foreach (IObserver observer in ListaFormularios)
            {
                observer.ActualizarIdioma();
            }
        }

        public void CargarIdioma()
        {
            var NombreArchivo = $"{SessionManager.GetInstance().idiomaActual}.json";
            var jsonString = File.ReadAllText(NombreArchivo);
            Diccionario = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        }

        public string ObtenerTexto(string key)
        {
            return Diccionario.ContainsKey(key) ? Diccionario[key] : key;
        }

        public void CambiarIdiomaControles(Control frm)
        {
            try
            {
                frm.Text = LanguageManager.ObtenerInstancia().ObtenerTexto(frm.Name + ".Text");

                foreach (Control c in frm.Controls)
                {
                    if (c is Button || c is Label)
                    {
                        c.Text = ObtenerInstancia().ObtenerTexto(frm.Name + "." + c.Name);
                    }

                    if(c is GroupBox g)
                    {
                        CambiarIdiomaControles(g);
                    }

                    if (c is MenuStrip m)
                    {
                        foreach (ToolStripMenuItem item in m.Items)
                        {
                            if (item is ToolStripMenuItem toolStripMenuItem)
                            {
                                item.Text = ObtenerInstancia().ObtenerTexto(frm.Name + "." + item.Name);
                                CambiarIdiomaMenuStrip(toolStripMenuItem.DropDownItems, frm);
                            }
                        }
                    }

                    if (c.Controls.Count > 0)
                    {
                        CambiarIdiomaControles(c);
                    }
                }
            }
            catch (Exception) { };
        }

        private void CambiarIdiomaMenuStrip(ToolStripItemCollection items, Control frm)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripMenuItem item1)
                {
                    item.Text = ObtenerInstancia().ObtenerTexto(frm.Name + "." + item.Name);

                    CambiarIdiomaMenuStrip(item1.DropDownItems, frm);
                }
            }
        }
    }
}
