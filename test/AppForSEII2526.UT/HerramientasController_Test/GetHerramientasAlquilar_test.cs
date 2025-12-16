using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.HerramientaDTOs;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_Test
{
    public class GetHerramientaAlquilar_test : AppForMovies4SqliteUT
    {
        public GetHerramientaAlquilar_test()
        {

            var fabricantes = new List<Fabricante>() {
                new Fabricante(1,"Aceros manolo", null),
                new Fabricante(2,"Maderas Juan", null),
                new Fabricante(3,"Aluminios Carlos", null),
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, fabricantes[0]),
                new Herramienta(2, 10, "madera", "Martillo", 15, 40, fabricantes[1]),
                new Herramienta(3, 15, "metal", "Clavos", 20, 60, fabricantes[2]),
            };



            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);

            _context.SaveChanges();
        }


        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaAlquilar_OK()
        {

            var herraiemntaDTOs = new List<HerramientaParaAlquilarDTO>() {
                new HerramientaParaAlquilarDTO(1,"Serrucho", "aluminio", "Aceros Manolo", 25),
                new HerramientaParaAlquilarDTO(2,"Martillo","madera", "Maderas Juan",15),
                new HerramientaParaAlquilarDTO(3, "Clavos", "metal", "Aluminios Carlos", 20),
            };

            var herramientaDTOsTC1 = new List<HerramientaParaAlquilarDTO>() { herraiemntaDTOs[0], herraiemntaDTOs[1], herraiemntaDTOs[2] };
            var herramientaDTOsTC2 = new List<HerramientaParaAlquilarDTO>() { herraiemntaDTOs[1] };
            var herramientaDTOsTC3 = new List<HerramientaParaAlquilarDTO>() { herraiemntaDTOs[2] };


            var allTests = new List<object[]>
            {            
                new object[] { null, null, herramientaDTOsTC1,  },
                new object[] { "Martillo", null, herramientaDTOsTC2, },
                new object[] { null, "metal", herramientaDTOsTC3, },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaAlquilar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientaParaAlquilar_OK_test(string? filtroNombre, string? filtroMaterial, IList<HerramientaParaAlquilarDTO> expectedHerramientas)
        {
            
            var controller = new HerramientaController(_context, null!);

            var result = await controller.GetHerramientaParaAlquilarDTO(filtroNombre, filtroMaterial);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaAlquilarDTO>>(okResult.Value);
            Assert.Equal(expectedHerramientas.Select(e => e.Id), herramientaDTOsActual.Select(a => a.Id));

        }
    }
}