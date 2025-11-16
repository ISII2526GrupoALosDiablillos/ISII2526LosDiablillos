using System.Data;
using System.Drawing.Printing;

namespace AppForSEII2526.API.Models
{
    public enum PaymentMethodTypes
    {
        CreditCard,
        PayPal,
        Cash
    }
    public class Alquilar
    {
        [Key]
        public int id {  get; set; }        
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Direccion de envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, pon tu direccion de envio")]
        public string direccionEnvio {  get; set; }
        public DateTime fechaAlquiler {  get; set; }
        public DateTime fechaFin {  get; set; }
        public DateTime fechaInicio {  get; set; }
        public int periodo {  get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public double precioTotal {  get; set; }
        public string correo { get; set; }
        public int numeroTelefono { get; set; }
        public ApplicationUser applicationUser { get; set; }

        public Alquilar()
        {

        }
        public Alquilar(string nombreCliente, string apellidoCliente, string direccionEnvio, DateTime fechaAlquiler, PaymentMethodTypes metodoPago, DateTime fechaFin, DateTime fechaInicio, IList<AlquilarItem>alquilarItems, ApplicationUser applicationUser)
        {
            PrecioTotal = alquilarItems.Sum(ri => ri.herramienta.precio * periodo);
            DireccionEnvio = direccionEnvio;
            FechaAlquiler = fechaAlquiler;
            FechaFin = fechaFin;
            FechaInicio = fechaInicio;
            
            MetodoPago = metodoPago;

        }
        public int Id { get; set; }
        public string DireccionEnvio { get; set; }
        public DateTime FechaAlquiler { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaInicio { get; set; }
        public int Periodo { get; set; }
        public double PrecioTotal { get; set; }
        


        public PaymentMethodTypes MetodoPago { get; set; }
        public IList<AlquilarItem> alquilarItems { get; set; }
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
