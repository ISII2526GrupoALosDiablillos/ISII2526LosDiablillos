namespace AppForSEII2526.API.DTOs
{
    public class CompraItemDTO
    {
        public String nombre { get; set; }
        public String material { get; set; }
        public double precio { get; set; }
        public String descripcion { get; set; }
        public String fabricante { get; set; }
        public int cantidad { get; set; }
        public int herramientaId { get; set; }
        public int compraId { get; set; }

        public CompraItemDTO(String nombre, String material, double precio, String descripcion, int cantidad, String fabricante, int herramientaId, int compraId)
        {
            this.nombre = nombre;
            this.material = material;
            this.precio = precio;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
            this.herramientaId = herramientaId;
            this.compraId = compraId;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemDTO dTO &&
                   nombre == dTO.nombre &&
                   material == dTO.material &&
                   precio == dTO.precio &&
                   fabricante ==dTO.fabricante &&
                   descripcion == dTO.descripcion &&
                   cantidad == dTO.cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(nombre, material, precio, fabricante, descripcion, cantidad);
        }
    }
}