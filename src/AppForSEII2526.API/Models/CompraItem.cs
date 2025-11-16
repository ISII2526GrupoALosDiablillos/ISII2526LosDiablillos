using System;

[PrimaryKey(nameof(compraId), nameof(herramientaId))]
public class CompraItem
{
    public int cantidad { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca una descripción.")]
    public String descripcion { get; set; }
    public int compraId { get; set; }
    public int herramientaId { get; set; }
    public double precio { get; set; }
    public CompraItem() { }
    public CompraItem(int cantidad, String descripcion, int compraId, int herramientaId, double precio, Compra compra, Herramienta herramienta)
    {
        this.cantidad = cantidad;
        this.descripcion = descripcion;
        this.compraId = compraId;
        this.herramientaId = herramientaId;
        this.precio = precio;
        this.compra = compra;
        this.herramienta = herramienta;
    }
    
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
