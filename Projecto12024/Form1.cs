﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            if (cmbCategorias.SelectedItem != null)
            {
                // Limpiar las columnas antes de filtrar
                DgvTienda.Columns.Clear();

                int categoriaId = (int)((DataRowView)cmbCategorias.SelectedItem)["id"];

                DataView dv = new DataView(TablaTienda);
                dv.RowFilter = $"IdCategorías = {categoriaId}"; 

                // Volver a agregar las columnas manualmente
                DgvTienda.Columns.Add("Código", "Código");
                DgvTienda.Columns.Add("Nombre", "Nombre");
                DgvTienda.Columns.Add("Descripción", "Descripción");
                DgvTienda.Columns.Add("Precio", "Precio");
                DgvTienda.Columns.Add("Stock", "Stock");
                DgvTienda.Columns.Add("Categoría", "Categoría");

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
                    dv.RowFilter = $"Código = {codigoNumerico}";

                    DgvTienda.Columns.Add("Código", "Código");
                    DgvTienda.Columns.Add("Nombre", "Nombre");
                    DgvTienda.Columns.Add("Descripción", "Descripción");
                    DgvTienda.Columns.Add("Precio", "Precio");
                    DgvTienda.Columns.Add("Stock", "Stock");
                    DgvTienda.Columns.Add("Categoría", "Categoría");

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

        private void BtnEliminarFiltro_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = string.Empty;  
            txtNombre.Text = string.Empty;

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

                DataView dv = new DataView(TablaTienda);
                dv.RowFilter = $"Nombre LIKE '%{nombre}%'";

                DgvTienda.Columns.Add("Código", "Código");
                DgvTienda.Columns.Add("Nombre", "Nombre");
                DgvTienda.Columns.Add("Descripción", "Descripción");
                DgvTienda.Columns.Add("Precio", "Precio");
                DgvTienda.Columns.Add("Stock", "Stock");
                DgvTienda.Columns.Add("Categoría", "Categoría");

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

        private void DgvTienda_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 )
            {
                //cargo los datos seleccionados en variables

                string codigo = DgvTienda.Rows[e.RowIndex].Cells["Código"].Value.ToString();
                string Nombre = DgvTienda.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                string Descripcion = DgvTienda.Rows[e.RowIndex].Cells["Descripción"].Value.ToString();
                string Precio= DgvTienda.Rows[e.RowIndex].Cells["Precio"].Value.ToString();
                string Stock= DgvTienda.Rows[e.RowIndex].Cells["Stock"].Value.ToString();
                string Categoria = DgvTienda.Rows[e.RowIndex].Cells["Categoria"].Value.ToString();

                //Abro el formulario de modificacion y le envio los datos
                Modificar ModificarForm = new Modificar(codigo,Nombre,Descripcion,Precio,Stock,Categoria);
                ModificarForm.ShowDialog();
                CargarTabla();
            }
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
                saveFileDialog.Title = "Guardar como";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Crear el archivo de texto
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int i = 0; i < DgvTienda.Columns.Count; i++)
                        {
                            sw.Write(DgvTienda.Columns[i].HeaderText);
                            if (i < DgvTienda.Columns.Count - 1)
                                sw.Write("\t");
                        }
                        sw.WriteLine();

                        // Escribir los datos de cada fila
                        foreach (DataGridViewRow row in DgvTienda.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int i = 0; i < DgvTienda.Columns.Count; i++)
                                {
                                    sw.Write(row.Cells[i].Value?.ToString());
                                    if (i < DgvTienda.Columns.Count - 1)
                                        sw.Write("\t");
                                }
                                sw.WriteLine(); 
                            }
                        }
                    }

                    MessageBox.Show("Datos exportados correctamente", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
