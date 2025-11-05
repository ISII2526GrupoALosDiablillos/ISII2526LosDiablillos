namespace AppForSEII2526.API.DTO.AlquilarDTOs
{
    public class AlquilarDetailDTO : AlquilarForCreateDTO
    {
        public AlquilarDetailDTO(int id, DateTime alquilerFecha, string nombreCliente, string apellidoCliente, string direccionEnvio, PaymentMethodTypes paymentMethod, DateTime fechaFin, DateTime fechaInicio, IList<AlquilarItemDTO> alquilarItems ) : base(nombreCliente, apellidoCliente, direccionEnvio, paymentMethod, fechaFin, fechaInicio, alquilarItems )
        {
            Id = id;
            AlquilerFecha = alquilerFecha;
        }
        public int Id { get; set; }
        public DateTime AlquilerFecha { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilarDetailDTO dTO &&
                   base.Equals(obj) &&
                   Id == dTO.Id &&
                   PrecioTotal == dTO.PrecioTotal &&
                   CompareDate(AlquilerFecha, dTO.AlquilerFecha);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, AlquilerFecha);
        }

    }
}
