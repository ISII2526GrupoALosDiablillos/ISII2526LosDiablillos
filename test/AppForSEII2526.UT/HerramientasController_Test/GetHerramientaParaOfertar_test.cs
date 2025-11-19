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
    public class GetHerramientaParaOfertar_test : AppForMovies4SqliteUT
    {
        public GetHerramientaParaOfertar_test()
        {
            var fabricantes = new List<Fabricante>
            {
                new Fabricante(1, "Makita", new List<Herramienta>()),
                new Fabricante(2, "Milwaukee", new List<Herramienta>()),
                new Fabricante(3, "Stanley", new List<Herramienta>())
            };

            var herramientas = new List<Herramienta>()
            {
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
                new HerramientaParaOfertarDTO(1, "Martillo", "Acero", "Makita", 23.0),
                new HerramientaParaOfertarDTO(2, "Llave inglesa", "Hierro", "Milwaukee", 16.0),
                new HerramientaParaOfertarDTO(3, "Destornillador", "Acero y Plastico", "Stanley", 20.0)
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
                new object[] { "Milwaukee", null, herramientaDTOsTC2 },
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
            var controller = new HerramientaController(_context, null!);

            var result = await controller.GetHerramientasPorFabricantePrecio(fabricante, precio);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaOfertarDTO>>(okResult.Value);

            Assert.Equal(expectedHerramientas.Select(e=>e.id), herramientaDTOsActual.Select(a=>a.id));
        }
    }
}
