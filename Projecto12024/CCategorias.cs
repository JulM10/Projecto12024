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
            CNN.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Tienda.mdb";
            CNN.Open();
            DS = new DataSet();
            //Tabla Categoria
            CmdCategoria = new OleDbCommand();
            CmdCategoria.Connection = CNN;
            CmdCategoria.CommandType = CommandType.TableDirect;
            CmdCategoria.CommandText = Tabla;
            OleDbDataAdapter DA = new OleDbDataAdapter(CmdCategoria);
            DA.Fill(DS, Tabla);
            DataColumn[] pk = new DataColumn[1];
            pk[0] = DS.Tables[Tabla].Columns["Id"];
            DS.Tables[Tabla].PrimaryKey = pk;
            OleDbCommandBuilder cb = new OleDbCommandBuilder(DA);
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
            insertProducto(transaccion, Nombre);
            transaccion.Commit();
            CNN.Close();

        }

        public void insertProducto(OleDbTransaction transaccion,
                                    String Nombre)
        {
            CmdCategoria.Transaction = transaccion;
            DataRow dr = DS.Tables[Tabla].NewRow();
            dr["Nombre"] = Nombre;
            DS.Tables[Tabla].Rows.Add(dr);
            DaCategoria.Update(DS, Tabla);
        }
        public void Dispose()
        {
            DS.Dispose();
        }



    }
}
