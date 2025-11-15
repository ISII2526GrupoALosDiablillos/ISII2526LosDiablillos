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
    public class GetHerramientasParaReparar_test : AppForMovies4SqliteUT
    {
        public GetHerramientasParaReparar_test()
        {

            var fabricantes = new List<Fabricante>() {
                new Fabricante(1,"Aluminios Manolo", null),
                new Fabricante(2,"Maderas Juan", null),
                new Fabricante(3,"Aceros Carlos", null),
            };

            var herramientas = new List<Herramienta>(){
                new Herramienta(1,5,"Aluminio", "Serrucho", 25, 50, fabricantes[0]),
                new Herramienta(2, 10, "Madera", "Martillo", 15, 40, fabricantes[1]),
                new Herramienta(3, 15, "Acero", "Clavos", 20, 50, fabricantes[2]),   
            };


               
            _context.AddRange(fabricantes);
            _context.AddRange(herramientas);

            _context.SaveChanges();
        }


        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaReparar()
        {

            var herramientaDTOs = new List<HerramientaParaRepararDTO>() {
                new HerramientaParaRepararDTO(1, "Serrucho", "Aluminio", "Aluminios Manolo", 25, 10),
                new HerramientaParaRepararDTO(2, "Martillo","Madera", "Maderas Juan",15, 15),
                new HerramientaParaRepararDTO(3, "Clavos", "Acero", "Aceros Manolo", 20, 20),
            };

            var herramientaDTOsTC1 = new List<HerramientaParaRepararDTO>() { herramientaDTOs[0], herramientaDTOs[1], herramientaDTOs[2] };
            var herramientaDTOsTC2 = new List<HerramientaParaRepararDTO>() { herramientaDTOs[1] };
            var herramientaDTOsTC3 = new List<HerramientaParaRepararDTO>() {herramientaDTOs[0],  herramientaDTOs[2] }; 
                

            var allTestsCases = new List<object[]>
            {   new object[] { null, null, null, null, herramientaDTOsTC1,  },
                new object[] { "Martillo", null, null, null, herramientaDTOsTC2, },
                new object[] { null, "Madera", null, null, herramientaDTOsTC3, },

            };

            return allTestsCases;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaReparar))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientaParaReparar_OK_test(string? nombre, float? tiempoReparacion,
            IList<HerramientaParaRepararDTO> expectedHerramientas)
        {
            // Arrange
            var controller = new HerramientaController(_context, Mock.Of<ILogger<HerramientaController>>());

            // Act
            var result = await controller.GetHerramientaParaRepararDTO(nombre, tiempoReparacion);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaRepararDTO>>(okResult.Value);
            Assert.Equal(expectedHerramientas, herramientaDTOsActual);

        }

    }
}