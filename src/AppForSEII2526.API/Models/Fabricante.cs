using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class Fabricante
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public IList<Herramienta> herramientas { get; set; }

        public Fabricante() { }

        public Fabricante(int Id, string Nombre, IList<Herramienta> herramientas)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.herramientas = herramientas;
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
