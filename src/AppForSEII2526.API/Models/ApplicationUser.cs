using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;
public class ApplicationUser : IdentityUser
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su apellido.")]
    public String apellidoCliente { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su correo electrónico.")]
    public String correoElectronico { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su nombre.")]
    public String nombreCliente { get; set; }
    public int telefono { get; set; }
    public ApplicationUser() { }
    public ApplicationUser(String apellidoCliente, String correoElectronico, String nombreCliente, int telefono)
    {
        this.apellidoCliente = apellidoCliente;
        this.correoElectronico = correoElectronico;
        this.nombreCliente = nombreCliente;
        this.telefono = telefono;
    }
    public IList<Compra> compras { get; set; }
    public IList<Reparacion> reparaciones { get; set; }
    public IList<Alquilar> alquilar { get; set; }
    public IList<Oferta> ofertas { get; set; }
}