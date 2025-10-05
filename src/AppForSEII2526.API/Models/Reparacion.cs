namespace AppForSEII2526.API.Models
{
    public class Reparacion
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduce tu Apellido.")]

        public String apellidoCliente { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduce la Fecha de Entrega que desea.")]

        public DateTime fechaEntrega { get; set; }
        public DateTime fechaRecogida { get; set; }
        public int Id { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="Por favor introduce tu nombre.")]
        public String nombreCliente { get; set; }
        public String numTelefono { get; set; }
        public float precioTotal { get; set; }
        
    }
}
