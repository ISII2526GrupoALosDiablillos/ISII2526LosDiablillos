namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(HerramientaId),nameof(AlquilerId))]
    public class AlquilarItem
    {
        public int AlquilerId {  get; set; }
        public Herramienta herramienta { get; set; }
        public Alquilar alquilar { get; set; }
        public int HerramientaId{  set; get; }
        public int precio {  set; get; }
        public int cantidad {  set; get; }

        public AlquilarItem()
        {

        }

        public AlquilarItem(int AlquilerId,Herramienta herramienta,Alquilar alquilar,int HerramientaId, int precio, int cantidad)
        {
            this.AlquilerId = AlquilerId;
            this.herramienta = herramienta;
            this.alquilar = alquilar;
            this.AlquilerId = AlquilerId;
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
