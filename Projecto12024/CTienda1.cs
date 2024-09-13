using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Projecto12024
{
    public class CTienda1
    {
        OleDbConnection CNN;
        OleDbCommand CmdTienda;
        OleDbDataAdapter DaTienda;
        DataSet DS;
        String Tabla = "Productos";

        public CTienda1()
        {
            CNN = new OleDbConnection();
            CNN.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Tienda.mdb";
            CNN.Open();
            DS = new DataSet();
            //Tabla tienda
            CmdTienda = new OleDbCommand();
            CmdTienda.Connection = CNN;
            CmdTienda.CommandType = CommandType.TableDirect;
            CmdTienda.CommandText = Tabla;
            DaTienda = new OleDbDataAdapter(CmdTienda);
            DaTienda.Fill(DS, Tabla);
            DataColumn[] pk = new DataColumn[1];
            pk[0] = DS.Tables[Tabla].Columns["Codigo"];
            DS.Tables[Tabla].PrimaryKey = pk;
            OleDbCommandBuilder cb = new OleDbCommandBuilder(DaTienda);
            CNN.Close();
        }
        public DataTable GetTienda()
        {
            if (DS != null && DS.Tables.Contains(Tabla))
            {
                return DS.Tables[Tabla];
            }
            return null;
        }
        public void AgregarProducto(String Nombre,
                                    String Descripcion,
                                    String Precio,
                                    String Stock,
                                    Int32 Categoria)
        {
            OleDbTransaction transaccion = null;
            CNN.Open();
            transaccion = CNN.BeginTransaction();
            insertProducto(transaccion,Nombre,Descripcion,Precio,Stock, Categoria);
            transaccion.Commit();
            CNN.Close();    

        }

        public void insertProducto(OleDbTransaction transaccion,
                                    String Nombre,
                                    String Descripcion,
                                    String Precio,
                                    String Stock,
                                    Int32 Categoria)
        {
            CmdTienda.Transaction = transaccion;
            DataRow dr = DS.Tables[Tabla].NewRow();
            dr["Nombre"] = Nombre;
            dr["Descripción"] = Descripcion;
            dr["Precio"] = Precio;
            dr["Stock"] = Stock;
            dr["IdCategorías"] = Categoria;
            DS.Tables[Tabla].Rows.Add(dr);
            DaTienda.Update(DS, Tabla);
        }
        public void Dispose()
        {
            DS.Dispose();
        }

    }
}
