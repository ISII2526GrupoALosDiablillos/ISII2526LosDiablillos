using AppForSEII2526.API.DTO.RepararDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReparacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReparacionesController> _logger;

        public ReparacionesController(ApplicationDbContext context, ILogger<ReparacionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RepararDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReparacionDetail(int id)
        {
            if (_context.Reparaciones == null)
            {
                _logger.LogError("Error: Reparaciones table does not exist");
                return NotFound();
            }

            var reparacion = await _context.Reparaciones
             .Where(r => r.Id == id)
                 .Include(r => r.ReparacionItems)
                    .ThenInclude(ri => ri.Herramienta)
                        .ThenInclude(herramienta => herramienta.fabricante)
             .Select(r => new RepararDetailDTO(r.Id, r.NombreCliente, r.ApellidoCliente,
                    r.FechaRecogida, r.FechaEntrega,
                    r.ReparacionItems
                        .Select(ri => new RepararItemDTO(ri.Herramienta.id,
                                ri.Herramienta.nombre, ri.Herramienta.precio,
                                ri.cantidad, ri.descripcion)).ToList<RepararItemDTO>()))
             .FirstOrDefaultAsync();


            if (reparacion == null)
            {
                _logger.LogError($"Error: Reparacion with id {id} does not exist");
                return NotFound();
            }


            return Ok(reparacion);
        }
    }
}