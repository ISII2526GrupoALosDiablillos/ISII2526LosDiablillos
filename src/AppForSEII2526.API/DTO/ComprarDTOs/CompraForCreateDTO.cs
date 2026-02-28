using AppForSEII2526.API.DTOs;
using Humanizer;

namespace AppForSEII2526.API.DTO.ComprarDTOs
{
    public class CompraForCreateDTO
    {
        public String Nombre_cliente { get; set; }
        public String Apellidos_cliente { get; set; }
        public String? UserName { get; set; }
        public String DireccionEnvio { get; set; }
        public PaymentMethodTypes Pago { get; set; }
        public int? Telefono { get; set; }
        public String? CorreoElectronico { get; set; }
        public double PrecioTotal { get; set; }
        public IList<CompraItemDTO> CompraItems { get; set; }
        public CompraForCreateDTO()
        {
            CompraItems = new List<CompraItemDTO>();
        }
        public CompraForCreateDTO(string username, string nombre_cliente, string apellidos_cliente, string direccionEnvio, PaymentMethodTypes pago, int? telefono, string? correoElectronico, double precioTotal, IList<CompraItemDTO> compraItems)
        {
            UserName = username;
            Nombre_cliente = nombre_cliente;
            Apellidos_cliente = apellidos_cliente;
            DireccionEnvio = direccionEnvio;
            Pago = pago;
            Telefono = telefono;
            CorreoElectronico = correoElectronico;
            PrecioTotal = precioTotal;
            CompraItems = compraItems ?? new List<CompraItemDTO>();
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre_cliente, Apellidos_cliente, DireccionEnvio, Pago, Telefono, CorreoElectronico, PrecioTotal, CompraItems);
        }
    }
}