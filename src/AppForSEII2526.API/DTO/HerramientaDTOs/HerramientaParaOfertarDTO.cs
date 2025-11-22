using System;

namespace AppForSEII2526.API.DTO.HerramientaDTOs
{
    public class HerramientaParaOfertarDTO
    {
        public int id { get; set; }
        public int itemsReparacion { get; set; }
        public string material { get; set; }
        public string nombre { get; set; }
        public int OfertaItems { get; set; }
        public double precio { get; set; }
        public int tiempoReparacion { get; set; }
        public string fabricante { get; set; }
        public HerramientaParaOfertarDTO(int id, string nombre, string material, string fabricante, double precio)
        {
            this.id = id;
            this.nombre = nombre;
            this.material = material;
            this.fabricante = fabricante;
            this.precio = precio;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not HerramientaParaOfertarDTO other) return false;
            return id == other.id
                && string.Equals(nombre, other.nombre, StringComparison.Ordinal)
                && string.Equals(material, other.material, StringComparison.Ordinal)
                && string.Equals(fabricante, other.fabricante, StringComparison.Ordinal)
                && Math.Abs(precio - other.precio) < 0.001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, nombre ?? string.Empty, material ?? string.Empty, fabricante ?? string.Empty, precio);
        }
    }
}
