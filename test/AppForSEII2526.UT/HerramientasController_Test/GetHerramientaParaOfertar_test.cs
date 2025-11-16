using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.HerramientaDTOs;
using AppForSEII2526.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetHerramientaParaOferta_test : AppForMovies4SqliteUT
    {
        public GetHerramientaParaOferta_test()
        {
            var fabricante = new List<Fabricante>
            {
                new Fabricante (1, "Ferretería López", null),
                new Fabricante (2, "Ferretería García", null),
                new Fabricante (3, "Ferretería Ruiz", null)
            };

            var herramientas = new List<Herramienta>
            {
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, null),
                new Herramienta(2,10,"madera", "Martillo", 15, 40, null),
                new Herramienta(3,15,"acero", "Clavos", 20, 50, null)
            };

            _context.AddRange(fabricante);
            _context.AddRange(herramientas);
            _context.SaveChanges();

        }

        public static IEnumerable<object?[]> TestCasesFor_GetHerramientasParaOferta_OK()
        {
            var herramientasDTOs = new List<HerramientaParaOfertarDTO>
            {
                new HerramientaParaOfertarDTO (1, "Martillo", "Madera", "Ferretería López", 5),
                new HerramientaParaOfertarDTO (2, "Taladro", "Acero", "Ferretería García", 30),
                new HerramientaParaOfertarDTO (3, "Alicates", "Hierro", "Ferretería Ruiz", 4)
            };

            var herramientasDTOs_TC1 = new List<HerramientaParaOfertarDTO> { herramientasDTOs[0], herramientasDTOs[1], herramientasDTOs[2] };

            var herramientasDTOs_TC2 = new List<HerramientaParaOfertarDTO> { herramientasDTOs[1] };
            var herramientasDTOs_TC3 = new List<HerramientaParaOfertarDTO> { herramientasDTOs[2] };

            var allTestCases = new List<object?[]>
            {
                new object[] { null, null, herramientasDTOs_TC1 },
                new object[] { "Ferretería García", null, herramientasDTOs_TC2 },
                new object[] { null, 4, herramientasDTOs_TC3 }
            };
            return allTestCases;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaOferta_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientaParaOferta_OK_test(string? fabricante, int? precio, IList<HerramientaParaOfertarDTO> herramientasDTOEsperado)
        {
            // Arrange
            var controller = new HerramientaController(_context, null);

            // Act
            var result = await controller.GetHerramientaParaOfertarDTO(precio, fabricante);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var herramientasDTOActual = Assert.IsType<List<HerramientaParaOfertarDTO>>(okResult.Value);
            Assert.Equal(herramientasDTOEsperado, herramientasDTOActual);
        }
    }
}