using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class OfertaStateContainer
    {
        public OfertaForCreateDTO Oferta { get; private set; } = new OfertaForCreateDTO()
        {
            OfertaItems = new List<OfertaItemForCreateDTO>()
        };

        public float PrecioTotal
        {
            get
            {
                return (float)Oferta.OfertaItems.Sum(oi => oi.PrecioFinal);
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddHerramientaToOferta(HerramientaParaOfertarDTO herramienta, double porcentaje)
        {
            if (!Oferta.OfertaItems.Any(oi => oi.HerramientaId == herramienta.Id))
                Oferta.OfertaItems.Add(new OfertaItemForCreateDTO()
                {
                    HerramientaId = herramienta.Id,
                    Porcentaje = porcentaje,
                    PrecioFinal = herramienta.Precio - (herramienta.Precio * (porcentaje / 100.0))
                }
            );
        }

        public void RemoveOfertaItemToOferta(OfertaItemForCreateDTO item)
        {
            Oferta.OfertaItems.Remove(item);
        }

        public void ClearOfertaCart()
        {
            Oferta.OfertaItems.Clear();
        }

        public void OfertaProcessed()
        {
            Oferta = new OfertaForCreateDTO()
            {
                OfertaItems = new List<OfertaItemForCreateDTO>()
            };
        }
    }
}
