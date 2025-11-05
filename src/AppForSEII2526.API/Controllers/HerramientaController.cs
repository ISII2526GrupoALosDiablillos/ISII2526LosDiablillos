using AppForSEII2526.API.DTO.HerramientaDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppForSEII2526.API.Controllers   
{
    [Route("api/[controller]")]
    [ApiController]
    public class HerramientaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HerramientaController> _logger;

        public HerramientaController(ApplicationDbContext context, ILogger<HerramientaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientas_sinDTOs()
        {
            IList<Herramienta> herramienta = await _context.Herramientas.ToListAsync();
            return Ok(herramienta);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaRepararDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientaParaRepararDTO(int filtroTiempoReparacion, string filtroNombre)
        {
            var herramientas = await _context.Herramientas
                .Where(c => (filtroTiempoReparacion == null || c.tiempoReparacion==filtroTiempoReparacion) && (filtroNombre == null || c.nombre.Contains(filtroNombre)))
                .Select(c => new HerramientaParaRepararDTO(c.id, c.nombre, c.material, c.fabricante.Nombre, c.precio, c.tiempoReparacion)).ToListAsync();
            return Ok(herramientas);
       
    }
        }
}