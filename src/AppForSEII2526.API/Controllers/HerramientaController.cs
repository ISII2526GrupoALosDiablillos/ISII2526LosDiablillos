using AppForSEII2526.API.DTO;
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
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        {
            if (op2 == 0)
            {
                _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
                return BadRequest("op2 must be different from 0");
            }
            decimal result = decimal.Round(op1 / op2, 2);
            return Ok(result);
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
        [ProducesResponseType(typeof(IList<HerramientaDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientas_conTodosLosDatosDTOs()
        {
            var herramientas = await _context.Herramientas
                .Select(c => new HerramientaDTO(c.id, c.nombre, c.material, c.fabricante, c.precio)).ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramienta_FILTRO_MATERIAL_DTO(string? filtroMaterial)
        {
            var herramientas = await _context.Herramientas
                .Where(c => c.material.Contains(filtroMaterial)|| filtroMaterial==null)
                .Where(c => c.material.Contains(filtroMaterial) || filtroMaterial == null)
                .Select(c => new HerramientaDTO(c.id, c.nombre, c.material, c.fabricante, c.precio)).ToListAsync();
            return Ok(herramientas);
        }
    }
}
}
