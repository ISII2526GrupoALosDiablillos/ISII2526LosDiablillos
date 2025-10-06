using System;

[PrimaryKey(nameof(IdCompra), nameof(IdHerramienta))]
public class CompraItem
{
    public int Cantidad { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca una descripción.")]
    public String Descripcion { get; set; }
    public int IdCompra { get; set; }
    public int IdHerramienta { get; set; }
    public double Precio { get; set; }
    public CompraItem() { }
    public CompraItem(int cantidad, String descripcion, int idCompra, int idHerramienta, double precio, Compra compra, Herramienta herramienta)
    {
        Cantidad = cantidad;
        Descripcion = descripcion;
        IdCompra = compra.Id;
        IdHerramienta = herramienta.id;
        Precio = precio;
    }
    public int cantidad { get; set; }
    public String descripcion { get; set; }
    public double precio { get; set; }
    public Compra compra { get; set; }
    public Herramienta herramienta { get; set; }
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
