using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTO.Oferta
{
    public class OfertaForCreateDTO
    {
        public OfertaForCreateDTO()
        {
            OfertaItems = new List<OfertaItemForCreateDTO>();
        }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha final es obligatoria.")]
        public DateTime FechaFinal { get; set; }

        
        public DateTime FechaOferta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe indicar el método de pago.")]
        public tiposMetodoPago MetodoPago { get; set; }

        [Required(ErrorMessage = "Debe indicar a quién va dirigida la oferta.")]
        public tiposDirigidaOferta DirigidaOferta { get; set; }

        [MinLength(1, ErrorMessage = "Debe incluir al menos una herramienta en la oferta.")]
        public List<OfertaItemForCreateDTO> OfertaItems { get; set; }
    }

    public class OfertaItemForCreateDTO
    {
        [Required(ErrorMessage = "Debe indicar la herramienta.")]
        public int HerramientaId { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        public double Porcentaje { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio final debe ser mayor que 0.")]
        public double PrecioFinal { get; set; }
    }
}
