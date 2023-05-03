using AccesoDatos;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VentasTransaction
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GuardarVenta();
            AgregarCliente();
            EliminarP();
            AgregarExistenciaP();
        }

        //Debemos reubicar este metodo 
        private void GuardarVenta()
        {
            try
            {
                Venta venta = new Venta();
                venta.CLienteId = 1;


                VentaDetalle producto1 = new VentaDetalle();
                producto1.ProductoId = 1;
                producto1.Cantidad = 1;
                producto1.Descripcion = "Azucar kg";
                producto1.PrecioUnitario = 27.00m;
                producto1.Importe = producto1.Cantidad * producto1.PrecioUnitario;

                venta.Total += producto1.Importe;

                VentaDetalle producto2 = new VentaDetalle();
                producto2.ProductoId = 2;
                producto2.Cantidad = 1;
                producto2.Descripcion = "Jugo Mango";
                producto2.PrecioUnitario = 10.00m;
                producto2.Importe = producto2.Cantidad * producto2.PrecioUnitario;

                venta.Total += producto2.Importe;

                venta.Conceptos.Add(producto1);
                venta.Conceptos.Add(producto2);

                venta.GuardarVenta(venta);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ocurrio un error al guardar la venta {ex.Message}");
            }
        }

        private void AgregarCliente()
        {
            Cliente clienteN = new Cliente();
            clienteN.Id = 1;
            clienteN.Nombre = "Ray";
            clienteN.AgregarCliente(clienteN);

            Cliente cliente2 = new Cliente();
            cliente2.Id = 1;
            cliente2.Nombre = "Sebas";
            cliente2.AgregarCliente(cliente2);

        }

        private void EliminarP()
        {
            Producto eliminarP= new Producto();
            eliminarP.Id = 5;
            eliminarP.Descripcion = "Cacahuates";
        }

        private void AgregarExistenciaP()
        {
            Producto producto_6 = new Producto();
            producto_6.Descripcion = "Harina Kg";
            producto_6.PrecioUnitario = 30;
            producto_6.Guardar(producto_6);
        }

       
    }
}

