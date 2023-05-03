using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ProductoExistencia
    {
        public void ActualizarExistencia(SqlConnection con, SqlTransaction transaction, VentaDetalle concepto)
        {
            string query = "Update Existencias " +
                                        "set Existencia = Existencia-@Cantidad " +
                                        "where ProductoId = @ProductoId";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;

                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                cmd.ExecuteNonQuery();
            }
        }
        public void AgregarExistenciaEnCero(SqlConnection con, SqlTransaction transaction, int ProductoId)
        {
            string query = "Instert Into Existencias (Existencia, ProductoId) VALUES (0, @ProductoId)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;

                cmd.Parameters.AddWithValue("@ProductoId", ProductoId);
                cmd.ExecuteNonQuery();
            }
        }
        public void AgregarExistencia(int ProductoId, decimal Cantidad)
        //SqlConnection con, SqlTransaction transaction, VentaDetalle concepto
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
                        string query = "Update Existencias " +
                                       "set Existencia = Existencia + @Cantidad " +
                                       "where ProductoId = @ProductoId";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@ProductoId", ProductoId);
                            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
                            cmd.ExecuteNonQuery();
                        }
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
    }
}
