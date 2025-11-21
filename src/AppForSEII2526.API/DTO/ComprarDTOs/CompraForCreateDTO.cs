using AppForSEII2526.API.DTOs;
using Humanizer;

namespace AppForSEII2526.API.DTO.ComprarDTOs
{
    public class CompraForCreateDTO
    {
        public CompraForCreateDTO() {
            CompraItems = new List<CompraItemDTO>();
        }
        public int Id { get; set; }
        public String Nombre {  get; set; }
        public String Material { get; set; }
        public double Precio { get; set; }
        public String Nombre_cliente { get; set; }
        public String Apellidos_cliente { get; set; }
        public String UserName { get; set; }
        public String DireccionEnvio { get; set; }
        public PaymentMethodTypes Pago { get; set; }
        public int? Telefono { get; set; }
        public String? CorreoElectronico { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateTime FechaRecibo { get; set; }
        public IList<CompraItemDTO> CompraItems { get; set; }
        public CompraForCreateDTO(int id, string nombre, string material, double precio, string username, string nombre_cliente, string apellidos_cliente, string direccionEnvio, PaymentMethodTypes pago, int? telefono, string? correoElectronico, IList<CompraItemDTO> compraItems, DateTime fechaCompra, DateTime fechaRecibo)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            UserName = username;
            Nombre_cliente = nombre_cliente;
            Apellidos_cliente = apellidos_cliente;
            DireccionEnvio = direccionEnvio;
            Pago = pago;
            Telefono = telefono;
            CorreoElectronico = correoElectronico;
            CompraItems = compraItems ?? new List<CompraItemDTO>();
            FechaCompra = fechaCompra;
            FechaRecibo = fechaRecibo;
        }

    }
}
