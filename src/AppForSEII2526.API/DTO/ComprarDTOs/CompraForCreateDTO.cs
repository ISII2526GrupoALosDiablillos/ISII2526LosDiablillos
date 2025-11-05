namespace AppForSEII2526.API.DTO.ComprarDTOs
{
    public class CompraForCreateDTO
    {
        public CompraForCreateDTO() {
            CompraItems = new List<CompraItemForCreateDTO>();
        }
        private String nombre {  get; set; }
        private String material { get; set; }
        private double precio { get; set; }
        private String nombre_cliente { get; set; }
        private String apellidos_cliente { get; set; }
        private String direccionEnvio { get; set; }
        private PaymentMethodTypes pago { get; set; }
        private int? telefono { get; set; }
        private String? correoElectronico { get; set; }
        public List<CompraItemForCreateDTO> CompraItems { get; set; }

    }

    public class CompraItemForCreateDTO()
    {
        public int cantidad { get; set; }
        public String descripcion { get; set; }
    }
}
