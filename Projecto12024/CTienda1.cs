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
        DataSet DS;
        String Tabla = "Productos";

        public CTienda1()
        {
            OleDbConnection cnn = new OleDbConnection();
            cnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Tienda.mdb";
            cnn.Open();
            DS = new DataSet();
            //Tabla tienda
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.TableDirect;
            cmd.CommandText = Tabla;
            OleDbDataAdapter DA = new OleDbDataAdapter(cmd);
            DA.Fill(DS, Tabla);
            DataColumn[] pk = new DataColumn[1];
            pk[0] = DS.Tables[Tabla].Columns["Codigo"];
            DS.Tables[Tabla].PrimaryKey = pk;
            OleDbCommandBuilder cb = new OleDbCommandBuilder(DA);
            cnn.Close();
        }
        public DataTable GetTienda()
        {
            if (DS != null && DS.Tables.Contains(Tabla))
            {
                return DS.Tables[Tabla];
            }
            return null;
        }
        public void Dispose()
        {
            DS.Dispose();
        }

    }
}
