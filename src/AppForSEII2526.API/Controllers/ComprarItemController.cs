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
                        oi.herramienta.id)).ToList()
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
            if (string.IsNullOrEmpty(compraForCreateDTO.Nombre_cliente))
                ModelState.AddModelError("Nombre_cliente", "Error. Por favor, introduzca su nombre.");
            if (string.IsNullOrEmpty(compraForCreateDTO.Apellidos_cliente))
                ModelState.AddModelError("Apellidos_cliente", "Error. Por favor, introduzca sus apellidos.");
            if (string.IsNullOrEmpty(compraForCreateDTO.DireccionEnvio))
                ModelState.AddModelError("DireccionEnvio", "Error. Por favor, introduzca su dirección para recibir su compra.");
            if (compraForCreateDTO.FechaCompra <= DateTime.Now)
                ModelState.AddModelError("FechaCompra", "Error. Tu fecha de compra debe ser después del momento actual.");
            if (compraForCreateDTO.FechaCompra >= compraForCreateDTO.FechaRecibo)
                ModelState.AddModelError("FechaRecibo", "Error. Cómo vas a recibir algo antes de comprarlo?");

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == compraForCreateDTO.Nombre_cliente);
            if (user == null)
                ModelState.AddModelError("NoRegistrado", "Error. Nombre de usuario no registrado.");

            if (ModelState.ErrorCount > 0)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    _logger.LogError($"ModelState Error: {error.ErrorMessage}");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var herramientasIds = (compraForCreateDTO.CompraItems ?? new List<CompraItemDTO>())
                .Select(ci => ci.herramientaId)
                .Distinct()
                .ToList();

            var herramientas = await _context.Herramientas
                .Include(h => h.compraItems)
                .Where(h => herramientasIds.Contains(h.id))
                .ToListAsync();

            var comprar = new Compra(
                compraForCreateDTO.DireccionEnvio,
                DateTime.Now,
                id: 0,
                preciototal: 0,
                compraItem: new List<CompraItem>(),
                atributos: user
            );

            var compraItems = compraForCreateDTO.CompraItems;

            foreach (var itemDto in compraItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.id == itemDto.herramientaId);
                if (herramienta == null)
                {
                    ModelState.AddModelError("ComprarItems", $"Error. La herramienta con el id {itemDto.herramientaId} no existe");
                    continue;
                }

                var precioUnitario = herramienta.precio;

                var compraItem = new CompraItem(itemDto.cantidad, itemDto.descripcion, 4, herramienta.id, precioUnitario);

                comprar.compraItem.Add(compraItem);
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var numeroDiasAlquiler = (compraForCreateDTO.FechaRecibo - compraForCreateDTO.FechaCompra).Days;
            comprar.preciototal = comprar.compraItem.Sum(ci => ci.precio * numeroDiasAlquiler);

            _context.Add(comprar);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Conflict("Error. " + ex.Message);
            }

            var comprarDetailDTO = new CompraDetailDTO(comprar.id, compraForCreateDTO.Nombre_cliente, compraForCreateDTO.Apellidos_cliente, comprar.direccionEnvio, comprar.preciototal, comprar.fechaCompra, compraForCreateDTO.CompraItems);

            return CreatedAtAction(nameof(CreateCompra), new { Id = comprarDetailDTO.id }, comprarDetailDTO);
        }

    }
}