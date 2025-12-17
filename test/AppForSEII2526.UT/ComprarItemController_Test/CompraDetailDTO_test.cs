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
                new Fabricante(1,"Aceros Manolo", null),
                new Fabricante(2,"Maderas Juan", null),
                new Fabricante(3,"Aluminios Carlos", null),
                new Fabricante(4,"Cristales Paqui", null)
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, fabricantes[0]),
                new Herramienta(2,10,"madera",   "Martillo", 15, 40, fabricantes[1]),
                new Herramienta(3,15,"acero",    "Clavos",   20, 50, fabricantes[2]),
                new Herramienta(4,20,"acero",    "Desatornillador", 100, 100, fabricantes[3]),
            };

            ApplicationUser username = new ApplicationUser("Ortiz", "gonzalo@alu.uclm.es", "Gonzalo", 684512269, new List<Compra>());
            Compra compra = new Compra("Mi casa", DateTime.Today, 5, 33.70, new List<CompraItem>(), username)
            {
                atributos = username,
                compraItem = new List<CompraItem>()
            };

            compra.compraItem.Add(new CompraItem(1, "Anticuado", 5, 2, 15));

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

            var expectedCompra = new CompraDetailDTO(5, "Gonzalo", "Ortiz", "Mi casa", 33.70, DateTime.Today, new List<CompraItemDTO>());
            expectedCompra.compraItems.Add(new CompraItemDTO("Martillo", "madera", 15, "Anticuado", 1, 2, 3));

            var result = await controller.GetCompraDetails(5);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var compraDTOActual = Assert.IsType<CompraDetailDTO>(okResult.Value);

            Assert.Equal(expectedCompra.id, compraDTOActual.id);
            Assert.Equal(expectedCompra.direccion, compraDTOActual.direccion);
            Assert.Equal(expectedCompra.apellido_cliente, compraDTOActual.apellido_cliente);
            Assert.Equal(expectedCompra.nombre_cliente, compraDTOActual.nombre_cliente);
            Assert.Equal(expectedCompra.preciototal, compraDTOActual.preciototal);
            Assert.Equal(expectedCompra.fechaCompra, compraDTOActual.fechaCompra);

            Assert.Single(compraDTOActual.compraItems);
            var compraItem = compraDTOActual.compraItems[0];

            Assert.Equal(2, compraItem.HerramientaId);
            Assert.Equal("Martillo", compraItem.Nombre);
            Assert.Equal("madera", compraItem.Material);
            Assert.Equal("Anticuado", compraItem.Descripcion);
            Assert.Equal(15, compraItem.Precio);
            Assert.Equal(1, compraItem.Cantidad);
        }
    }
}