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
            IList<Models.Herramienta> herramienta = await _context.Herramientas.ToListAsync();
            return Ok(herramienta);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaRepararDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientaParaRepararDTO(int filtroTiempoReparacion, string filtroNombre)
        {
            var herramientas = await _context.Herramientas
                .Where(c => (filtroTiempoReparacion == null || c.tiempoReparacion == filtroTiempoReparacion) && (filtroNombre == null || c.nombre.Contains(filtroNombre)))
                .Select(c => new HerramientaParaRepararDTO(c.id, c.nombre, c.material, c.fabricante.Nombre, c.precio, c.tiempoReparacion)).ToListAsync();
            return Ok(herramientas);
       
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaAlquilarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientaParaAlquilarDTO(string filtroMateiral, string filtroNombre)
        {
            var herramientas = await _context.Herramientas
                .Where(c => (filtroMateiral == null || c.material.Contains(filtroMateiral)) && (filtroNombre == null || c.nombre.Contains(filtroNombre)))
                .Select(c => new HerramientaParaAlquilarDTO(c.id, c.nombre, c.material, c.fabricante.Nombre, c.precio)).ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientaParaComprarDTO(int filtroPrecio, String filtroMaterial)
        {
            var herramientas = await _context.Herramientas
                .Where(c => (filtroPrecio == null || c.precio==filtroPrecio) && (filtroMaterial == null || c.material.Contains(filtroMaterial)))
                .Select(c => new HerramientaParaComprarDTO(c.id, c.nombre, c.material, c.fabricante.Nombre, c.precio)).ToListAsync();
            return Ok(herramientas);
        }
    }
}