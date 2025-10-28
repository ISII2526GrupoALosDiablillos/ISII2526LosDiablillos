namespace AppForSEII2526.API.DTO.AlquilarDTOs
{
    public class AlquilarItemDTO
    {
        public AlquilarItemDTO(int alquilarId, int cantidad, double precio, int herramientaId)
        {
            AlquilarId = alquilarId;
            Cantidad = cantidad;
            Precio = precio;
            HerramientaId = herramientaId;
        }
        public int AlquilarId { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public int HerramientaId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilarItemDTO dTO &&
                   AlquilarId == dTO.AlquilarId &&
                   Cantidad == dTO.Cantidad &&
                   Precio == dTO.Precio &&
                   HerramientaId == dTO.HerramientaId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AlquilarId, Cantidad, Precio, HerramientaId);
        }
    }
}
