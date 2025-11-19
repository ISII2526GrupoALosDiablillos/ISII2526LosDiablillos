using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Oferta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.OfertaController_test
{
    public class GetDetalleOferta_test : AppForMovies4SqliteUT
    {
        public GetDetalleOferta_test()
        {
            var user = new ApplicationUser(
                apellidoCliente: "TestApellido",
                correoElectronico: "test@tests.com",
                nombreCliente: "TestNombre",
                telefono: 123456789,
                compras: new List<Compra>()
            )
            {
                Id = "test-user",
                UserName = "test@tests.com",
                NormalizedUserName = "TEST@TESTS.COM",
                Email = "test@tests.com",
                NormalizedEmail = "TEST@TESTS.COM",
                EmailConfirmed = true
            };

            _context.Add(user);

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
                new List<OfertaItem>())
            {
                applicationUser = user
            };

            oferta.ofertaItems.Add(new OfertaItem(
                herramientas[0].id,
                oferta.Id == 0 ? 1 : oferta.Id,
                50.0,
                10.0
            ));

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
            var mock = new Mock<ILogger<OfertaItemController>>();
            ILogger<OfertaItemController> logger = mock.Object;

            var controller = new OfertaItemController(_context, logger);

            var result = await controller.GetOfertaDetalleById(0);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }


        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetOfertasPorId_Found_test()
        {
            var mock = new Mock<ILogger<OfertaItemController>>();
            ILogger<OfertaItemController> logger = mock.Object;
            var controller = new OfertaItemController(_context, logger);

            var baseDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var fechaInicio = baseDate.AddDays(2);
            var fechaFinal = baseDate.AddDays(5);
            var fechaOferta = baseDate;

            var expectedOferta = new OfertaDetailDTO(
                        fechaInicio,
                        fechaFinal,
                        fechaOferta,
                        tiposDirigidaOferta.Clientes,
                        tiposMetodoPago.PayPal,
                        new List<OfertaItemDTO>());

            expectedOferta.Items.Add(new OfertaItemDTO(1, "Martillo", "Acero", "Stanley", 20m, 50.0, 10.0));

            var result = await controller.GetOfertaDetalleById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var ofertaDTOActual = Assert.IsType<OfertaDetailDTO>(okResult.Value);

            Assert.Equal(expectedOferta.FechaInicio, ofertaDTOActual.FechaInicio);
            Assert.Equal(expectedOferta.FechaFinal, ofertaDTOActual.FechaFinal);
            Assert.Equal(expectedOferta.FechaOferta, ofertaDTOActual.FechaOferta);
            Assert.Equal(expectedOferta.DirigidaOferta, ofertaDTOActual.DirigidaOferta);
            Assert.Equal(expectedOferta.MetodoPago, ofertaDTOActual.MetodoPago);

            Assert.Single(ofertaDTOActual.Items);
            var actualItem = ofertaDTOActual.Items[0];

            Assert.Equal(1, actualItem.HerramientaId);
            Assert.Equal("Martillo", actualItem.NombreHerramienta);
            Assert.Equal("Acero", actualItem.Material);
            Assert.Equal("Stanley", actualItem.Fabricante);
            Assert.Equal(20m, actualItem.PrecioOriginal);
            Assert.Equal(50.0, actualItem.PorcentajeDescuento);
            Assert.Equal(10.0, actualItem.PrecioFinal);
        }
    }
}
