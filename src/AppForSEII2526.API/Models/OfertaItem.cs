namespace AppForSEII2526.API.Models
{
    public class OfertaItem
    {
        [Key]
        public int idHerramienta { get; set; }
        [Required]
        public int idOferta { get; set; }
        [Range(0.01, 99.99, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        public double porcentaje { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio final debe ser mayor que 0.")]
        public double precioFinal {  get; set; }

        public Herramienta herramienta { get; set; }
        public Oferta oferta { get; set; }
    }
}
