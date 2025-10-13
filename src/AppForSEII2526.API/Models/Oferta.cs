namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
        public Oferta() { }
        public Oferta(DateTime fechaFinal, DateTime fechaInicio, DateTime fechaOferta, int id, tiposMetodoPago metodoPago, tiposDirigidaOferta dirigidaOferta, IList<OfertaItem> ofertaItems)
        {
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = fechaOferta;
            Id = id;
            this.metodoPago = metodoPago;
            this.dirigidaOferta = dirigidaOferta;
            this.ofertaItems = ofertaItems;
        }

        public DateTime fechaFinal {  get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaOferta { get; set; }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public tiposMetodoPago metodoPago { get; set; }
        
        [Required(ErrorMessage = "Debe indicar a quién va dirigida la oferta.")]
        public tiposDirigidaOferta dirigidaOferta { get; set; }
        public ApplicationUser applicationUser { get; set; }


        public IList<OfertaItem> ofertaItems { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    public enum tiposMetodoPago
    {
        TarjetaCredito,
        PayPal,
        Efectivo
    }
    public enum tiposDirigidaOferta
    {
        Socios,
        Clientes
    }

}
