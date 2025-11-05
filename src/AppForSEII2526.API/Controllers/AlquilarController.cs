using AppForSEII2526.API.DTO.AlquilarDTOs;
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
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilarDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquilar(int alquilerId)
        {
            if (_context.Alquileres == null)
            {
                _logger.LogError("Error: Rentals table does not exist");
                return NotFound();
            }
            var alquiler = await _context.Alquileres
                .Where(r => r.id == alquilerId)
                    .Include(r => r.alquilarItems)
                        .ThenInclude(ri => ri.herramienta)
                            .ThenInclude(herramienta => herramienta.fabricante)
                .Select(r => new AlquilarDetailDTO(r.Id, r.FechaAlquiler, r.applicationUser.nombreCliente, r.applicationUser.apellidoCliente, r.DireccionEnvio, (PaymentMethodTypes)r.MetodoPago, r.FechaFin, r.FechaInicio, r.alquilarItems
                        .Select(ri => new AlquilarItemDTO(ri.herramienta.id, ri.herramienta.fabricante.Id, ri.herramienta.precio, ri.HerramientaId)).ToList<AlquilarItemDTO>()))
                .FirstOrDefaultAsync();

            if (alquiler == null)
            {
                _logger.LogError($"Error: Rental with id {alquilerId} not found");
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
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");

            if (alquilarForCreateDTO.FechaInicio >= alquilarForCreateDTO.FechaFin)
                ModelState.AddModelError("RentalDateFrom&RentalDateTo", "Error! Your rental must end later than it starts");

            if (alquilarForCreateDTO.AlquilarItem.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! You must include at least one movie to be rented");
            
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == alquilarForCreateDTO.NombreCliente);
            if (user == null)
                ModelState.AddModelError("AlquilarApplicationUser", "Error! Nombre de usuario no registrado");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var herramientasIds = alquilarForCreateDTO.AlquilarItem.Select(ri => ri.HerramientaId).ToList();

            var herramientas = _context.Herramientas.Include(h => h.alquilarItems)
                .ThenInclude(ri => ri.alquilar)
                .Where(h => herramientasIds.Contains(h.id))

            .Select(h => new
            {
                h.id,
                h.nombre,
                h.material,
                h.precio,
                //we count the number of rentalItems that are within the rental period
                NumberOfRentedItems = h.alquilarItems.Count(ri => ri.alquilar.FechaInicio <= alquilarForCreateDTO.FechaFin
                        && ri.alquilar.fechaFin >= alquilarForCreateDTO.FechaInicio)
            })
                .ToList();

            Alquilar alquilar = new Alquilar(alquilarForCreateDTO.NombreCliente, alquilarForCreateDTO.ApellidoCliente, alquilarForCreateDTO.DireccionEnvio, DateTime.Now, alquilarForCreateDTO.PaymentMethod, alquilarForCreateDTO.FechaFin, alquilarForCreateDTO.FechaInicio, new List<AlquilarItem>(), user);

            alquilar.PrecioTotal = 0;

            var numeroDiasAlquiler = (alquilarForCreateDTO.FechaFin - alquilarForCreateDTO.FechaInicio).Days;

            foreach (var item in alquilarForCreateDTO.AlquilarItem)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.id == item.HerramientaId);
                if (herramienta == null)
                {
                    ModelState.AddModelError("AlquilarItems", $"Error! La herramienta con el id {item.HerramientaId} no existe");
                }
                else
                {
                    //alquilar.alquilarItems.Add(new AlquilarItem(herramienta.id, alquilar, herramienta.precio, ));
                    item.Precio = herramienta.precio;
                }
                alquilar.PrecioTotal = alquilar.alquilarItems.Sum(ri => ri.precio * numeroDiasAlquiler);


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

                var alquilerDetailDTO = new AlquilarDetailDTO(alquilar.Id, alquilar.FechaAlquiler, alquilar.applicationUser.nombreCliente, alquilar.applicationUser.apellidoCliente, alquilar.DireccionEnvio, (PaymentMethodTypes)alquilar.MetodoPago, alquilar.FechaFin, alquilar.FechaInicio, alquilarForCreateDTO.AlquilarItem);
                return CreatedAtAction(nameof(GetAlquilar), new { alquilerId = alquilerDetailDTO.Id }, alquilerDetailDTO);

            
        }
    }
}
