namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(HerramientaId),nameof(alquilerId))]
    public class AlquilarItem
    {
        public int alquilerId {  get; set; }
        [Required]
        public Herramienta herramienta { get; set; }
        [Required]
        public Alquilar alquilar { get; set; }
        public int HerramientaId{  set; get; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public int precio {  set; get; }
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int cantidad {  set; get; }

        public AlquilarItem()
        {

        }

        public AlquilarItem(int AlquilerId,Herramienta herramienta,Alquilar alquilar,int HerramientaId, int precio, int cantidad)
        {
            this.alquilerId = AlquilerId;
            this.herramienta = herramienta;
            this.alquilar = alquilar;
            this.HerramientaId = HerramientaId;
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
