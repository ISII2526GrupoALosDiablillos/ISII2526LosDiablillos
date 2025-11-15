namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionForCreateDTO
    {
        public ReparacionForCreateDTO(string nombreCliente, string apellidoCliente, DateTime fechaRecogida,
            DateTime fechaEntrega, IList<RepararItemDTO> reparacionItems)
        {
            NombreCliente = nombreCliente ?? throw new ArgumentNullException(nameof(nombreCliente));
            ApellidoCliente = apellidoCliente ?? throw new ArgumentNullException(nameof(apellidoCliente));
            FechaRecogida = fechaRecogida;
            FechaEntrega = fechaEntrega;
            ReparacionItems = reparacionItems ?? throw new ArgumentNullException(nameof(reparacionItems));
        }

        public ReparacionForCreateDTO()
        {
            ReparacionItems = new List<RepararItemDTO>();
        }

        public DateTime FechaRecogida { get; set; }

        public DateTime FechaEntrega { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
        public int Telefono { get; set; }

        [Required]
        public MetodosPago PaymentMethod { get; set; }

        [EmailAddress]
        [Required]
        public string NombreCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduce tu Nombre y Apellido")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Nombre y Apellido deben tener al menos 10 caracteres")]
        public string ApellidoCliente { get; set; }

        public IList<RepararItemDTO> ReparacionItems { get; set; }

       

        [Display(Name = "Precio Total")]
        [JsonPropertyName("PrecioTotal")]
        public double PrecioTotal
        {
            get
            {
                return ReparacionItem.Sum(ri => Precio * Cantidad);
            }
        }


        public override bool Equals(object? obj)
        {
            return obj is ReparacionForCreateDTO dTO &&
                   NombreCliente == dTO.NombreCliente &&
                   ApellidoCliente == dTO.ApellidoCliente &&
                     FechaRecogida == dTO.FechaRecogida &&
                     FechaEntrega == dTO.FechaEntrega &&
                   ReparacionItems.SequenceEqual(dTO.ReparacionItems) &&
                   PrecioTotal == dTO.PrecioTotal;
        }
    }
}