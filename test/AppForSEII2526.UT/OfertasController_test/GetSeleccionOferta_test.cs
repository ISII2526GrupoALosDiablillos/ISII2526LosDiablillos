using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.HerramientaDTOs;
using AppForSEII2526.API.DTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_Test
{
    public class GetSeleccionOferta_test : AppForMovies4SqliteUT
    {
        public GetSeleccionOferta_test()
        {
            var fabricantes = new List<Fabricante>
            {
                new Fabricante(1, "Makita", new List<Herramienta>()),
                new Fabricante(2, "Milwaukee", new List<Herramienta>()),
                new Fabricante(3, "Stanley", new List<Herramienta>())
            };

            var herramientas = new List<Herramienta>()
            {
                // Constructor Herramienta(int id, int itemsReparacion, string material, string nombre, int precio, int tiempoReparacion, Fabricante fabricante)
                new Herramienta(1, 1, "Acero", "Martillo", 23, 30, fabricantes[0]),
                new Herramienta(2, 1, "Hierro", "Llave inglesa", 16, 30, fabricantes[1]),
                new Herramienta(3, 1, "Acero y Plastico", "Destornillador", 20, 30, fabricantes[2])
            };

            _context.AddRange(fabricantes);
            _context.AddRange(herramientas);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetHerramientasPorFabricantePrecio()
        {
            var herramientaDTOs = new List<HerramientaParaOfertarDTO>()
            {
                // Ajustada la orden de parámetros al constructor declarado: (id, nombre, material, fabricante, precio)
                new HerramientaParaOfertarDTO(1, "Martillo", "Acero", "Bosch", 23.0),
                new HerramientaParaOfertarDTO(2, "Llave inglesa", "Hierro", "Makita", 16.0),
                new HerramientaParaOfertarDTO(3, "Destornillador", "Metal y Platico", "Stanley", 20.0)
            };

            var herramientaDTOsTC1 = new List<HerramientaParaOfertarDTO>()
            {
                herramientaDTOs[0],
                herramientaDTOs[1],
                herramientaDTOs[2]
            };

            var herramientaDTOsTC2 = new List<HerramientaParaOfertarDTO>()
            {
                herramientaDTOs[1]
            };

            var herramientaDTOsTC3 = new List<HerramientaParaOfertarDTO>()
            {
                herramientaDTOs[2]
            };

            var allTest = new List<object[]>()
            {
                new object[] { null, null, herramientaDTOsTC1 },
                new object[] { "DeWalt", null, herramientaDTOsTC2 },
                new object[] { null, 20.0f, herramientaDTOsTC3 }
            };

            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasPorFabricantePrecio))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetHerramientasPorFabricantePrecio_test(string? fabricante, float? precio, List<HerramientaParaOfertarDTO> expectedHerramientas)
        {
            // Arrange
            var controller = new HerramientaController(_context, null!);

            // Act
            var result = await controller.GetHerramientasPorFabricantePrecio(fabricante, precio);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaOfertarDTO>>(okResult.Value);

            Assert.Equal(expectedHerramientas, herramientaDTOsActual);
        }
    }
}