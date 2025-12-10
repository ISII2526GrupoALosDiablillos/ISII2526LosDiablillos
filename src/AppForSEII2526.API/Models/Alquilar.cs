using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppForSEII2526.API.Models
{
    public enum PaymentMethodTypes
    {
        CreditCard,
        PayPal,
        Cash
    }
    public class Alquilar
    {
        [Key]
        public int Id { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Direccion de envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, pon tu direccion de envio")]
        public string direccionEnvio { get; set; }

        public DateTime fechaAlquiler { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime fechaInicio { get; set; }
        public int periodo { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public double precioTotal { get; set; }

        public string correo { get; set; }
        public int numeroTelefono { get; set; }
        public PaymentMethodTypes MetodoPago { get; set; }

        public ApplicationUser applicationUser { get; set; }
        public IList<AlquilarItem> alquilarItems { get; set; }


        public Alquilar()
        {
            alquilarItems = new List<AlquilarItem>();
        }

        public Alquilar(string nombreCliente, string apellidoCliente, string direccionEnvio, DateTime fechaAlquiler, PaymentMethodTypes metodoPago, DateTime fechaFin, DateTime fechaInicio, IList<AlquilarItem> alquilarItems, ApplicationUser applicationUser)
        {
            this.direccionEnvio = direccionEnvio;
            this.fechaAlquiler = fechaAlquiler;
            this.fechaFin = fechaFin;
            this.fechaInicio = fechaInicio;
            this.MetodoPago = metodoPago;
            this.applicationUser = applicationUser;
            this.alquilarItems = alquilarItems;

            this.periodo = (fechaFin - fechaInicio).Days;
            precioTotal = alquilarItems.Sum(ri => ri.precio * periodo);
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