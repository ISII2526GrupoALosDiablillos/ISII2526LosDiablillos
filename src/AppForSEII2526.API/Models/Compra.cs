using System;
using System.Security.Cryptography.X509Certificates;

public class Compra
{
	[Key]
    public int id {  get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su dirección")]
    public String direccionEnvio { get; set; }
	public DateTime fechaCompra {  get; set; }
	public double preciototal { get; set; }
	public IList<CompraItem> compraItem { get; set; }
	public DateTime fechaInicio { get; set; }
	public DateTime fechaRecibo { get; set; }
	public Compra() { }
	public Compra(String direccionEnvio, DateTime fechaCompra, double preciototal, IList<CompraItem> compraItem, ApplicationUser atributos)
	{
		this.direccionEnvio = direccionEnvio;
		this.fechaCompra = fechaCompra;
		this.preciototal = preciototal;
		this.compraItem = compraItem ?? new List<CompraItem>();
		this.atributos = atributos;
	}
	public ApplicationUser atributos {  get; set; }
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

	public PaymentMethodTypes metodoPago { get; set; }
}