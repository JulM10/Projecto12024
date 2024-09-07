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
    public partial class Modificar : Form
    {
        CTienda1 tienda1 = new CTienda1();
        public Modificar()
        {
            InitializeComponent();
        }

        private void Modificar_Load(object sender, EventArgs e)
        {
            CargarCombo();
        }
        private void CargarCombo()
        {
            // Cargar los datos de los productos
            cmbCodigo.DisplayMember = "Nombre";
            cmbCodigo.ValueMember = "Id";
            cmbCodigo.DataSource = tienda1.GetTienda();
            //Datos categorias
            CCategorias categorias = new CCategorias();
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "id";
            cmbCategoria.DataSource = categorias.GetTabla();

        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            
        }

        private void cmbCodigo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CONSULTAR PROFE SI PUEDO CARGAR LOS TXT CON LOS DATOS
        }
    }
}
