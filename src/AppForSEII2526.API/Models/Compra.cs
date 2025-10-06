using System;
using System.Security.Cryptography.X509Certificates;

public class Compra
{
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su apellido.")]
    public String ApellidoCliente {  get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su correo electrónico.")]
    public String CorreoElectronico { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su dirección")]
    public String DireccionEnvio { get; set; }
	public DateTime FechaCompra {  get; set; }
	public int Id {  get; set; }
	[Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su nombre.")]
	public String NombreCliente { get; set; }
	public double Preciototal { get; set; }
    public int Telefono { get; set; }
	public List<CompraItem> ComprasItem { get; set; }
	public Compra() { }
	public Compra(String apellidoCliente, String correoElectronico, String direccionEnvio, DateTime fechaCompra, int id, String nombreCliente, double preciototal, int telefono)
	{
		ApellidoCliente = apellidoCliente;
		CorreoElectronico = correoElectronico;
		DireccionEnvio = direccionEnvio;
		FechaCompra = fechaCompra;
		Id = id;
		NombreCliente = nombreCliente;
		Preciototal = preciototal;
		Telefono = telefono;
	}
	public String telefono { get; set; }
	public String direccionEnvio { get; set; }
	public String apellidoCliente { get; set; }
	public String correoElectronico { get; set; }
	public DateTime fechaCompra { get; set; }
	public String nombreCliente { get; set; }
	public double preciototal { get; set; }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

	public tiposMetodoPago metodoPago { get; set; }
	public enum tiposMetodoPago
	{
		TarjetaCredito,
		PayPal,
		Efectivo
	}
}