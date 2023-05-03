using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ClaseFolio
    {
        public int ObtenerFolioActual(SqlConnection con, SqlTransaction transaction)
        {
            string query = "select top(1) Folio from Folios";
            int folioActual = 0;
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;

                if (!int.TryParse(cmd.ExecuteScalar().ToString(), out folioActual))
                {
                    throw new Exception("Ocurrio un error al obtener el folio");
                }
            }
            return folioActual;
        }
        public void IncrementarFolio(SqlConnection con, SqlTransaction transaction)
        {
            try
            {
                string query = "Update Folios set Folio = Folio + 1 ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = transaction;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
            
        }
    }
}
