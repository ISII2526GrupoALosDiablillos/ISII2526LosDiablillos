using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.HerramientasController_Test
{
    public class CompraDetailDTO_test : AppForMovies4SqliteUT
    {
        public CompraDetailDTO_test()
        {
            var fabricantes = new List<Fabricante>() {
                new Fabricante(1,"Makita", null),
                new Fabricante(2,"Bosch", null),
                new Fabricante(3,"Milwaukee", null),
                new Fabricante(4,"Stanley", null)
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,"Madera", "Martillo", 20, 50, fabricantes[0]),
                new Herramienta(3,10,"Metal", "Destornillador", 15, 40, fabricantes[1]),
                new Herramienta(4,15,"Madera", "Astillas", 5, 50, fabricantes[2]),
                new Herramienta(6,20,"Acero", "Sierra", 20, 100, fabricantes[3]),
            };

            ApplicationUser username = new ApplicationUser("Ortiz", "gonzalo@alu.uclm.es", "Gonza", 684512269);
            Compra compra = new Compra("Mi casa", DateTime.Today, 20, 0, new List<CompraItem>(), username)
            {
                atributos = username,
                compraItem = new List<CompraItem>()
            };

            compra.compraItem.Add(new CompraItem(1, "Afilada", 1, 6, 20));

            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);
            _context.Add(username);
            _context.Add(compra);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetCompraDetalle_NotFound_test()
        {
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            var result = await controller.GetCompraDetails(0);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompraDetalle_Found_test()
        {
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;
            var controller = new ComprasController(_context, logger);

            var expectedCompra = new CompraDetailDTO(1, "Gonza", "Ortiz", "Mi casa", 20, DateTime.Today, new List<CompraItemDTO>());
            expectedCompra.compraItems.Add(new CompraItemDTO("Sierra", "Acero", 20, "Afilada", 1, 6, 1));

            var result = await controller.GetCompraDetails(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var compraDTOActual = Assert.IsType<CompraDetailDTO>(okResult.Value);

            Assert.Equal(expectedCompra, compraDTOActual);

            Assert.Single(compraDTOActual.compraItems);
            var compraItem = compraDTOActual.compraItems[0];

            Assert.Equal(6, compraItem.herramientaId);
            Assert.Equal("Sierra", compraItem.nombre);
            Assert.Equal("Acero", compraItem.material);
            Assert.Equal("Afilada", compraItem.descripcion);
            Assert.Equal(20, compraItem.precio);
            Assert.Equal(1, compraItem.cantidad);
        }
    }
}