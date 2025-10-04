namespace AppForSEII2526.API.Models
{
    public class Herramienta
    {
        public int compraItems {  get; set; }
        [Key]
        public int id {  get; set; }
        public int itemsReparación {  get; set; }
        public string material { get; set; }
        public string nombre {  get; set; }
        public int OfertaItems {  get; set; }
        public int precio {  get; set; }
        public int tiempoReparacion {  get; set; }


    }
}
