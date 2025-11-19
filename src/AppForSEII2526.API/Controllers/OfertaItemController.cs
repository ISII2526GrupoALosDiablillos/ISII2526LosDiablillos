using AppForSEII2526.API.DTO.Oferta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(OfertaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateOferta(OfertaForCreateDTO ofertaForCreate)
        {
            if (ofertaForCreate == null)
            {
                ModelState.AddModelError("Oferta", "Oferta no puede ser nula");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (ofertaForCreate.FechaInicio == default)
                ModelState.AddModelError("FechaInicio", "Error! Fecha Inicio es un campo obligatorio");

            if (ofertaForCreate.FechaFinal == default)
                ModelState.AddModelError("FechaFinal", "Error! Fecha Final es un campo obligatorio");

            if (ofertaForCreate.FechaInicio <= DateTime.Today)
                ModelState.AddModelError("FechaInicio", "Error! La fecha de inicio de tu oferta debe ser posterior a hoy");

            if (ofertaForCreate.FechaInicio >= ofertaForCreate.FechaFinal)
                ModelState.AddModelError("FechaInicio&FechaFinal", "Error! Tu oferta debe terminar después de que empiece");

            if (ofertaForCreate.OfertaItems == null || !ofertaForCreate.OfertaItems.Any())
                ModelState.AddModelError("OfertaItems", "Error! Tienes que incluir al menos una herramienta para aplicar una oferta");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var herramientaIds = ofertaForCreate.OfertaItems.Select(i => i.HerramientaId).Distinct().ToList();

            var herramientas = await _context.Herramientas
                .Include(h => h.fabricante)
                .Where(h => herramientaIds.Contains(h.id))
                .ToListAsync();

            var nuevaOferta = new Oferta(
                ofertaForCreate.FechaFinal,
                ofertaForCreate.FechaInicio,
                DateTime.Today,
                0,
                ofertaForCreate.MetodoPago,
                ofertaForCreate.DirigidaOferta,
                new List<OfertaItem>());

            foreach (var itemDto in ofertaForCreate.OfertaItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.id == itemDto.HerramientaId);
                if (herramienta == null)
                {
                    ModelState.AddModelError("OfertaItems", $"La herramienta con id {itemDto.HerramientaId} no fue encontrada");
                    continue;
                }

                if (itemDto.Porcentaje < 0 || itemDto.Porcentaje > 100)
                {
                    ModelState.AddModelError("Porcentaje", "Error: El porcentaje debe estar entre 0 y 100");
                    continue;
                }

                var ofertaItem = new OfertaItem(itemDto.HerramientaId, 0, itemDto.Porcentaje, itemDto.PrecioFinal);
                ofertaItem.herramienta = herramienta;
                ofertaItem.oferta = nuevaOferta;

                nuevaOferta.ofertaItems.Add(ofertaItem);
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            
            var user = await _context.Users.FirstOrDefaultAsync();
            nuevaOferta.applicationUser = user;

            _context.Ofertas.Add(nuevaOferta);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la nueva oferta");
                ModelState.AddModelError("Oferta", "Error! Ha habido un problema al guardar la nueva Oferta");
                return Conflict("Error: " + ex.Message);
            }


            var ofertaCreada = new OfertaDetailDTO(
                nuevaOferta.fechaInicio,
                nuevaOferta.fechaFinal,
                nuevaOferta.fechaOferta,
                nuevaOferta.dirigidaOferta,
                nuevaOferta.metodoPago,
                nuevaOferta.ofertaItems.Select(oi => new OfertaItemDTO(
                    oi.herramientaId,
                    oi.herramienta.nombre,
                    oi.herramienta.material,
                    oi.herramienta.fabricante.Nombre,
                    (decimal)oi.herramienta.precio,
                    oi.porcentaje,
                    oi.precioFinal
                )).ToList()
            );

            return CreatedAtAction(nameof(GetOfertaDetalleById), new { id = nuevaOferta.Id }, ofertaCreada);
        }
    }
}
