using System.Data;

namespace AppForSEII2526.API.Models
{
    public class Alquilar
    {
        public int id {  get; set; }
        public string apellidoCliente { get; set; }
        public string correo { get; set;}
        [Required(AllowEmptyStrings =false, ErrorMessage ="Por favor pon tu direccion de envio")]
        public string direccionEnvio {  get; set; }
        public DateTime fechaAlquiler {  get; set; }
        public DateTime fechaFin {  get; set; }
        public DateTime fechaInicio {  get; set; }
        public string nombreCliente {  get; set; }
        public int numeroTelefono { get; set; }
        public int periodo {  get; set; }
        public double precioTotal {  get; set; }
        
        public IList<AlquilarItem> alquilarItems { get; set; }

    }
}
