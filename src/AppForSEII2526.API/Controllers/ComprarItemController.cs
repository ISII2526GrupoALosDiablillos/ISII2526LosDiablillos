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
        private readonly ILogger<HerramientaController> _logger;
        public ComprasController(ApplicationDbContext context, ILogger<HerramientaController> logger)
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
                        oi.cantidad)).ToList()
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
                ModelState.AddModelError("CompraItems", "Error! Debes incluir un item en la compra");

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == compraForCreateDTO.Nombre);
            if (user == null)
                ModelState.AddModelError("ComprarApplicationUser", "Error! Nombre de usuario no registrado");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var herramientasIds = compraForCreateDTO.compraObjetos.Select(ri => ri.herramientaId).ToList();

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

            Compra comprar = new Compra(compraForCreateDTO.DireccionEnvio, DateTime.Now, compraForCreateDTO.Id, compraForCreateDTO.Precio, compraForCreateDTO.compraObjetos, user);

            comprar.preciototal = 0;

            var numeroDiasAlquiler = (compraForCreateDTO.FechaRecibo - compraForCreateDTO.FechaCompra).Days;

            foreach (var item in compraForCreateDTO.compraObjetos)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.id == item.herramientaId);
                if (herramienta == null)
                {
                    ModelState.AddModelError("ComprarItems", $"Error! La herramienta con el id {item.herramientaId} no existe");
                }
                else
                {
                    //alquilar.alquilarItems.Add(new AlquilarItem(herramienta.id, alquilar, herramienta.precio, ));
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
                ModelState.AddModelError("Alquilar", $"Error! Hubo un error mientras se guardaba tu alquiler, por favor, intentalo de nuevo mas tarde");
                return Conflict("Error" + ex.Message);

            }

            //Faltan un par de cosas por implementar, si da error no pasa nada, luego lo cambio
            return Ok();
        }
    }



}