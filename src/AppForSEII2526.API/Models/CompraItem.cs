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
        Compra = compra;
        Herramienta = herramienta;
    }
    
    public Compra Compra { get; set; }
    public Herramienta Herramienta { get; set; }
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
