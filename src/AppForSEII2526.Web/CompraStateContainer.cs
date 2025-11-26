using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class CompraStateContainer
    {
        //we create an instance of Compra when an instance of CompraStateContainer is created.
        public CompraForCreateDTO Compra { get; private set; } = new CompraForCreateDTO()
        {
            CompraItems = new List<CompraItemDTO>()
        };

        //we compute the TotalPrice of the tools we have selected for buying them
        public double PrecioTotal
        {
            get
            {
                return (double)Compra.CompraItems.Sum(ci => ci.Precio * ci.Cantidad);
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddHerramientaToCompra(HerramientaParaComprarDTO herramienta)
        {
            //before adding a tool we checked whether it has been already added
            if (!Compra.CompraItems.Any(ri => ri.HerramientaId == herramienta.Id))
                //we add it if it is not in the list
                Compra.CompraItems.Add(new CompraItemDTO()
                {
                    HerramientaId = herramienta.Id,
                    Nombre = herramienta.Nombre,
                    Material = herramienta.Material,
                    Precio = herramienta.Precio,
                }
            );
        }

        //to delete tools from the list of selected tools
        public void RemoveCompraItemToCompra(CompraItemDTO item)
        {
            Compra.CompraItems.Remove(item);

        }

        //We eliminate all the tools from the list
        public void ClearCompraCart()
        {
            Compra.CompraItems.Clear();
        }

        //we have already finished the process of buying, thus, we create a new Buy
        public void CompraProcessed()
        {
            //we have finished the buy process so we create a new object without data
            Compra = new CompraForCreateDTO()
            {
                CompraItems = new List<CompraItemDTO>()
            };
        }

    }
}