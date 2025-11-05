namespace AppForSEII2526.API.DTO.HerramientaDTOs
{
    public class HerramientaParaRepararDTO
    {
        public  HerramientaParaRepararDTO()
        {
        }

        public HerramientaParaRepararDTO(int id, string nombre, string material, string fabricante, double precio, int tiemporeparacion)
        {
            this.id = id;
            this.nombre = nombre;
            this.material = material;
            this.tiempoReparacion = tiemporeparacion;
            this.precio = (int)precio;
        }


        [Key]
        public int id { get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]
        public int itemsReparacion { get; set; }
        public string material { get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]

        public string nombre { get; set; }  
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int OfertaItems { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int precio { get; set; }
        public int tiempoReparacion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is HerramientaParaRepararDTO dTO &&
                   id == dTO.id &&
                   itemsReparacion == dTO.itemsReparacion &&
                   material == dTO.material &&
                   nombre == dTO.nombre &&
                   OfertaItems == dTO.OfertaItems &&
                   precio == dTO.precio &&
                   tiempoReparacion == dTO.tiempoReparacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, itemsReparacion, material, nombre, OfertaItems, precio, tiempoReparacion);
        }
    }
}
