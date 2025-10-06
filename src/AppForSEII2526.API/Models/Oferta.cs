namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
        public DateTime fechaFinal {  get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaOferta { get; set; }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public tiposMetodoPago metodoPago { get; set; }
        public enum tiposMetodoPago
        {
            TarjetaCredito,
            PayPal,
            Efectivo
        }
        [Required(ErrorMessage = "Debe indicar a quién va dirigida la oferta.")]
        public tiposDirigidaOferta dirigidaOferta { get; set; }
        public enum tiposDirigidaOferta
        {
            Socios,
            Clientes
        }

        public List<OfertaItem> ofertaItems { get; set; }

    }

}
