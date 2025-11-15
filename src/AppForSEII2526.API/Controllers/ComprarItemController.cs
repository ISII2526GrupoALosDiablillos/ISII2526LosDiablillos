using AppForSEII2526.API.DTO.AlquilarDTOs;
using AppForSEII2526.API.DTO.ComprarDTOs;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
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
        [ProducesResponseType(typeof(IList<CompraDetailDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCompraDetails()
        {
            if (_context.Compras == null)
            {
                _logger.LogError("Error: La tabla no existe.");
                return NotFound();
            }

            IList<CompraDetailDTO> compraDetalles = await _context.Compras
                .Include(o => o.atributos)
                .Include(o => o.compraItem)
                    .ThenInclude(oi => oi.herramienta)
                    .ThenInclude(h => h.fabricante)
                .OrderBy(o => o.id)
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
                 .ToListAsync();

            if (compraDetalles == null)
            {
                _logger.LogError("No se encontraron compras en la base de datos.");
                return NotFound();
            }

            return Ok(compraDetalles);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCompra(CompraForCreateDTO compraForCreateDTO)
        {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            if (compraForCreateDTO.CompraItems.Count == 0)
                ModelState.AddModelError("ComprarNingúnItem", "Error! No quedan items en ");

            if (compraForCreateDTO.FechaCompra <= DateTime.Now)
                ModelState.AddModelError("ComprarAntesDeTiempo", "Error! Tu fecha de compra debe ser después de este momento actual.");

            if (compraForCreateDTO.FechaCompra >= compraForCreateDTO.FechaRecibo)
                ModelState.AddModelError("RecibirAntesDeComprar", "Error! Cómo vas a recibir algo antes de comprarlo?");

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == compraForCreateDTO.Nombre_cliente);
            if (user == null)
                ModelState.AddModelError("NoRegistrado", "Error! Nombre de usuario no registrado.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var herramientasIds = compraForCreateDTO.CompraItems.Select(ri => ri.herramientaId).ToList();

            var herramientas = _context.Herramientas.Include(h => h.compraItems)
                .ThenInclude(ri => ri.compra)
                .Where(h => herramientasIds.Contains(h.id))

            .Select(h => new
            {
                h.id,
                h.nombre,
                h.material,
                h.precio,
                //we count the number of rentalItems that are within the rental period
                NumberOfRentedItems = h.compraItems.Count(ri => ri.compra.fechaInicio <= compraForCreateDTO.FechaRecibo
                        && ri.compra.fechaRecibo >= compraForCreateDTO.FechaCompra)
            })
                .ToList();

            Compra comprar = new Compra(compraForCreateDTO.DireccionEnvio, DateTime.Now, compraForCreateDTO.Id, compraForCreateDTO.Precio, new List<CompraItem>(), user);

            comprar.preciototal = 0;

            var numeroDiasAlquiler = (compraForCreateDTO.FechaRecibo - compraForCreateDTO.FechaCompra).Days;

            foreach (var item in compraForCreateDTO.CompraItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.id == item.herramientaId);
                if (herramienta == null)
                {
                    ModelState.AddModelError("ComprarItems", $"Error! La herramienta con el nombre {item.herramientaId} no existe");
                }
                else
                {
                    item.precio = herramienta.precio;
                }
                comprar.preciototal = comprar.compraItem.Sum(ri => ri.precio * numeroDiasAlquiler);


            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(comprar);

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Compra", $"Error! Hubo un error mientras se guardaba tu compra, por favor, intentalo de nuevo mas tarde");
                return Conflict("Error" + ex.Message);

            }

            var comprarDetailDTO = new CompraDetailDTO(comprar.id, comprar.atributos.nombreCliente, comprar.atributos.apellidoCliente, comprar.direccionEnvio, comprar.preciototal, comprar.fechaCompra, compraForCreateDTO.CompraItems);
            return CreatedAtAction(nameof(CreateCompra), new { Id = comprarDetailDTO.id }, comprarDetailDTO);
        }
    }



}