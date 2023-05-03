using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Producto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }


        public void Guardar(Producto producto)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlTransaction transaction;
                    con.Open();
                    transaction = con.BeginTransaction();
                    try
                    {
                        string query = "INSERT INTO Productos " +
                          "(Descripcion, PrecioUnitario) " +
                          "VALUES " +
                          "(@Descripcion, @PrecioUnitario);select scope_identity()";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                            cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);

                            string ejecutaQuery = cmd.ExecuteScalar().ToString();
                            bool sePudoConvertir = int.TryParse(ejecutaQuery, out int idProducto);

                            if (!sePudoConvertir)
                            {
                                throw new Exception("Ocurrio un error al obtener el id del producto");
                            }
                            producto.Id = idProducto;
                        }

                        ProductoExistencia existencia = new ProductoExistencia();
                        existencia.AgregarExistenciaEnCero(con, transaction, producto.Id);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void Eliminar(Producto producto)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlTransaction transaction;
                    con.Open();
                    transaction = con.BeginTransaction();
                    try
                    {
                        string query = "DELETE INTO Productos " +
                          "(Descripcion, PrecioUnitario) " +
                          "VALUES " +
                          "(@Descripcion, @PrecioUnitario);select scope_identity()";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                            cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);

                            string ejecutaQuery = cmd.ExecuteScalar().ToString();
                            bool sePudoConvertir = int.TryParse(ejecutaQuery, out int idProducto);

                            if (!sePudoConvertir)
                            {
                                throw new Exception("Ocurrio un error al obtener el id del producto");
                            }
                            producto.Id = idProducto;
                        }

                        ProductoExistencia existencia = new ProductoExistencia();
                        existencia.AgregarExistenciaEnCero(con, transaction, producto.Id);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                }


            }
            catch (Exception ex) { }

        }
    }
}

