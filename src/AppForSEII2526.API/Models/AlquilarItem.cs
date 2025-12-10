namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(HerramientaId), nameof(AlquilarId))]
    public class AlquilarItem
    {
        public int AlquilarId { get; set; }
        public int HerramientaId { get; set; } 

        [Required]
        public Herramienta herramienta { get; set; }

        [Required]
        public Alquilar alquilar { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public int precio { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int cantidad { set; get; }

        public AlquilarItem() { }

        public AlquilarItem(int cantidad, int precio, Alquilar alquilar, Herramienta herramienta)
        {
            this.cantidad = cantidad;
            this.precio = precio;
            this.alquilar = alquilar;
            this.herramienta = herramienta;
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
