using AppForSEII2526.API.DTO.HerramientaDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
                _logger.LogError($"{DateTime.Now} Exception: op2=0, divido por 0");
                return BadRequest("op2 debe ser diferente de 0");
            }
            decimal result = decimal.Round(op1 / op2, 2);
            return Ok(result);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientassinDTOs()
        {
            IList<Models.Herramienta> herramienta = await _context.Herramientas.ToListAsync();
            return Ok(herramienta);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaRepararDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientaParaRepararDTO(string? nombre, float? tiempoReparacion)
        {
            var herramientas = await _context.Herramientas
                .Include(c => c.fabricante)
                .Where(c => (nombre == null || c.nombre == nombre) && (tiempoReparacion == null || c.tiempoReparacion <= tiempoReparacion))
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
        public async Task<ActionResult> GetHerramientaParaComprarDTO(int? filtroPrecio, string? filtroMaterial)
        {
            var herramientas = await _context.Herramientas
                .Where(c => (filtroPrecio == null || c.precio==filtroPrecio) && (filtroMaterial == null || c.material==(filtroMaterial)))
                .Select(c => new HerramientaParaComprarDTO(c.id, c.nombre, c.material, c.fabricante.Nombre, c.precio)).ToListAsync();
            return Ok(herramientas);
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaOfertarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientaParaOfertarDTO(int? filtroPrecio, string filtroFabricante)
        {
            var herramientas = await _context.Herramientas
                .Include(c => c.fabricante)
                .Where(c => (filtroPrecio == 0 || c.precio == filtroPrecio) && (filtroFabricante == null || c.fabricante.Nombre.Contains(filtroFabricante)))
                .Select(c => new HerramientaParaOfertarDTO(c.id, c.nombre, c.material, c.fabricante.Nombre, c.precio)).ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaOfertarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientasPorFabricantePrecio(string? fabricante, float? precio)
        {
            var query = _context.Herramientas.Include(h => h.fabricante).AsQueryable();

            if (!string.IsNullOrEmpty(fabricante))
            {
                query = query.Where(h => h.fabricante != null && h.fabricante.Nombre.Contains(fabricante));
            }

            if (precio.HasValue)
            {
                // comparar convirtiendo el precio almacenado (int) a float para evitar problemas de tipos
                float buscado = precio.Value;
                query = query.Where(h => (float)h.precio == buscado);
            }

            var resultado = await query
                .Select(h => new HerramientaParaOfertarDTO(h.id, h.nombre, h.material, h.fabricante != null ? h.fabricante.Nombre : string.Empty, (double)h.precio))
                .ToListAsync();

            return Ok(resultado);
        }

    }
}