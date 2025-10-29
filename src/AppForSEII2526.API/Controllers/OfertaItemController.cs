using AppForSEII2526.API.DTO.Oferta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OfertaItemController> _logger;

        public OfertaItemController(ApplicationDbContext context, ILogger<OfertaItemController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("Oferta-Detalle")]
        [ProducesResponseType(typeof(IList<OfertaDetailDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOfertaDetalle()
        {
            if (_context.Ofertas == null)
            {
                _logger.LogError("Error: La tabla Ofertas no existe.");
                return NotFound("La tabla Ofertas no existe.");
            }

            var ofertaDetalles = await _context.Ofertas
                .AsNoTracking()
                .Include(o => o.ofertaItems)
                    .ThenInclude(oi => oi.herramienta)
                        .ThenInclude(h => h.fabricante)     
                .Select(o => new OfertaDetailDTO(
                    o.fechaInicio,
                    o.fechaFinal,
                    o.fechaOferta,
                    o.dirigidaOferta,
                    o.metodoPago,
                    o.ofertaItems.Select(oi => new OfertaItemDTO(
                        oi.herramientaId,
                        oi.herramienta.nombre,
                        oi.herramienta.material,
                        oi.herramienta.fabricante.Nombre,   
                        (decimal)oi.herramienta.precio,      
                        oi.porcentaje,
                        oi.precioFinal
                    )).ToList()
                ))
                .ToListAsync();

            return Ok(ofertaDetalles); 
        }

        
        [HttpGet]
        [Route("Oferta-Detalle/{id:int}")]
        [ProducesResponseType(typeof(OfertaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOfertaDetalleById(int id)
        {
            if (_context.Ofertas == null)
            {
                _logger.LogError("Error: La tabla Ofertas no existe.");
                return NotFound("La tabla Ofertas no existe.");
            }

            var dto = await _context.Ofertas
                .AsNoTracking()
                .Include(o => o.ofertaItems)
                    .ThenInclude(oi => oi.herramienta)
                        .ThenInclude(h => h.fabricante)
                .Where(o => o.Id == id)
                .Select(o => new OfertaDetailDTO(
                    o.fechaInicio,
                    o.fechaFinal,
                    o.fechaOferta,
                    o.dirigidaOferta,
                    o.metodoPago,
                    o.ofertaItems.Select(oi => new OfertaItemDTO(
                        oi.herramientaId,
                        oi.herramienta.nombre,
                        oi.herramienta.material,
                        oi.herramienta.fabricante.Nombre,
                        (decimal)oi.herramienta.precio,
                        oi.porcentaje,
                        oi.precioFinal
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (dto is null)
                return NotFound($"No se encontró ninguna oferta con ID {id}.");

            return Ok(dto);
        }
    }
}
