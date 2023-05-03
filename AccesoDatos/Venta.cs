using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Venta
    {
        public int Id { get; set; }
        public int Folio { get; private set; }
        public DateTime Fecha { get; private set; }
        public int CLienteId { get; set; }
        public decimal Total { get; set; }
        public List<VentaDetalle> Conceptos { get; set; } = new List<VentaDetalle>();

        public void GuardarVenta(Venta venta)
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
                        ClaseFolio folio = new ClaseFolio();
                        int folioActual = folio.ObtenerFolioActual(con, transaction) + 1;

                        string query = "INSERT INTO Ventas " +
                            "(Folio,Fecha,ClienteId,Total) " +
                            "VALUES " +
                            "(@Folio,@Fecha,@ClienteId,@Total);select scope_identity()";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@Folio", folioActual);
                            cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                            cmd.Parameters.AddWithValue("@ClienteId", venta.CLienteId);
                            cmd.Parameters.AddWithValue("@Total", venta.Total);

                            //int idVenta = 0;
                            //bool sePudoCovertir   = false;

                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out int idVenta))
                            {
                                throw new Exception("Ocurrio un error al obtener el id de la venta");
                            }
                            Id = idVenta;
                        }

                        foreach (VentaDetalle concepto in venta.Conceptos)
                        {
                            concepto.VentaId = Id;
                            concepto.GuardarVentaDetalle(con, transaction, concepto);

                            ProductoExistencia productoExistencia = new ProductoExistencia();
                            productoExistencia.ActualizarExistencia(con, transaction, concepto);
                        }


                        folio.IncrementarFolio(con, transaction);

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