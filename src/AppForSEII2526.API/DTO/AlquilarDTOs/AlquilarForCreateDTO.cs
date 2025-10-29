using System.Data;

namespace AppForSEII2526.API.DTO.AlquilarDTOs
{
    public class AlquilarForCreateDTO
    {
        public AlquilarForCreateDTO(string nombreCliente, string apellidoCliente, string direccionEnvio, PaymentMethodTypes paymentMethod, DateTime fechaInicio, DateTime fechaFin, IList<AlquilarItemDTO> alquilarItems)
        {
            NombreCliente = nombreCliente ?? throw new ArgumentNullException(nameof(nombreCliente));
            ApellidoCliente = apellidoCliente ?? throw new ArgumentNullException(nameof(apellidoCliente));
            DireccionEnvio = direccionEnvio ?? throw new ArgumentNullException(nameof(direccionEnvio));
            PaymentMethod = paymentMethod;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            AlquilarItem = alquilarItems ?? throw new ArgumentNullException(nameof(alquilarItems));
        }

        public AlquilarForCreateDTO()
        {
            AlquilarItem = new List<AlquilarItemDTO>();
        }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Dirección de envio")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "La dirección de envio debe tener 10 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, pon la dirección de envio")]
        public string DireccionEnvio { get; set; }
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, pon el nombre del cliente")]
        public string NombreCliente { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, pon el apellido del cliente")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El apellido del cliente debe tener al menos 10 caracteres")]
        public string ApellidoCliente { get; set; }

        public DateTime FechaFin { get; set; }
        public DateTime FechaInicio { get; set; }  

        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        private int NumeroDiasAlquiler
        {
            get
            {
                return (FechaFin - FechaInicio).Days;
            }
        }

        public IList<AlquilarItemDTO> AlquilarItem { get; set; } 

        //[Display(Name = "Precio total")]
        //[JsonPropertyName("PrecioTotal")]
        //public double TotalPrice
        //{
        //    get
        //    {
        //        return AlquilarItem.Sum(ri => ri.precio * NumberOfDays);
        //    }
        //}

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }
        public override bool Equals(object? obj)
        {
            return obj is AlquilarForCreateDTO dto &&
                   DireccionEnvio == dto.DireccionEnvio &&
                   CompareDate(FechaFin, dto.FechaFin) &&
                   CompareDate(FechaInicio, dto.FechaInicio) &&
                   PaymentMethod == dto.PaymentMethod && 
                ApellidoCliente == dto.ApellidoCliente &&
                   NombreCliente == dto.NombreCliente &&
                     AlquilarItem.SequenceEqual(dto.AlquilarItem);

        }

    }
}
