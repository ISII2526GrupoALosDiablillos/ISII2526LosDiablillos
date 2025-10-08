namespace AppForSEII2526.API.Models
{
    public class Herramienta
    {
        
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

        public Fabricante fabricante { get; set; }
        public IList<AlquilarItem> alquilarItems { get; set; }
        public IList<OfertaItem> ofertaItems { get; set; }
        public IList<CompraItem> compraItems { get; set; }
        public IList<ReparacionItem> reparacionItems { get; set; }
        public Herramienta() { }
        public Herramienta(int id,int itemsReparación,string material,string nombre,int precio,int tiempoReparacion, Fabricante fabricante)
        {
            
            this.id = id;
            this.itemsReparacion = itemsReparacion;
            this.material = material;
            this.nombre = nombre;
            this.precio = precio;
            this.tiempoReparacion = tiempoReparacion;

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
