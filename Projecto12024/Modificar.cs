using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projecto12024
{
    public partial class Modificar : Form
    {
        CTienda1 tienda1 = new CTienda1();
        public Modificar(string codigo, string nombre, string descripcion, string precio, string stock, string categoria)
        {
            InitializeComponent();

            // Cargar los datos en los cuadros de texto
            TxtCodigo.Text = codigo;
            txtNombre.Text = nombre;
            txtDescripcion.Text = descripcion;
            txtPrecio.Text = precio;
            txtStock.Text = stock;

            // Seleccionar la categoría en el ComboBox
            CargarCombo(categoria);

        }

        private void Modificar_Load(object sender, EventArgs e)
        {
            
        }
        private void CargarCombo(string categoria)
        {
            //Datos categorias
            CCategorias categorias = new CCategorias();
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "id";
            cmbCategoria.DataSource = categorias.GetTabla();

            //Buscamos la opcion correcta que viene del form1
            foreach (DataRowView item in cmbCategoria.Items)
            {
                if (item["Nombre"].ToString() == categoria)
                {
                    cmbCategoria.SelectedItem = item;
                    break;
                }
            }

        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria();
            categoria.ShowDialog();

        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos y que el precio y el stock sean válidos
            if (string.IsNullOrEmpty(txtNombre.Text) ||
                !decimal.TryParse(txtPrecio.Text, out decimal precioValue) ||
                precioValue < 0 ||
                !int.TryParse(txtStock.Text, out int stockValue) ||
                stockValue < 0)
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.\nAsegúrese de que el precio no sea menor a 0 y el stock sea un número entero positivo.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Obtener el código del producto (guardado internamente y no editable)
                int codigo = int.Parse(TxtCodigo.Text);

                // Obtener los valores del formulario
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                int categoria = (int)cmbCategoria.SelectedValue; // Valor seleccionado en el ComboBox

                // Llamar al método para actualizar los datos
                tienda1.ActualizarProducto(codigo, nombre, descripcion, precioValue.ToString(), stockValue.ToString(), categoria);

                MessageBox.Show("Producto actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {

        }
    }
}
