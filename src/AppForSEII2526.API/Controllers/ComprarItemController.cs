using AppForSEII2526.API.DTO.CompraDTOs;

namespace AppForSEII2526.API.Controllers
{
    public class ComprarItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComprarItemController> _logger;
        public ComprarItemController(ApplicationDbContext context, ILogger<ComprarItemController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCompra(int id)
        {
            if (_context.Compras == null)
            {
                _logger.LogError("Error. La tabla Compra no existe.");
                return NotFound();
            }

            var comprasDTO = await _context.Compras
                .Where(c => c.id == id)
                    .Include(c => c.Compras)
                        .ThenInclude(f => f.fabricante)
                            .ThenInclude(h => h.herramienta)
                    .Select(comprar => new CompraDetailDTO(
                        comprar.nombre_cliente,
                        comprar.apellido_cliente,
                        comprar.direccion,
                        comprar.precio_total,
                        comprar.fecha_compra,
                        comprar.nombre_herramienta,
                        comprar.material,
                        comprar.precio,
                        comprar.descripcion,
                        comprar.cantidad
                    ))
                    .FirstOrDefaultAsync();


            return Ok(comprasDTO);
        }

    }
}