using AppForSEII2526.API.DTO.ComprarDTOs;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

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
                .Where(o => o.id == id)
                .Include(o => o.compraItem)
                .ThenInclude(oi => oi.herramienta)
                .ThenInclude(h => h.fabricante)
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
            if (compraForCreateDTO == null)
                ModelState.AddModelError("CrearCompraDTO", "Pasa un objeto como parámetro.");

            if (compraForCreateDTO.CompraItems.Count == 0)
                ModelState.AddModelError("CompraItems", "Error: No se puede comprar cero herramientas...");

            if (compraForCreateDTO.Nombre_cliente == null)
                ModelState.AddModelError("NoNombreCliente", "Error. Introduzca su nombre.");

            if (compraForCreateDTO.Apellidos_cliente == null)
                ModelState.AddModelError("NoApellidosCliente", "Error. Introduzca su apellido.");

            if (compraForCreateDTO.DireccionEnvio == null)
                ModelState.AddModelError("NoDirecciónEnvío", "Error. Introduzca su dirección.");

            var ultimaCompra = compraForCreateDTO.CompraItems.FirstOrDefault();
            if (ultimaCompra != null && ultimaCompra.cantidad == 3 && string.IsNullOrEmpty(ultimaCompra.descripcion))
                ModelState.AddModelError("MuchasHerramientas", "¡Error! Estas comprando demasiadas herramientas sin descripción.");

            var pago = compraForCreateDTO.Pago;
            if (pago == PaymentMethodTypes.Cash)
                ModelState.AddModelError("Metalico", "¡Error! No aceptamos compras pagadas en metálico.");

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.nombreCliente == compraForCreateDTO.Nombre_cliente && au.apellidoCliente == compraForCreateDTO.Apellidos_cliente);
            if (user == null)
                return BadRequest(new ValidationProblemDetails(ModelState));

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var nombreHerramienta = compraForCreateDTO.CompraItems.Select(h => h.nombre).Distinct().ToList();

            var herramientas = await _context.Herramientas
                .Include(f => f.fabricante)
                .Where(h => nombreHerramienta.Contains(h.nombre))
                .ToListAsync();

            var comprar = new Compra
            {
                direccionEnvio = compraForCreateDTO.DireccionEnvio,
                fechaCompra = DateTime.Today,
                preciototal = 0.0,
                metodoPago = pago,
                compraItem = new List<CompraItem>(),
                atributos = user
            };

            foreach(var compraDTO in compraForCreateDTO.CompraItems)
            {
                if(compraDTO == null)
                {
                    ModelState.AddModelError("NoItems", "Error: Usted no puede comprar un objeto nulo, tiene que comprar una herramienta.");
                    continue;
                }

                if (compraDTO.cantidad == null)
                    ModelState.AddModelError("CompraNoVálida", "Error: Introduzca cuántas unidades quiere comprar.");
                
                if (compraDTO.cantidad <= 0)
                    ModelState.AddModelError("CompraNoVálida", "Error: La cantidad debe ser superior a cero.");

                if (compraDTO.descripcion == null)
                    ModelState.AddModelError("CompraNoVálida", "Error: Introduzca la descripción de la herramienta que quiere comprar.");

                var herramienta = herramientas.FirstOrDefault(h => h.nombre == compraDTO.nombre);

                if (herramienta == null)
                {
                    ModelState.AddModelError("NoHerramientas", "Error: La herramienta no está disponible.");
                    continue;
                }
                else
                {
                    double precioHerramienta = herramienta.precio;
                    int cantidadHerramienta = compraDTO.cantidad;

                    var herramientaParaComprar = new CompraItem
                    {
                        cantidad = cantidadHerramienta,
                        descripcion = compraDTO.descripcion,
                        compra = comprar,
                        herramienta = herramienta,
                        precio = precioHerramienta * cantidadHerramienta
                    };

                    comprar.compraItem.Add(herramientaParaComprar);
                }

            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            comprar.preciototal = comprar.compraItem.Sum(oi => oi.precio);

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
                comprar.atributos.nombreCliente,
                comprar.atributos.apellidoCliente,
                comprar.direccionEnvio,
                comprar.preciototal,
                comprar.fechaCompra,
                compraForCreateDTO.CompraItems
            );

            return CreatedAtAction(nameof(CreateCompra), new { Id = comprar.id }, comprarDetailDTO);
        }

    }
}