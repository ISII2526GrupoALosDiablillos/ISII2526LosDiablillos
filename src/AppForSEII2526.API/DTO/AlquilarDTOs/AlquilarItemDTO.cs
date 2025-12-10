namespace AppForSEII2526.API.DTO.AlquilarDTOs
{
    public class AlquilarItemDTO
    {
        public AlquilarItemDTO(int herramientaId, string nombreHerramienta, string material, int cantidad, double precio)
        {
            HerramientaId = herramientaId;
            NombreHerramienta = nombreHerramienta;
            Material = material;
            Cantidad = cantidad;
            Precio = precio;
        }
        public int AlquilarId { get; set; }
        public string NombreHerramienta { get; set; }
        public string Material { get; set; }
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
