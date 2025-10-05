using System;
using System.Security.Cryptography.X509Certificates;

public class Compra
{
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su apellido.")]
    public String apellidoCliente {  get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su correo electrónico.")]
    public String correoElectronico { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su dirección")]
    public String direccionEnvio { get; set; }
	public DateTime fechaCompra {  get; set; }
	public int id {  get; set; }
	[Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su nombre.")]
	public String nombreCliente { get; set; }
	public double preciototal { get; set; }
    public int telefono { get; set; }

	public Lista<CompraItem> ComprasItem { get; set; }

	public tiposMetodoPago metodoPago { get; set; }

	public enum tiposMetodoPago
	{
		TarjetaCredito,
		PayPal,
		Efectivo
	}
}