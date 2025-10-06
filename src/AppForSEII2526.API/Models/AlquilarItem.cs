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

        public AlquilarItem()
        {

        }

        public AlquilarItem(int idAlquiler,Herramienta herramienta,Alquilar alquilar,int idHerramienta, int precio, int cantidad)
        {
            IdAlquiler = idAlquiler;
            Herramienta= herramienta;
            Alquilar = alquilar;
            IdHerramienta = idHerramienta;
            Precio = precio;
            Cantidad = cantidad;
        }
        public int IdAlquiler { get; set; }
        public Herramienta Herramienta { get; set; }
        public Alquilar Alquilar { get;set; }
        public int IdHerramienta { get; set; }
        public int Precio { get; set; }
        public int Cantidad { get; set; }

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
