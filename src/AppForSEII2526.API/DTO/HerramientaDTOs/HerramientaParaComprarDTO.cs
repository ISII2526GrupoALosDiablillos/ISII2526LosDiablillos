namespace AppForSEII2526.API.DTO.HerramientaDTOs
{
    public class HerramientaParaComprarDTO
    {
        private Fabricante fabricante;

        [Key]
        public int id { get; set; }
        public string material { get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]

        public string nombre { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int precio { get; set; }

        public HerramientaParaComprarDTO() { }

        public HerramientaParaComprarDTO(int id, string nombre, string material, String fabricante, int precio)
        {
            this.id = id;
            this.nombre = nombre;
            this.material = material;
            this.fabricante.Nombre = fabricante;
            this.precio = precio;
        }
    }
}