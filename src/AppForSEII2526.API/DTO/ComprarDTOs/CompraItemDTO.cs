namespace AppForSEII2526.API.DTOs
{
    public class CompraItemDTO
    {
        public string nombre { get; set; }
        public string material { get; set; }
        public decimal precio { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public CompraItemDTO() { }
        public CompraItemDTO(string nombre, string material, decimal precio, string descripcion, int cantidad)
        {
            this.nombre = nombre;
            this.material = material;
            this.precio = precio;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
        }
        public override bool Equals(object? obj)
        {
            return obj is CompraItemDTO dTO &&
                   nombre == dTO.nombre &&
                   material == dTO.material &&
                   precio == dTO.precio &&
                   descripcion == dTO.descripcion &&
                   cantidad == dTO.cantidad;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(nombre, material, precio, descripcion, cantidad);
        }
    }
}