namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
        public DateTime fechaFinal {  get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaOferta { get; set; }
        public int Id { get; set; }
        
        public tiposMetodoPago metodoPago { get; set; }
        public enum tiposMetodoPago
        {
            TarjetaCredito,
            PayPal,
            Efectivo
        }
        
        public tiposDirigidaOferta dirigidaOferta { get; set; }
        public enum tiposDirigidaOferta
        {
            Socios,
            Clientes
        }
    }

}
