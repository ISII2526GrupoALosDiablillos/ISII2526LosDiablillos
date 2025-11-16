using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Oferta;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.OfertaController_test
{
    public class GetDetalleOferta_test : AppForMovies4SqliteUT
    {
        public GetDetalleOferta_test()
        {
            var fabricante = new Fabricante(1, "Stanley", new List<Herramienta>());

            var herramientas = new List<Herramienta>()
            {
                new Herramienta(1, 1, "Acero", "Martillo", 20, 1, fabricante),
                new Herramienta(2, 1, "Hierro", "Llave inglesa", 15, 1, fabricante),
            };

            var oferta = new Oferta(
                DateTime.Today.AddDays(5),
                DateTime.Today.AddDays(2),
                DateTime.Today,
                1,
                tiposMetodoPago.PayPal,
                tiposDirigidaOferta.Clientes,
                new List<OfertaItem>());

            oferta.ofertaItems.Add(new OfertaItem(herramientas[0].id, oferta.Id == 0 ? 1 : oferta.Id, 50.0, 10.0));

            _context.Add(fabricante);
            _context.AddRange(herramientas);
            _context.Add(oferta);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetOfertasPorId_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertaItemController>>();
            ILogger<OfertaItemController> logger = mock.Object;

            var controller = new OfertaItemController(_context, logger);

            // Act
            var result = await controller.GetOfertaDetalleById(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetOfertasPorId_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertaItemController>>();
            ILogger<OfertaItemController> logger = mock.Object;
            var controller = new OfertaItemController(_context, logger);

            var expectedOferta = new OfertaDetailDTO(
                        DateTime.Today.AddDays(5),
                        DateTime.Today.AddDays(2),
                        DateTime.Today,
                        tiposDirigidaOferta.Clientes,
                        tiposMetodoPago.PayPal,
                        new List<OfertaItemDTO>());

            expectedOferta.Items.Add(new OfertaItemDTO(1, "Martillo", "Acero", "Stanley", 20m, 50.0, 10.0));

            // Act 
            var result = await controller.GetOfertaDetalleById(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var ofertaDTOActual = Assert.IsType<OfertaDetailDTO>(okResult.Value);
            var eq = expectedOferta.Equals(ofertaDTOActual);

            Assert.Equal(expectedOferta, ofertaDTOActual);
        }
    }
}   