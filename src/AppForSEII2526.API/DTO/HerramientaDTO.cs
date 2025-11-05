namespace AppForSEII2526.API.DTO
{
    public class HerramientaDTO
    {
        private Fabricante fabricante;

        [Key]
        public int id { get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]
        public int itemsReparacion { get; set; }  
        public string material { get; set; }
        public int compras {  get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]
        public string nombre { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int OfertaItems { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int precio { get; set; }
        public int tiempoReparacion { get; set; }

        public HerramientaDTO(int id, String nombre, String material, Fabricante fabricante, int precio, int itemsReparacion, int OfertaItems, int tiempoReparacion, int compras)
        {
            this.id = id;
            this.nombre = nombre;
            this.material = material;
            this.fabricante = fabricante;
            this.precio = precio;
            this.itemsReparacion = itemsReparacion;
            this.OfertaItems = OfertaItems;
            this.tiempoReparacion = tiempoReparacion;
            this.compras = compras;
        }
    }
}
