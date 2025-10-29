<<<<<<<< HEAD:src/AppForSEII2526.API/DTO/HerramientasDTOs/HerramientaDTO.cs
﻿namespace AppForSEII2526.API.DTO.HerramientasDTOs
========
﻿namespace AppForSEII2526.API.DTO.HerramientaDTOs
>>>>>>>> origin/development:src/AppForSEII2526.API/DTO/HerramientaDTOs/HerramientaParaComprarDTO.cs
{
    public class HerramientaParaComprarDTO
    {
        private Fabricante fabricante;

        [Key]
        public int id { get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]
        public int itemsReparacion { get; set; }  
        public string material { get; set; }
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]

        public string nombre { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int OfertaItems { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Precio mínimo es 0.5")]
        public int precio { get; set; }
        public int tiempoReparacion { get; set; }

<<<<<<<< HEAD:src/AppForSEII2526.API/DTO/HerramientasDTOs/HerramientaDTO.cs
        public HerramientaDTO(int id, string nombre, string material, Fabricante fabricante, int precio)
========
        public HerramientaParaComprarDTO(int id, string nombre, string material, Fabricante fabricante, int precio)
>>>>>>>> origin/development:src/AppForSEII2526.API/DTO/HerramientaDTOs/HerramientaParaComprarDTO.cs
        {
            this.id = id;
            this.nombre = nombre;
            this.material = material;
            this.fabricante = fabricante;
            this.precio = precio;
        }
    }
}