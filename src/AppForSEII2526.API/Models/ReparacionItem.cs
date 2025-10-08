using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppForSEII2526.API.Models
{
    public class ReparacionItem
    {
        public int id { get; set; }
        public int idReparacion { get; set; }
        public int idHerramienta { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="Porfavor, introduzca la cantidad.")]   
        public int cantidad { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public int tiempoReparacion { get; set; }
        public ReparacionItem() { }

        public ReparacionItem(int id, int idReparacion, int idherramienta, int cantidad, double precio, int tiempoReparacion, string descripcion)
        {
            this.id = id;
            this.idReparacion = idReparacion;
            this.idHerramienta = idherramienta;
            this.cantidad = cantidad;
            this.precio = precio;
            this.tiempoReparacion = tiempoReparacion;
            this.descripcion = descripcion;
        }
        
    
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
