using System;
using System.Security.Cryptography.X509Certificates;

public class Compra
{
	public String apellidoCliente {  get; set; }
	public String correoElectronico { get; set; }
	public String direccionEnvio { get; set; }
	public DateTime fechaCompra {  get; set; }
	public int id {  get; set; }
	public String nombreCliente { get; set; }
	public double preciototal { get; set; }
	public int telefono { get; set; }
	public tiposMetodoPago metodoPago { get; set; }

	public enum tiposMetodoPago
	{
		TarjetaCredito,
		PayPal,
		Efectivo
	}
}