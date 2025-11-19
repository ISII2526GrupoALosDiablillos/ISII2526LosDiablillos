using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII2526.API.Models;

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

        public override bool Equals(object? obj)
        {
            return obj is OfertaDetailDTO dTO &&
                   FechaInicio == dTO.FechaInicio &&
                   FechaFinal == dTO.FechaFinal &&
                   FechaOferta == dTO.FechaOferta &&
                   DirigidaOferta == dTO.DirigidaOferta &&
                   MetodoPago == dTO.MetodoPago &&
                   (Items == null && dTO.Items == null ||
                    Items != null && dTO.Items != null && Items.SequenceEqual(dTO.Items));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaInicio, FechaFinal, FechaOferta, DirigidaOferta, MetodoPago, Items);
        }
    }
}