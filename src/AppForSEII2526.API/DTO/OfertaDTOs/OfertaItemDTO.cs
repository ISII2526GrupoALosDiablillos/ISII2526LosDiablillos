namespace AppForSEII2526.API.DTO.Oferta
{
    public class OfertaItemDTO
    {
        public OfertaItemDTO(
            int herramientaId,
            string nombreHerramienta,
            string material,
            string fabricante,
            decimal precioOriginal,
            double porcentajeDescuento,
            double precioFinal)
        {
            HerramientaId = herramientaId;
            NombreHerramienta = nombreHerramienta;
            Material = material;
            Fabricante = fabricante;
            PrecioOriginal = precioOriginal;
            PorcentajeDescuento = porcentajeDescuento;
            PrecioFinal = precioFinal;
        }

        public int HerramientaId { get; set; }
        public string NombreHerramienta { get; set; }
        public string Material { get; set; }
        public string Fabricante { get; set; }
        public decimal PrecioOriginal { get; set; }
        public double PorcentajeDescuento { get; set; }
        public double PrecioFinal { get; set; }
    }
}
