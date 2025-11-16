using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.HerramientaDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.HerramientasController_Test
{
    public class GetHerramientasParaComprar_test : AppForMovies4SqliteUT
    {
        public GetHerramientasParaComprar_test()
        {
            var fabricantes = new List<Fabricante>() {
                new Fabricante(1,"Aceros manolo", null),
                new Fabricante(2,"Maderas Juan", null),
                new Fabricante(3,"Aluminios Carlos", null),
                new Fabricante(4,"Cristales Joaquin", null)
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, null),
                new Herramienta(2,10,"madera", "Martillo", 15, 40, null),
                new Herramienta(3,15,"acero", "Clavos", 20, 50, null),
                new Herramienta(4,20,"acero", "Desatornillador", 100, 100, null),
            };

            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaComprar_OK()
        {
            var fabricantes = new List<Fabricante>() {
                new Fabricante(1,"Aceros manolo", null),
                new Fabricante(2,"Maderas Juan", null),
                new Fabricante(3,"Aluminios Carlos", null),
                new Fabricante(4,"Cristales Joaquin", null)
            };

            var herramientaDTOs = new List<HerramientaParaComprarDTO>() {
                new HerramientaParaComprarDTO(1, "Serrucho", "aluminio", fabricantes[0].Nombre, 25),
                new HerramientaParaComprarDTO(2, "Martillo", "madera", fabricantes[1].Nombre, 15),
                new HerramientaParaComprarDTO(3, "Clavos", "acero", fabricantes[2].Nombre, 20),
                new HerramientaParaComprarDTO(4, "Desatornillador", "acero", fabricantes[3].Nombre, 100)
            };

            var herramientaSinFiltros = herramientaDTOs.OrderBy(m => m.id).ToList();
            var herramientaPorMaterial = herramientaDTOs.Where(h => h.material.Equals("acero", StringComparison.OrdinalIgnoreCase))
                                                      .OrderBy(m => m.id).ToList();
            var herramientaPorPrecio = herramientaDTOs.Where(h => h.precio == 20)
                                                     .OrderBy(m => m.id).ToList();

            return new List<object[]>
            {
                // filterPrice, filterMaterial, expectedHerramientas
                new object[] { null, null, herramientaSinFiltros },
                new object[] { null, "acero", herramientaPorMaterial },
                new object[] { 20, null, herramientaPorPrecio }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaComprar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientaParaComprar_OK_test(int? filterPrice, string? filterMaterial,
            IList<HerramientaParaComprarDTO> expectedHerramientas)
        {
            // Arrange
            var controller = new HerramientaController(_context, null!);

            // Act
            var actionResult = await controller.GetHerramientaParaComprarDTO(filterPrice, filterMaterial);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaComprarDTO>>(okResult.Value);

            // Compare by id to avoid failing on different object instances
            Assert.Equal(expectedHerramientas.Select(e => e.id), herramientaDTOsActual.Select(a => a.id));
        }
    }
}