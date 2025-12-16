using AppForSEII2526.API.DTO.AlquilarDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilarController> _logger;
        public AlquilarController(ApplicationDbContext context, ILogger<AlquilarController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("Alquiler initialized");
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilarDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquilar(int alquilerId)
        {
            if (_context.Alquileres == null)
            {
                _logger.LogError("Error: Las tablas de alquilar no existen");
                return NotFound();
            }
            var alquiler = await _context.Alquileres
                .Where(r => r.Id == alquilerId)
                    .Include(r => r.alquilarItems)
                        .ThenInclude(ri => ri.herramienta)
                .Select(r => new AlquilarDetailDTO(r.Id, r.fechaAlquiler, r.applicationUser.nombreCliente, r.applicationUser.apellidoCliente, r.direccionEnvio, (PaymentMethodTypes)r.MetodoPago, r.fechaFin, r.fechaInicio, r.alquilarItems
                        .Select(ri => new AlquilarItemDTO(ri.herramienta.id,ri.herramienta.nombre, ri.herramienta.material, ri.cantidad,ri.herramienta.precio)).ToList<AlquilarItemDTO>()))
                .FirstOrDefaultAsync();

            if (alquiler == null)
            {
                _logger.LogError($"Error: Alquiler con id {alquilerId} no encontrado");
                return NotFound();
            }

            return Ok(alquiler);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilarDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateAlquiler(AlquilarForCreateDTO alquilarForCreateDTO)
        {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            if (alquilarForCreateDTO.FechaInicio <= DateTime.Today)
                ModelState.AddModelError("AlquilerDateFrom", "Error! Tu fecha de alquiler debe ser mas tarde que hoy");

            if (alquilarForCreateDTO.FechaInicio >= alquilarForCreateDTO.FechaFin)
                ModelState.AddModelError("RentalDateFrom&RentalDateTo", "Error! Tu alquiler debe acabar antes de iniciar");

            if (alquilarForCreateDTO.AlquilarItem.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! Tu deberias incluir una herramienta para alquilar");
            
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == alquilarForCreateDTO.NombreCliente);
            if (user == null)
                ModelState.AddModelError("AlquilarApplicationUser", "Error! Nombre de usuario no registrado");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var herramientaNombre = alquilarForCreateDTO.AlquilarItem.Select(a => a.NombreHerramienta).ToList();

            var herramientas = _context.Herramientas.Include(h => h.alquilarItems)
                .ThenInclude(ri => ri.alquilar)
                .Where(h => herramientaNombre.Contains(h.nombre))
                .ToList();

            var alquilar = new Alquilar(alquilarForCreateDTO.NombreCliente, alquilarForCreateDTO.ApellidoCliente, alquilarForCreateDTO.DireccionEnvio, DateTime.Now, alquilarForCreateDTO.PaymentMethod, alquilarForCreateDTO.FechaFin, alquilarForCreateDTO.FechaInicio, new List<AlquilarItem>(), user);

            alquilar.precioTotal = 0;

            var numeroDiasAlquiler = (alquilarForCreateDTO.FechaFin - alquilarForCreateDTO.FechaInicio).Days;

            foreach (var alquileritem in alquilarForCreateDTO.AlquilarItem)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.nombre == alquileritem.NombreHerramienta);
                if (herramienta == null)
                {
                    ModelState.AddModelError("AlquilarItems", $"Error! La herramienta con el nombre {alquileritem.NombreHerramienta} no existe");
                    continue;
                }
                if (alquileritem.Cantidad == 0)
                {
                    ModelState.AddModelError("Cantidad", "Error! La cantidad no puede ser 0");
                }
                else
                {
                    alquileritem.Precio = herramienta.precio;
                    alquilar.alquilarItems.Add(new AlquilarItem(alquileritem.Cantidad,herramienta.precio,alquilar,herramienta));
                }
                alquilar.precioTotal = alquilar.alquilarItems.Sum(ri => ri.precio * numeroDiasAlquiler);


            }

                if (ModelState.ErrorCount > 0)
                {
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                _context.Add(alquilar);

                try
                {
                    //we store in the database both rental and its rentalitems
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    ModelState.AddModelError("Alquilar", $"Error! Hubo un error mientras se guardaba tu alquiler, por favor, intentalo de nuevo mas tarde");
                    return Conflict("Error" + ex.Message);

                }

                var alquilerDetailDTO = new AlquilarDetailDTO(alquilar.Id, alquilar.fechaAlquiler, alquilar.applicationUser.nombreCliente, alquilar.applicationUser.apellidoCliente, alquilar.direccionEnvio, (PaymentMethodTypes)alquilar.MetodoPago, alquilar.fechaFin, alquilar.fechaInicio, alquilarForCreateDTO.AlquilarItem);
                return CreatedAtAction(nameof(GetAlquilar), new { alquilerId = alquilerDetailDTO.Id }, alquilerDetailDTO);

            
        }
    }
}
