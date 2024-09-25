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
            CNN.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=DB/Tienda.mdb";
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
            pk[0] = DS.Tables[Tabla].Columns["Código"];
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
            try
            {
                CNN.Open();
                transaccion = CNN.BeginTransaction();
                // Lógica de inserción o actualización
                InsertProducto(transaccion, Nombre, Descripcion, Precio, Stock, Categoria);
                transaccion.Commit();
            }
            catch (Exception ex)
            {
                transaccion?.Rollback();
                throw new Exception("Error en la base de datos: " + ex.Message);
            }
            finally
            {
                CNN.Close();
            }
        }

        public void InsertProducto(OleDbTransaction transaccion,
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

        public void ActualizarProducto(Int32 Codigo,
                            String Nombre,
                            String Descripcion,
                            String Precio,
                            String Stock,
                            Int32 Categoria)
        {
            OleDbTransaction transaccion = null;
            try
            {
                CNN.Open();
                transaccion = CNN.BeginTransaction();
                // Lógica de inserción o actualización
                ActualizarDatos(transaccion, Codigo, Nombre, Descripcion, Precio, Stock, Categoria);
                transaccion.Commit();
            }
            catch (Exception ex)
            {
                transaccion?.Rollback();
                throw new Exception("Error en la base de datos: " + ex.Message);
            }
            finally
            {
                CNN.Close();
            }
            
        }
        public void ActualizarDatos(OleDbTransaction transaccion,
                            Int32 Codigo,
                            String Nombre,
                            String Descripcion,
                            String Precio,
                            String Stock,
                            Int32 Categoria)
        {
            CmdTienda.Transaction = transaccion;

            // Busco el producto a modificar usando la clave primaria
            DataRow dr = DS.Tables[Tabla].Rows.Find(Codigo);

            if (dr != null)
            {
                // Actualizo los datos
               // dr["Código"] = Codigo;
                dr["Nombre"] = Nombre;
                dr["Descripción"] = Descripcion;
                dr["Precio"] = Precio;
                dr["Stock"] = Stock;
                dr["IdCategorías"] = Categoria;

                DaTienda.Update(DS, Tabla);
            }
            else
            {
                throw new Exception("El producto no fue encontrado.");
            }
        }

        public void Dispose()
        {
            DS.Dispose();
        }

    }
}
