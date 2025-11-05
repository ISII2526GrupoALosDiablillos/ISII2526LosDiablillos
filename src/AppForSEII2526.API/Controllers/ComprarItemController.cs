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
    }



}