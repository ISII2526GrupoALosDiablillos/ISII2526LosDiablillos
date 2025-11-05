namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class RepararDetailDTO : ReparacionForCreateDTO
    {
        public RepararDetailDTO(int id,string nombreCliente, string apellidoCliente, DateTime fechaRecogida,
            DateTime fechaEntrega, IList<RepararItemDTO> reparacionItems): base(
                   nombreCliente,
                   apellidoCliente,
                   fechaRecogida,
                   fechaEntrega,
                   reparacionItems
                  )
        {
            Id = id;
        }
        public int Id { get; set; }
        public double PrecioTotal
        {
            get
            {
                double total = 0;
                foreach (var item in ReparacionItems)
                {
                    total += item.Precio;
                }
                return total;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is RepararDetailDTO dTO &&
                   base.Equals(obj) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   Id == dTO.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id);
        }
    }
}