using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Oferta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.OfertaController_test
{
    public class PostOferta_test : AppForMovies4SqliteUT
    {
        public PostOferta_test()
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
                new List<OfertaItem>())
            {
                applicationUser = user
            };

            oferta.ofertaItems.Add(new OfertaItem(
                herramientas[0].id,
                1,
                50.0,
                10.0
            ));

            _context.Add(fabricante);
            _context.AddRange(herramientas);
            _context.Add(oferta);
            _context.SaveChanges();
        }


        public static IEnumerable<object[]> TestCasesFor_CreateOferta()
        {
            // 1) Sin items 
            var ofertaNoItem = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(2),
                FechaFinal = DateTime.Today.AddDays(5),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
            };

            var ofertaItemsValidos = new List<OfertaItemForCreateDTO>()
            {
                new OfertaItemForCreateDTO { HerramientaId = 1, Porcentaje = 50.0, PrecioFinal = 10.0 }
            };

            // 2) Fecha inicio antes de hoy 
            var ofertaFromBeforeToday = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(-1),
                FechaFinal = DateTime.Today.AddDays(5),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = ofertaItemsValidos
            };

            // 3) Fecha final antes de inicio
            var ofertaToBeforeFrom = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(5),
                FechaFinal = DateTime.Today.AddDays(2),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = ofertaItemsValidos
            };

            // 4) Herramienta inexistente
            var ofertaHerramientaNoDisponible = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(2),
                FechaFinal = DateTime.Today.AddDays(5),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
                {
                    new OfertaItemForCreateDTO { HerramientaId = 999, Porcentaje = 50.0, PrecioFinal = 40 }
                }
            };

            // 5) Porcentaje fuera de rango
            var ofertaPorcentajeNoValido = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(2),
                FechaFinal = DateTime.Today.AddDays(5),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>()
                {
                    new OfertaItemForCreateDTO { HerramientaId = 1, Porcentaje = 150, PrecioFinal = 20 }
                }
            };

            // 6) Sin fecha final
            var ofertaSinFechaFinal = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(2),
                FechaFinal = DateTime.MinValue,
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = ofertaItemsValidos
            };

            // 7) Sin fecha inicio
            var ofertaSinFechaInicio = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.MinValue,
                FechaFinal = DateTime.Today.AddDays(5),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = ofertaItemsValidos
            };


            return new List<object[]>
            {
                new object[]{ofertaNoItem, "Error! Tienes que incluir al menos una herramienta para aplicar una oferta"},
                new object[]{ofertaFromBeforeToday, "Error! La fecha de inicio de tu oferta debe ser posterior a hoy"},
                new object[]{ofertaToBeforeFrom, "Error! Tu oferta debe terminar después de que empiece"},
                new object[]{ofertaHerramientaNoDisponible, "La herramienta con id 999 no fue encontrada"},
                new object[]{ofertaPorcentajeNoValido, "Error: El porcentaje debe estar entre 0 y 100"},
                new object[]{ofertaSinFechaFinal, "Error! Fecha Final es un campo obligatorio"},
                new object[]{ofertaSinFechaInicio, "Error! Fecha Inicio es un campo obligatorio"},
            };
        }



        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateOferta))]
        public async Task CreateOferta_Error_test(OfertaForCreateDTO ofertaDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<OfertaItemController>>();
            var controller = new OfertaItemController(_context, mock.Object);

            var result = await controller.CreateOferta(ofertaDTO);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            var errorActual = problem.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }




        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateOferta_Success_test()
        {
            var mock = new Mock<ILogger<OfertaItemController>>();
            var controller = new OfertaItemController(_context, mock.Object);

            var dto = new OfertaForCreateDTO
            {
                FechaInicio = DateTime.Today.AddDays(2),
                FechaFinal = DateTime.Today.AddDays(5),
                MetodoPago = tiposMetodoPago.PayPal,
                DirigidaOferta = tiposDirigidaOferta.Clientes,
                OfertaItems = new List<OfertaItemForCreateDTO>
                {
                    new OfertaItemForCreateDTO{ HerramientaId=1, Porcentaje=50, PrecioFinal=10 }
                }
            };

            var result = await controller.CreateOferta(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<OfertaDetailDTO>(created.Value);

            Assert.Equal(dto.FechaInicio, actual.FechaInicio);
            Assert.Equal(dto.FechaFinal, actual.FechaFinal);
            Assert.Equal(dto.DirigidaOferta, actual.DirigidaOferta);
            Assert.Equal(dto.MetodoPago, actual.MetodoPago);

            Assert.Single(actual.Items);

            var item = actual.Items[0];
            Assert.Equal(1, item.HerramientaId);
            Assert.Equal("Martillo", item.NombreHerramienta);
            Assert.Equal("Acero", item.Material);
            Assert.Equal("Bosch", item.Fabricante);
            Assert.Equal(20m, item.PrecioOriginal);
            Assert.Equal(50, item.PorcentajeDescuento);
            Assert.Equal(10, item.PrecioFinal);
        }
    }
}
