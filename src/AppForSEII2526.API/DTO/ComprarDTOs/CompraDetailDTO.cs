namespace AppForSEII2526.API.DTOs
{
    public class CompraDetailDTO
    {
        public int id {  get; set; }
        public String nombre_cliente { get; set; }
        public String apellido_cliente { get; set; }
        public String direccion { get; set; }
        public double preciototal { get; set; }
        public DateTime fechaCompra { get; set; }
        public IList<CompraItemDTO> compraItems { get; set; }
        public CompraDetailDTO() { }
        public CompraDetailDTO(int id, String nombre_cliente, String apellido_cliente, String direccion, double preciototal, DateTime fechaCompra, IList<CompraItemDTO> compraItems)
        {
            this.id = id;
            this.nombre_cliente = nombre_cliente;
            this.apellido_cliente = apellido_cliente;
            this.direccion = direccion;
            this.preciototal = preciototal;
            this.fechaCompra = fechaCompra;
            this.compraItems = compraItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraDetailDTO dTO &&
                   id == dTO.id &&
                   nombre_cliente == dTO.nombre_cliente &&
                   apellido_cliente == dTO.apellido_cliente &&
                   direccion == dTO.direccion &&
                   preciototal == dTO.preciototal &&
                   fechaCompra == dTO.fechaCompra &&
                   //Cambiar por SequenceEquals --> AppForMovies --> DTOCreateRental.
                   EqualityComparer<IList<CompraItemDTO>>.Default.Equals(compraItems, dTO.compraItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, nombre_cliente, apellido_cliente, direccion, preciototal, fechaCompra, compraItems);
        }
    }
}