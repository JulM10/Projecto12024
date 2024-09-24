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
    public partial class Categoria : Form
    {
        public Categoria()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            String Nombre = txtCategoria.Text;
            if (string.IsNullOrEmpty(Nombre))
            {
                MessageBox.Show("Por favor, ingrese un nombre para la categoría.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                //Agregar categoria
                CCategorias cCategorias = new CCategorias();
                cCategorias.AgregarProducto(Nombre);

                //Si sale bien mensaje de exito
                MessageBox.Show("Categoría agregada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargamos el combo de categorías
                CargarCombo();
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo capturamos y mostramos un mensaje al usuario
                MessageBox.Show("Error al agregar la categoría: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void Categoria_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargamos las categorías cuando se carga el formulario
                CargarCombo();
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo mostramos al usuario
                MessageBox.Show("Error al cargar las categorías: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarCombo()
        {
            try
            {
                // Datos de categorías
                CCategorias categorias = new CCategorias();
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "id";
                cmbCategoria.DataSource = categorias.GetTabla();
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo mostramos al usuario
                MessageBox.Show("Error al cargar el combo de categorías: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
