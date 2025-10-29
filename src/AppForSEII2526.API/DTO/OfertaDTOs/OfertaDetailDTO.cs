namespace AppForSEII2526.API.DTO.Oferta
{
    public class OfertaDetailDTO
    {
        public OfertaDetailDTO(
            DateTime fechaInicio,
            DateTime fechaFinal,
            DateTime fechaOferta,
            tiposDirigidaOferta dirigidaOferta,
            tiposMetodoPago metodoPago,
            List<OfertaItemDTO> items)
        {
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            FechaOferta = fechaOferta;
            DirigidaOferta = dirigidaOferta;
            MetodoPago = metodoPago;
            Items = items ?? new List<OfertaItemDTO>();
        }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaOferta { get; set; }
        public tiposDirigidaOferta DirigidaOferta { get; set; }
        public tiposMetodoPago MetodoPago { get; set; }
        public List<OfertaItemDTO> Items { get; set; }
    }
}
