namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(ofertaId))]
    public class OfertaItem
    {
        public OfertaItem() { }
        public OfertaItem(int herramientaId, int ofertaId, double porcentaje, double precioFinal)
        {
            this.herramientaId = herramientaId;
            this.ofertaId = ofertaId;
            this.porcentaje = porcentaje;
            this.precioFinal = precioFinal;
        }

        
        public int herramientaId { get; set; }
        
        public int ofertaId { get; set; }
        [Range(0.01, 99.99, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        public double porcentaje { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio final debe ser mayor que 0.")]
        public double precioFinal {  get; set; }

        public Herramienta herramienta { get; set; }
        public Oferta oferta { get; set; }

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
