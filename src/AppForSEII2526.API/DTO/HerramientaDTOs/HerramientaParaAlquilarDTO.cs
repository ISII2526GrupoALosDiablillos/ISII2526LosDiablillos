namespace AppForSEII2526.API.DTO.HerramientaDTOs
{
    public class HerramientaParaAlquilarDTO
    {
        public HerramientaParaAlquilarDTO() { }
        public HerramientaParaAlquilarDTO(int id, string nombre, string material, string fabricante, double precio)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Fabricante = fabricante;
            Precio = precio;
        }
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "El nombre tiene que tener una longitud maxima de 50 caracteres")]
        public string Nombre { get; set; }
        [StringLength(50, ErrorMessage = "La herramienta debe tener una longitud maxima de 50 caracteres", MinimumLength = 4)]
        public string Material { get; set; }
        public string Fabricante { get; set; }
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "El precio minimo es 1 ")]
        [Display(Name = "Precio por alquilar")]
        public double Precio { get; set; }
 

        public override bool Equals(object? obj)
        {
            return obj is HerramientaParaAlquilarDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Fabricante == dTO.Fabricante &&
                   Precio == dTO.Precio;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Material, Fabricante, Precio);
        }

    }
}
