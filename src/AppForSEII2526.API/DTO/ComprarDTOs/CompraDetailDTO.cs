namespace AppForSEII2526.API.DTOs
{
    public class CompraDetailDTO
    {
        public String nombre_cliente { get; set; }
        public String apellido_cliente { get; set; }
        public String direccion { get; set; }
        public double preciototal { get; set; }
        public DateTime fechaCompra { get; set; }
        public CompraDetailDTO() { }
        public CompraDetailDTO(String nombre_cliente, String apellido_cliente, String direccion, double preciototal, DateTime fechaCompra)
        {
            this.nombre_cliente = nombre_cliente;
            this.apellido_cliente = apellido_cliente;
            this.direccion = direccion;
            this.preciototal = preciototal;
            this.fechaCompra = fechaCompra;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraDetailDTO dTO &&
                   nombre_cliente == dTO.nombre_cliente &&
                   apellido_cliente == dTO.apellido_cliente &&
                   direccion == dTO.direccion &&
                   preciototal == dTO.preciototal &&
                   fechaCompra == dTO.fechaCompra;
                   
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(nombre_cliente, apellido_cliente, direccion, preciototal, fechaCompra);
        }
    }
}