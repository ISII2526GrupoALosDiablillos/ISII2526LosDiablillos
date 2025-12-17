using System;

namespace AppForSEII2526.API.DTOs
{
    public class CompraItemDTO
    {
        // CAMBIOS AQUÍ: Primera letra en MAYÚSCULA
        public string Nombre { get; set; }
        public string Material { get; set; }
        public double Precio { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int HerramientaId { get; set; }
        public int CompraId { get; set; }

        // Constructor actualizado
        public CompraItemDTO(string nombre, string material, double precio, string descripcion, int cantidad, int herramientaId, int compraId)
        {
            this.Nombre = nombre;
            this.Material = material;
            this.Precio = precio;
            this.Descripcion = descripcion;
            this.Cantidad = cantidad;
            this.HerramientaId = herramientaId;
            this.CompraId = compraId;
        }

        public CompraItemDTO() { } // Constructor vacío recomendado para serialización

        public override bool Equals(object? obj)
        {
            return obj is CompraItemDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Precio == dTO.Precio &&
                   Descripcion == dTO.Descripcion &&
                   Cantidad == dTO.Cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Material, Precio, Descripcion, Cantidad);
        }
    }
}