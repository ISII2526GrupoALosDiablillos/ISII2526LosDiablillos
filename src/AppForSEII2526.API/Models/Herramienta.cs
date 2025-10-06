namespace AppForSEII2526.API.Models
{
    public class Herramienta
    {
        public int compraItems {  get; set; }
        [Key]
        public int id {  get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]
        public int itemsReparacion {  get; set; }
        public string material { get; set; }
        [StringLength(100, ErrorMessage ="El nombre no puede tener mas de 100 caracteres")]

        public string nombre {  get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int OfertaItems {  get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5,float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int precio {  get; set; }
        public int tiempoReparacion {  get; set; }

        public Fabricante Fabricante { get; set; }
        public IList<AlquilarItem> alquilarItems { get; set; }
        public IList<OfertaItem> ofertaItems { get; set; }
        public Herramienta() { }
        public Herramienta(int compraItems,int id,int itemsReparación,string material,string nombre,int precio,int tiempoReparacion)
        {
            CompraItem=compraItems;
            Id = id;
            ItemsReparacion = itemsReparacion;
            Material = material;
            Nombre = nombre;
            Precio = precio;
            TiempoReparacion = tiempoReparacion;

        }
        public int Id {  get; set; }
        public int CompraItem { get; set; }
        public int ItemsReparacion { get;set; }
        public string Material { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public int TiempoReparacion { get; set; }
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
