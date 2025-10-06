namespace AppForSEII2526.API.Models
{
    
    public class AlquilarItem
    {
        [Key]
        public int idAlquiler {  get; set; }
        [Required]
        public Herramienta herramienta { get; set; }
        [Required]
        public Alquilar alquilar { get; set; }
        [Required]
        public int idHerramienta {  set; get; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public int precio {  set; get; }
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int cantidad {  set; get; }

        public AlquilarItem()
        {

        }

        public AlquilarItem(int idAlquiler,Herramienta herramienta,Alquilar alquilar,int idHerramienta, int precio, int cantidad)
        {
            this.idAlquiler = idAlquiler;
            this.herramienta= herramienta;
            this.alquilar = alquilar;
            this.idHerramienta = idHerramienta;
            this.precio = precio;
            this.cantidad = cantidad;
        }
        

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
