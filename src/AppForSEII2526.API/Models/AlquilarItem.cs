namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(idAlquiler),nameof(idHerramienta))]
    public class AlquilarItem
    {
        public int idAlquiler {  get; set; }
        public Herramienta herramienta { get; set; }
        public Alquilar alquilar { get; set; }
        public int idHerramienta {  set; get; }
        public int precio {  set; get; }
        public int cantidad {  set; get; }


    }
}
