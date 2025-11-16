using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Oferta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.OfertaController_test
{
    public class PostOferta_test : AppForMovies4SqliteUT
    {
        public PostOferta_test()
        {
            var fabricante = new Fabricante(1, "Bosch", new List<Herramienta>());

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

        public static IEnumerable<object[]> TestCasesFor_CreateOferta()
        {
            // Use OfertaForCreateDTO + OfertaItemForCreateDTO (DTO para creación)
            var ofertaNoItem = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(5),
                FechaFinal = DateTime.Today.AddDays(2),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
            };

            var ofertaItems = new List<OfertaItemForCreateDTO>()
            {
                new OfertaItemForCreateDTO { HerramientaId = 1, Porcentaje = 50.0, PrecioFinal = 10.0 }
            };

            var ofertaFromBeforeToday = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(3),
                FechaFinal = DateTime.Today,
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = ofertaItems
            };

            var ofertaToBeforeFrom = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(2),
                FechaFinal = DateTime.Today.AddDays(5),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = ofertaItems
            };

            var ofertaHerramientaNoDisponible = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(5),
                FechaFinal = DateTime.Today.AddDays(2),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
                { new OfertaItemForCreateDTO { HerramientaId = 999, Porcentaje = 50.0, PrecioFinal = 40.0 } }
            };

            var ofertaPorcentajeNoValido = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(5),
                FechaFinal = DateTime.Today.AddDays(2),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
                { new OfertaItemForCreateDTO { HerramientaId = 1, Porcentaje = 150.0, PrecioFinal = 20.0 } }
            };

            var ofertaSinFechaFinal = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.MinValue,
                FechaFinal = DateTime.Today.AddDays(2),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
                { new OfertaItemForCreateDTO { HerramientaId = 1, Porcentaje = 50.0, PrecioFinal = 20.0 } }
            };

            var ofertaSinFechaInicio = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(5),
                FechaFinal = DateTime.MinValue,
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
                { new OfertaItemForCreateDTO { HerramientaId = 1, Porcentaje = 50.0, PrecioFinal = 20.0 } }
            };

            var allTests = new List<object[]>
            {
                new object[] { ofertaNoItem, "Error! Tienes que incluir al menos una herramienta para aplicar una oferta" },
                new object[] { ofertaFromBeforeToday, "Error! La fecha de inicio de tu oferta debe ser posterior a hoy" },
                new object[] { ofertaToBeforeFrom, "Error! Tu oferta debe terminar después de que empiece" },
                // adapt expected message to reference id because creation DTO doesn't include NombreHerramienta
                new object[] { ofertaHerramientaNoDisponible, $"La herramienta con id {ofertaHerramientaNoDisponible.OfertaItems[0].HerramientaId} no fue encontrada" },
                new object[] { ofertaPorcentajeNoValido, "Error: El porcentaje debe estar entre 0 y 100" },
                new object[] { ofertaSinFechaFinal, "Error! Fecha Final es un campo obligatorio" },
                new object[] { ofertaSinFechaInicio, "Error! Fecha Inicio es un campo obligatorio" },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateOferta))]
        public async Task CreateOferta_Error_test(OfertaForCreateDTO ofertaDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<OfertaItemController>>();
            ILogger<OfertaItemController> logger = mock.Object;

            var controller = new OfertaItemController(_context, logger);

            // Act
            var result = await controller.CreateOferta(ofertaDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateOferta_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertaItemController>>();
            ILogger<OfertaItemController> logger = mock.Object;

            var controller = new OfertaItemController(_context, logger);

            var ofertaDTO = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(5),
                FechaFinal = DateTime.Today.AddDays(2),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>
                {
                    new OfertaItemForCreateDTO { HerramientaId = 1, Porcentaje = 50.0, PrecioFinal = 10.0 }
                }
            };

            var expectedOfertaDetailDTO = new OfertaDetailDTO(
                ofertaDTO.FechaInicio,
                ofertaDTO.FechaFinal,
                DateTime.Today,
                ofertaDTO.DirigidaOferta,
                ofertaDTO.MetodoPago,
                new List<OfertaItemDTO>
                {
                    new OfertaItemDTO(1, "Martillo", "Acero", "Stanley", 20m, 50.0, 10.0)
                });

            // Act
            var result = await controller.CreateOferta(ofertaDTO);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualOfertaDetailDTO = Assert.IsType<OfertaDetailDTO>(createdResult.Value);

            Assert.Equal(expectedOfertaDetailDTO, actualOfertaDetailDTO);
        }
    }
}
