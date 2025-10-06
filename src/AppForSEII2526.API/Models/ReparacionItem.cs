using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppForSEII2526.API.Models
{
    public class ReparacionItem
    {
        public int Id { get; set; }
        public int IdReparacion { get; set; }
        public int IdHerramienta { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="Porfavor, introduzca la cantidad.")]   
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int TiempoReparacion { get; set; }
        public ReparacionItem() { }

        public ReparacionItem(int id, int idReparacion, Herramienta herramienta, int cantidad, double precio, int tiempoReparacion)
        {
            Id = id;
            IdReparacion = idReparacion;
            Herramienta = herramienta;
            idHerramienta = herramienta.id;
            Cantidad = cantidad;
            Precio = precio;
            TiempoReparacion = tiempoReparacion;
            Descripcion = descripcion;
        }
        public int cantidad { get; set; }
        public string descripcion { get; set; }
        public int idHerramienta { get; set; }
        public int idReparacion { get; set; }
        public float precio { get; set; }
    
        public Herramienta Herramienta { get; set; }
        public Reparacion Reparacion { get; set; }


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
