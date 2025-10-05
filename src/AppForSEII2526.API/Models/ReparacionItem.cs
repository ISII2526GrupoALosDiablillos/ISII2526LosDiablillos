namespace AppForSEII2526.API.Models
{
    public class ReparacionItem
    {
        public int cantidad { get; set; }
        public string descripcion { get; set; }
        public int idHerramienta { get; set; }
        public int idReparacion { get; set; }
        public float precio { get; set; }
    }
}
