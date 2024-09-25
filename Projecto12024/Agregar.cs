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
            try
            {
                CTienda1 cTienda1 = new CTienda1();
                String Nombre = txtNombre.Text.Trim();
                String Descripcion = txtDescripcion.Text.Trim();
                String Precio = txtPrecio.Text.Trim();
                String Stock = txtStock.Text.Trim();

                // Validación de campos
                if (string.IsNullOrEmpty(Nombre) ||
                    string.IsNullOrEmpty(Descripcion) ||
                    string.IsNullOrEmpty(Precio) ||
                    string.IsNullOrEmpty(Stock))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar que el precio y el stock sean numéricos y positivos
                if (!decimal.TryParse(Precio, out decimal precioValue) || precioValue < 0)
                {
                    MessageBox.Show("El precio debe ser un valor numérico válido y no puede ser menor a 0.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(Stock, out int stockValue) || stockValue < 0)
                {
                    MessageBox.Show("El stock debe ser un valor numérico válido y no puede ser menor a 0.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Int32 Categoria = (int)cmbCategoria.SelectedValue;

                // Agregar productos a la tienda
                cTienda1.AgregarProducto(Nombre, Descripcion, Precio, Stock, Categoria);
                MessageBox.Show("Producto agregado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al agregar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Agregar_Load(object sender, EventArgs e)
        {
            CargarCombo();        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria();  
            categoria.ShowDialog();
        }
        private void CargarCombo()
        {
            //Datos categorias
            CCategorias categorias = new CCategorias();
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "id";
            cmbCategoria.DataSource = categorias.GetTabla();

        }
    }
}
