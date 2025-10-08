using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su apellido.")]
    public String apellidoCliente { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su correo electrónico.")]
    public String correoElectronico { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su nombre.")]
    public String nombreCliente { get; set; }
    public int telefono { get; set; }
    public ApplicationUser(String apellidoCliente, String correoElectronico, String nombreCliente, int telefono, IList<Compra> compras)
    {
        this.apellidoCliente = apellidoCliente;
        this.correoElectronico = correoElectronico;
        this.nombreCliente = nombreCliente;
        this.telefono = telefono;
    }
    public IList<Compra> compras { get; set; }
}