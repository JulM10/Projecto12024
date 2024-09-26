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

            // Obtener el valor máximo de 'Id' ya existente en la tabla
            int maxId = 1;
            if (DS.Tables[Tabla].Rows.Count > 0)
            {
                maxId = DS.Tables[Tabla].AsEnumerable()
                                        .Max(row => row.Field<int>("Código"));
            }

            // Configurar 'Id' como autoincremental en el DataSet, comenzando desde el valor máximo + 1
            DS.Tables[Tabla].Columns["Código"].AutoIncrement = true;
            DS.Tables[Tabla].Columns["Código"].AutoIncrementSeed = maxId + 1; // Comenzar desde el valor máximo existente
            DS.Tables[Tabla].Columns["Código"].AutoIncrementStep = 1;

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
        //Elimino un producto
        public void EliminarProducto(int codigo)
        {
            OleDbTransaction transaccion = null;
            try
            {
                CNN.Open();
                transaccion = CNN.BeginTransaction();
                CmdTienda.Transaction = transaccion;
                // Buscar el producto por su clave primaria (Código)
                DataRow dr = DS.Tables[Tabla].Rows.Find(codigo);

                if (dr != null)
                {
                    // Eliminar la fila del dataset
                    dr.Delete();
                    DaTienda.Update(DS, Tabla);
                    transaccion.Commit();
                }
                else
                {
                    throw new Exception("Producto no encontrado.");
                }
            }
            catch (Exception ex)
            {
                if (transaccion != null)
                {
                    transaccion.Rollback();
                }
                throw new Exception("Error al eliminar el producto: " + ex.Message);
            }
            finally
            {
                CNN.Close();
            }
        }

        public void Dispose()
        {
            DS.Dispose();
        }

    }
}
