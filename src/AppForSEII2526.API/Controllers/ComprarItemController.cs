using AppForSEII2526.API.DTO.ComprarDTOs;
using AppForSEII2526.API.DTOs;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComprasController> _logger;

        public ComprasController(ApplicationDbContext context, ILogger<ComprasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("action")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCompraDetails(int id)
        {
            if (_context.Compras == null)
            {
                _logger.LogError("Error: La tabla no existe.");
                return NotFound("La tabla Compras no existe.");
            }

            var compraDetalle = await _context.Compras
                .Include(o => o.compraItem)
                .ThenInclude(oi => oi.herramienta)
                .ThenInclude(h => h.fabricante)
                .Where(o => o.id == id)
                .Select(o => new CompraDetailDTO(
                    o.id,
                    o.atributos.nombreCliente,
                    o.atributos.apellidoCliente,
                    o.direccionEnvio,
                    o.preciototal,
                    o.fechaCompra,
                    o.compraItem.Select(oi => new CompraItemDTO(
                        oi.herramienta.nombre,
                        oi.herramienta.material,
                        oi.herramienta.precio,
                        oi.descripcion,
                        oi.cantidad,
                        oi.herramienta.id,
                        oi.compra.id)).ToList()
                    ))
                .FirstOrDefaultAsync();

            if (compraDetalle == null)
            {
                _logger.LogError("No se encontraron compras en la base de datos.");
                return NotFound("Compra no encontrada.");
            }

            return Ok(compraDetalle);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCompra(CompraForCreateDTO compraForCreateDTO)
        {
            if (string.IsNullOrEmpty(compraForCreateDTO.Nombre))
                ModelState.AddModelError("Nombre", "Error. Introduzca el nombre de la herramienta que desea.");

            if (string.IsNullOrEmpty(compraForCreateDTO.Material))
                ModelState.AddModelError("Material", "Error. Introduzca el material de la herramienta que desea.");

            if (compraForCreateDTO.Precio <= 0)
                ModelState.AddModelError("Precio", "Error. La compra no puede costar 0 euros sin códigos de descuento ni cheques de regalo.");

            if (compraForCreateDTO.FechaCompra <= DateTime.Now)
                ModelState.AddModelError("FechaCompra", "Error. Tu fecha de compra debe ser después del momento actual.");

            if (compraForCreateDTO.FechaCompra >= compraForCreateDTO.FechaRecibo)
                ModelState.AddModelError("FechaRecibo", "Error. Cómo vas a recibir algo antes de comprarlo?");

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == compraForCreateDTO.UserName);
            if (user == null)
                ModelState.AddModelError("NoUsername", "Error. Nombre de usuario no registrado.");

            var nombre = _context.ApplicationUsers.FirstOrDefault(au => au.nombreCliente == compraForCreateDTO.Nombre_cliente);
            if (nombre == null)
                ModelState.AddModelError("NoNombreCliente", "Error. Nombre no registrado.");

            var apellido = _context.ApplicationUsers.FirstOrDefault(au => au.apellidoCliente == compraForCreateDTO.Apellidos_cliente);
            if (apellido == null)
                ModelState.AddModelError("NoApellidoCliente", "Error. Apellido no registrado.");

            var ultimaCompra = compraForCreateDTO.CompraItems.FirstOrDefault();
            if (ultimaCompra != null && ultimaCompra.cantidad == 3 && string.IsNullOrEmpty(ultimaCompra.descripcion))
                ModelState.AddModelError("MuchasHerramientas", "¡Error! Estas comprando demasiadas herramientas sin descripción.");

            var direccion = _context.ApplicationUsers
                .FirstOrDefault(au => au.compras.Any(c => c.direccionEnvio == compraForCreateDTO.DireccionEnvio));
            if (direccion == null)
                ModelState.AddModelError("NoDirecciónDeEnvio", "Error. Dirección no registrada.");

            var pago = compraForCreateDTO.Pago;
            if (pago == PaymentMethodTypes.Cash)
                ModelState.AddModelError("Metalico", "¡Error! No aceptamos compras pagadas en metálico.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var herramientasIds = compraForCreateDTO.CompraItems.Select(ri => ri.herramientaId).ToList();

            var herramientas = _context.Herramientas
                .Include(h => h.compraItems)
                    .ThenInclude(ri => ri.compra)
                .Where(h => herramientasIds.Contains(h.id))
                .Select(h => new
                {
                    h.id,
                    h.nombre,
                    h.material,
                    h.precio,
                    NumberOfRentedItems = h.compraItems.Count(ri =>
                        ri.compra.fechaInicio <= compraForCreateDTO.FechaRecibo &&
                        ri.compra.fechaRecibo >= compraForCreateDTO.FechaCompra)
                })
                .ToList();

            var comprar = new Compra(
                compraForCreateDTO.DireccionEnvio,
                compraForCreateDTO.FechaCompra,
                compraForCreateDTO.Id,
                0,
                new List<CompraItem>(),
                user)
            {
                atributos = user,
                compraItem = new List<CompraItem>()
            };

            var numeroDiasAlquiler = (compraForCreateDTO.FechaRecibo - compraForCreateDTO.FechaCompra).Days;

            foreach (var item in compraForCreateDTO.CompraItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.id == item.herramientaId);
                if (herramienta == null)
                {
                    ModelState.AddModelError("ComprarItems", $"Error. La herramienta con el id {item.herramientaId} no existe");
                }
                else
                {
                    item.precio = herramienta.precio;
                }
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Add(comprar);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Conflict("Error. " + ex.Message);
            }

            var comprarDetailDTO = new CompraDetailDTO(
                comprar.id,
                user.nombreCliente,
                user.apellidoCliente,
                comprar.direccionEnvio,
                comprar.preciototal,
                comprar.fechaCompra,
                compraForCreateDTO.CompraItems
            );

            return CreatedAtAction(nameof(CreateCompra), new { Id = comprarDetailDTO.id }, comprarDetailDTO);
        }

    }
}