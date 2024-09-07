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
    public partial class Agregar : Form
    {
        public Agregar()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            CTienda1 cTienda1 = new CTienda1();
            String Nombre = txtNombre.Text;
            String Descripcion = txtDescripcion.Text;
            String Precio = txtPrecio.Text;
            String Stock = txtStock.Text;
            String Categoria = cmbCategoria.SelectedValue.ToString();



        }

        private void Agregar_Load(object sender, EventArgs e)
        {

        }
    }
}
