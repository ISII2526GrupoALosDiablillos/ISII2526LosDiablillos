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
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientas_sinDTOs()
        {
            IList<Models.Herramienta> herramienta = await _context.Herramientas.ToListAsync();
            return Ok(herramienta);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientas_conTodosLosDatosDTOs()
        {
            var herramientas = await _context.Herramientas
                .Select(c => new HerramientaParaComprarDTO(c.id, c.nombre, c.material, c.fabricante, c.precio)).ToListAsync();
            return Ok(herramientas);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramienta_FILTRO_MATERIAL_DTO(string? filtroMaterial)
        {
            var herramientas = await _context.Herramientas
                .Where(c => c.material.Contains(filtroMaterial) || filtroMaterial == null)
                .Select(c => new HerramientaParaComprarDTO(c.id, c.nombre, c.material, c.fabricante, c.precio)).ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramienta_FILTRO_PRECIO_DTO(string? filtroPrecio)
        {
            var herramientas = await _context.Herramientas
                .Where(c => c.precio.ToString().Contains(filtroPrecio) || filtroPrecio == null)
                .Select(c => new HerramientaParaComprarDTO(c.id, c.nombre, c.material, c.fabricante, c.precio)).ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramienta_FILTRO_NOMBRE_DTO(string? filtroNombre)
        {
            var herramientas = await _context.Herramientas
                .Where(c => c.material.Contains(filtroNombre) || filtroNombre == null)
                .Select(c => new HerramientaParaComprarDTO(c.id, c.nombre, c.material, c.fabricante, c.precio)).ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramienta_FILTRO_TIEMPOREPARACION_DTO(string? filtroTiempoReparacion)
        {
            var herramientas = await _context.Herramientas
                .Where(c => c.material.Contains(filtroTiempoReparacion) || filtroTiempoReparacion == null)
                .Select(c => new HerramientaParaComprarDTO(c.id, c.nombre, c.material, c.fabricante, c.precio)).ToListAsync();
            return Ok(herramientas);
        }
    }

}