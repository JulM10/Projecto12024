using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projecto12024
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }
        private void CargarTabla()
        {
            // Limpiar las filas actuales
            dgvTienda.Rows.Clear();

            // Cargar los datos
            CTienda1 tienda1 = new CTienda1();
            CCategorias categorias = new CCategorias();
            DataTable TablaTienda = tienda1.GetTienda();

            if (TablaTienda != null)
            {
                foreach (DataRow dr in TablaTienda.Rows)
                {
                    String Categoria = categorias.GetCategoria((int)dr["IdCategorías"]);
                    dgvTienda.Rows.Add(dr["Código"], dr["Nombre"], dr["Descripción"], dr["Precio"], dr["Stock"],Categoria);
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //cargar formulario para agregar items.
            Agregar Agg = new Agregar();
            Agg.ShowDialog();
            CargarTabla();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Modificar Modificar = new Modificar();
            Modificar.ShowDialog();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
