using AppForSEII2526.API.DTO.RepararDTOs;
using AppForSEII2526.API.DTO.HerramientaDTOs;
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
                _logger.LogError("Error: Tabla Reparaciones no existe");
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
                _logger.LogError($"Error: Reparacion con id {id} no existe");
                return NotFound();
            }


            return Ok(reparacion);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RepararDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        
        public async Task<ActionResult> CreateReparacion(ReparacionForCreateDTO repararForCreateDTO)
        {
            //  Validaciones iniciales
            if (string.IsNullOrWhiteSpace(repararForCreateDTO.NombreCliente))
                ModelState.AddModelError("NombreCliente", "El nombre del cliente es obligatorio");

            if (string.IsNullOrWhiteSpace(repararForCreateDTO.ApellidoCliente))
                ModelState.AddModelError("ApellidoCliente", "El apellido del cliente es obligatorio");

            if (repararForCreateDTO.FechaEntrega <= repararForCreateDTO.FechaRecogida)
                ModelState.AddModelError("FechaEntrega", "La fecha de entrega debe ser posterior a la de recogida");

            if (repararForCreateDTO.ReparacionItems == null || repararForCreateDTO.ReparacionItems.Count == 0)
                ModelState.AddModelError("ReparacionItems", "Debe incluir al menos una herramienta para reparar");

            // Validar cantidades
            foreach (var item in repararForCreateDTO.ReparacionItems)
            {
                if (item.Cantidad <= 0)
                    ModelState.AddModelError("Cantidad", $"Debe especificar una cantidad válida para la herramienta {item.HerramientaID}");
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // 2 Buscar herramientas en BD
            var herramientasIds = repararForCreateDTO.ReparacionItems.Select(ri => ri.HerramientaID).ToList();

            var herramientas = await _context.Herramientas
                .Where(h => herramientasIds.Contains(h.id))
                .ToListAsync();

            //  Crear entidad principal Reparacion
            var reparacion = new Reparacion
            {
                NombreCliente = repararForCreateDTO.NombreCliente,
                ApellidoCliente = repararForCreateDTO.ApellidoCliente,
                FechaRecogida = repararForCreateDTO.FechaRecogida,
                FechaEntrega = repararForCreateDTO.FechaEntrega,
                MetodosPago = repararForCreateDTO.MetodosPago,
                Telefono = repararForCreateDTO.Telefono,
                ReparacionItems = new List<ReparacionItem>()
            };

            decimal precioTotal = 0;

            //  Procesar herramientas seleccionadas
            foreach (var item in repararForCreateDTO.ReparacionItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.id == item.HerramientaID);
                if (herramienta == null)
                {
                    ModelState.AddModelError("ReparacionItems", $"La herramienta con id {item.HerramientaID} no existe");
                    continue;
                }

                // Crear ReparacionItem asociado
                var reparacionItem = new ReparacionItem
                {
                    HerramientaId = herramienta.id,
                    cantidad = item.Cantidad,
                    descripcion = item.Descripcion,
                    precio = herramienta.precio,
                    Reparacion = reparacion
                };

                reparacion.ReparacionItems.Add(reparacionItem);

                // Sumar precio
                precioTotal += herramienta.precio * item.Cantidad;
            }

            // Si hubo errores de herramientas inexistentes
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            reparacion.PrecioTotal = (double)precioTotal;

            //  Guardar en BD
            _context.Add(reparacion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Conflict("Error al guardar la reparación: " + ex.Message);
            }

            //  Preparar DTO de respuesta
            var reparacionDetailDTO = new RepararDetailDTO(
                reparacion.Id,
                reparacion.NombreCliente,
                reparacion.ApellidoCliente,
                reparacion.FechaRecogida,
                reparacion.FechaEntrega,
                reparacion.ReparacionItems.Select(ri =>
                    new RepararItemDTO(
                        ri.HerramientaId,
                        herramientas.First(h => h.id == ri.HerramientaId).nombre,
                        ri.precio,
                        ri.cantidad,
                        ri.descripcion
                    )).ToList()
            );

            return CreatedAtAction(nameof(GetReparacionDetail), new { id = reparacionDetailDTO.Id }, reparacionDetailDTO);
        }

    }


}