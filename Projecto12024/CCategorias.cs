using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto12024
{
    internal class CCategorias
    {
        OleDbConnection CNN;
        OleDbCommand CmdCategoria;
        OleDbDataAdapter DaCategoria;
        DataSet DS;
        String Tabla = "Categorias";

        public CCategorias()
        {
            CNN = new OleDbConnection();
            CNN.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=DB/Tienda.mdb";
            CNN.Open();
            DS = new DataSet();
            //Tabla Categoria
            CmdCategoria = new OleDbCommand();
            CmdCategoria.Connection = CNN;
            CmdCategoria.CommandType = CommandType.TableDirect;
            CmdCategoria.CommandText = Tabla;
            DaCategoria = new OleDbDataAdapter(CmdCategoria);
            DaCategoria.Fill(DS, Tabla);

            // Obtener el valor máximo de 'Id' ya existente en la tabla
            int maxId = 1;
            if (DS.Tables[Tabla].Rows.Count > 0)
            {
                maxId = DS.Tables[Tabla].AsEnumerable()
                                        .Max(row => row.Field<int>("Id"));
            }

            // Configurar 'Id' como autoincremental en el DataSet, comenzando desde el valor máximo + 1
            DS.Tables[Tabla].Columns["Id"].AutoIncrement = true;
            DS.Tables[Tabla].Columns["Id"].AutoIncrementSeed = maxId + 1; // Comenzar desde el valor máximo existente
            DS.Tables[Tabla].Columns["Id"].AutoIncrementStep = 1;


            DataColumn[] pk = new DataColumn[1];
            pk[0] = DS.Tables[Tabla].Columns["Id"];
            DS.Tables[Tabla].PrimaryKey = pk;
            OleDbCommandBuilder cb = new OleDbCommandBuilder(DaCategoria);
            CNN.Close();
        }

        public String GetCategoria(int categoria)
        {
            String nombre = "";
            DataRow drC = DS.Tables[Tabla].Rows.Find(categoria);
            if (drC != null)
            {
                nombre = drC["Nombre"].ToString();
            }
            return nombre;
        }

        public DataTable GetTabla()
        {
            if (DS != null && DS.Tables.Contains(Tabla))
            {
                return DS.Tables[Tabla];
            }
            return null;
        }
        public void AgregarProducto(String Nombre)
        {
            OleDbTransaction transaccion = null;
            CNN.Open();
            transaccion = CNN.BeginTransaction();
            InsertProducto(transaccion, Nombre);
            transaccion.Commit();
            CNN.Close();
        }

        public void InsertProducto(OleDbTransaction transaccion,
                                    String Nombre)
        {
            CmdCategoria.Transaction = transaccion;
            DataRow dr = DS.Tables[Tabla].NewRow();

            dr["Nombre"] = Nombre;
            DS.Tables[Tabla].Rows.Add(dr);
            DaCategoria.Update(DS, Tabla);
            DS.Tables[Tabla].AcceptChanges();
        }
        public void FiltrarPorCategoria()
        {

        }
        public void Dispose()
        {
            DS.Dispose();
        }
    }
}
