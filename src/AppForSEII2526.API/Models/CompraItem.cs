using System;

[PrimaryKey(nameof(idCompra),nameof(idHerramienta))]
public class CompraItem
{
	public int cantidad;
	[Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca una descripción.")]
	public String descripcion;
	public int idCompra;
	public int idHerramienta;
	public double precio;
}
