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
        private DataTable TablaTienda;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarTabla();
            CargarCombo();

        }
        private void CargarTabla()
        {
            // Limpiar las filas actuales
            DgvTienda.Rows.Clear();

            // Cargar los datos
            CTienda1 tienda1 = new CTienda1();
            CCategorias categorias = new CCategorias();
            TablaTienda = tienda1.GetTienda();

            if (TablaTienda != null)
            {
                foreach (DataRow dr in TablaTienda.Rows)
                {
                    String Categoria = categorias.GetCategoria((int)dr["IdCategorías"]);
                    DgvTienda.Rows.Add(dr["Código"], dr["Nombre"], dr["Descripción"], "$ "+dr["Precio"], dr["Stock"],Categoria);
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
        private void CargarCombo()
        {
            try
            {
                // Datos de categorías
                CCategorias categorias = new CCategorias();
                cmbCategorias.DisplayMember = "Nombre";
                cmbCategorias.ValueMember = "id";
                cmbCategorias.DataSource = categorias.GetTabla();
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo mostramos al usuario
                MessageBox.Show("Error al cargar el combo de categorías: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BtnFiltrarCategorias_Click(object sender, EventArgs e)
        {
            // Asegúrate de que se haya seleccionado una categoría
            if (cmbCategorias.SelectedItem != null)
            {
                // Limpiar las columnas antes de filtrar
                DgvTienda.Columns.Clear();

                // Obtener el id de la categoría seleccionada
                int categoriaId = (int)((DataRowView)cmbCategorias.SelectedItem)["id"];

                // Crear DataView para aplicar el filtro
                DataView dv = new DataView(TablaTienda);
                dv.RowFilter = $"IdCategorías = {categoriaId}"; // Filtrar por Id de categoría

                // Volver a agregar las columnas manualmente
                DgvTienda.Columns.Add("Código", "Código");
                DgvTienda.Columns.Add("Nombre", "Nombre");
                DgvTienda.Columns.Add("Descripción", "Descripción");
                DgvTienda.Columns.Add("Precio", "Precio");
                DgvTienda.Columns.Add("Stock", "Stock");
                DgvTienda.Columns.Add("Categoría", "Categoría");

                // Cargar los datos filtrados en el DataGridView
                foreach (DataRowView drv in dv)
                {
                    CCategorias categorias = new CCategorias();
                    String Categoria = categorias.GetCategoria((int)drv["IdCategorías"]);
                    DgvTienda.Rows.Add(drv["Código"], drv["Nombre"], drv["Descripción"], "$ " + drv["Precio"], drv["Stock"], Categoria);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una categoría válida para filtrar.", "Error de filtro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnFiltrarCodigo_Click(object sender, EventArgs e)
        {
            String codigo = txtCodigo.Text;
            if (!string.IsNullOrEmpty(codigo))
            {
                if (int.TryParse(codigo, out int codigoNumerico))
                {
                    // Limpiar las columnas antes de filtrar
                    DgvTienda.Columns.Clear();

                    DataView dv = new DataView(TablaTienda);
                    dv.RowFilter = $"Código = {codigoNumerico}"; // Eliminar comillas simples para valores numéricos
                    
                    // Volver a agregar las columnas manualmente
                    DgvTienda.Columns.Add("Código", "Código");
                    DgvTienda.Columns.Add("Nombre", "Nombre");
                    DgvTienda.Columns.Add("Descripción", "Descripción");
                    DgvTienda.Columns.Add("Precio", "Precio");
                    DgvTienda.Columns.Add("Stock", "Stock");
                    DgvTienda.Columns.Add("Categoría", "Categoría");

                    // Cargar los datos filtrados en el DataGridView
                    foreach (DataRowView drv in dv)
                    {
                        CCategorias categorias = new CCategorias();
                        String Categoria = categorias.GetCategoria((int)drv["IdCategorías"]);
                        DgvTienda.Rows.Add(drv["Código"], drv["Nombre"], drv["Descripción"], "$ " + drv["Precio"], drv["Stock"], Categoria);
                    }
                    txtCodigo.Text = string.Empty;
                }
            }
                else
                {
                    MessageBox.Show("Por favor, ingresa un código numérico válido.", "Error de filtro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //Limpio las caja de texto 
            txtCodigo.Text = string.Empty;  
            txtNombre.Text = string.Empty;
            //Limpio la data grid view para agregar los datos de nuevo
            DgvTienda.Columns.Clear ();

            DgvTienda.Columns.Add("Código", "Código");
            DgvTienda.Columns.Add("Nombre", "Nombre");
            DgvTienda.Columns.Add("Descripción", "Descripción");
            DgvTienda.Columns.Add("Precio", "Precio");
            DgvTienda.Columns.Add("Stock", "Stock");
            DgvTienda.Columns.Add("Categoría", "Categoría");
            CargarTabla();
        }

        private void BtnFiltrarNombre_Click(object sender, EventArgs e)
        {
            String nombre = txtNombre.Text;
            if (!string.IsNullOrEmpty(nombre))
            {
                // Limpiar las columnas antes de filtrar
                DgvTienda.Columns.Clear();

                // Crear DataView para aplicar el filtro
                DataView dv = new DataView(TablaTienda);
                dv.RowFilter = $"Nombre LIKE '%{nombre}%'"; // Usar LIKE para coincidencias parciales

                // Volver a agregar las columnas manualmente
                DgvTienda.Columns.Add("Código", "Código");
                DgvTienda.Columns.Add("Nombre", "Nombre");
                DgvTienda.Columns.Add("Descripción", "Descripción");
                DgvTienda.Columns.Add("Precio", "Precio");
                DgvTienda.Columns.Add("Stock", "Stock");
                DgvTienda.Columns.Add("Categoría", "Categoría");

                // Cargar los datos filtrados en el DataGridView
                foreach (DataRowView drv in dv)
                {
                    CCategorias categorias = new CCategorias();
                    String Categoria = categorias.GetCategoria((int)drv["IdCategorías"]);
                    DgvTienda.Rows.Add(drv["Código"], drv["Nombre"], drv["Descripción"], "$ " + drv["Precio"], drv["Stock"], Categoria);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un nombre válido para filtrar.", "Error de filtro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
