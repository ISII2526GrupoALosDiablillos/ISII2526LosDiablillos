using AppForSEII2526.API.DTO.HerramientaDTOs;

namespace AppForSEII2526.API.DTO.CompraDTOs
{
    public class CompraDetailDTO
    {
        public CompraDetailDTO(String nombre_cliente, String apellido_cliente, String direccion, double precio_total, DateTime fecha_compra, String nombre_herramienta, String material, double precio, String descripcion, int cantidad)
        {
            this.nombre_cliente = nombre_cliente;
            this.apellido_cliente = apellido_cliente;
            this.direccion = direccion;
            this.precio_total = precio_total;
            this.fecha_compra = fecha_compra;
            this.nombre_herramienta = nombre_herramienta;
            this.material = material;
            this.precio = precio;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
        }
        private String nombre_cliente {  get; set; }
        private String apellido_cliente { get; set; }
        private String direccion {  get; set; }
        private double precio_total { get; set; }
        private DateTime fecha_compra {  get; set; }
        private String nombre_herramienta { get; set; }
        private String material {  get; set; }
        private double precio { get; set; }
        private String descripcion { get; set; }
        private int cantidad { get; set; }
    }
}
