using System;

[PrimaryKey(nameof(idCompra),nameof(idHerramienta))]
public class CompraItem
{
	public int cantidad { get; set; }
	[Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca una descripción.")]
	public String descripcion { get; set; }
	public int idCompra { get; set; }
	public int idHerramienta { get; set; }
	public double precio { get; set; }

	public Compra compra {  get; set; }
	public Herramienta herramienta { get; set; }
}
