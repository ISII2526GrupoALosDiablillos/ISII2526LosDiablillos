namespace AppForSEII2526.API.Models
{
    public enum MetodosPago
    {
        TarjetaCredito,
        PayPal,
        Efectivo
    }
    public class Reparacion
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduzca su Nombre.")]  
        public string NombreCliente { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduzca su Apellido.")]
        public string ApellidoCliente { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduzca la Fecha de Entrega.")]
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaRecogida { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduzca el Metodo de Pago que desee.")]
        public MetodosPago MetodosPago { get; set; }
        public string Telefono { get; set; }
        public double PrecioTotal { get; set; }

        public List<ReparacionItem> ReparacionItems { get; set; }

        public Reparacion()
        {
            ReparacionItems = new List<ReparacionItem>();
        }

        public Reparacion(int id, string nombreCliente, string apellidoCliente, DateTime fechaEntrega, DateTime fechaRecogida, MetodosPago metodosPago, string telefono, double precioTotal)
        {
            Id = id;
            NombreCliente = nombreCliente;
            ApellidoCliente = apellidoCliente;
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            MetodosPago = metodosPago;
            Telefono = telefono;
            PrecioTotal = precioTotal;
            ReparacionItems = new List<ReparacionItem>();
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