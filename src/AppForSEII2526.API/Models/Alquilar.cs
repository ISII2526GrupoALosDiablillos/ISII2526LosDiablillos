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
        public string apellidoCliente { get; set; }
        public string correo { get; set;}
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Direccion de envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, pon tu direccion de envio")]
        public string direccionEnvio {  get; set; }
        public DateTime fechaAlquiler {  get; set; }
        public DateTime fechaFin {  get; set; }
        public DateTime fechaInicio {  get; set; }
        public string nombreCliente {  get; set; }
        public int numeroTelefono { get; set; }
        public int periodo {  get; set; }
        public double precioTotal {  get; set; }

        public Alquilar()
        {

        }
        public Alquilar(int id, string apellidoCliente, string correo, string direccionEnvio, DateTime fechaAlquiler, DateTime fechaFin, DateTime fechaInicio, string nombreCliente, int numeroTelefono, int periodo, double precioTotal,IList<AlquilarItem>alquilarItems)
        {
            PrecioTotal = precioTotal;
            Id = id;
            ApellidoCliente = apellidoCliente;
            Correo = correo;
            DireccionEnvio = direccionEnvio;
            FechaAlquiler = fechaAlquiler;
            FechaFin = fechaFin;
            FechaInicio = fechaInicio;
            NombreCliente = nombreCliente;
            NumeroTelefono = numeroTelefono;
            Periodo = periodo;
            
            
        }
        public int Id {  get; set; }
        public string ApellidoCliente {  get; set; }
        public string Correo { get; set; }
        public string DireccionEnvio {  get; set; }
        public DateTime FechaAlquiler {  get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaInicio { get; set; }
        public string NombreCliente {get; set;}
        public int NumeroTelefono { get; set;}
        public int Periodo { get; set;}
        public double PrecioTotal { get; set;}

        public PaymentMethodTypes metodoPago { get; set; }


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
