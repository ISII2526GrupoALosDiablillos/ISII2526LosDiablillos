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
    public class GetHerramienta_test : AppForMovies4SqliteUT
    {
        public GetHerramienta_test()
        {

            var fabricantes = new List<Fabricante>() {
                new Fabricante(1,"Aceros manolo", null),
                new Fabricante(2,"Maderas Juan", null),
                new Fabricante(3,"Aluminios Carlos", null),
                new Fabricante(4,"Cristales Joaquin", null)
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, null),
                new Herramienta(2, 10, "madera", "Martillo", 15, 40, null),
                new Herramienta(3, 15, "acero", "Clavos", 20, 50, null),
                //this movie has quantityforpurchase=0 and quantityforrenting=0 so it shouldn't be returned when 
                //quering for movies for being purchased or rented
                new Herramienta(4, 20, "acero", "Desatornillador", 100, 100, null),
            };



            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);

            _context.SaveChanges();
        }


        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaAlquilar_OK()
        {

            var herraiemntaDTOs = new List<HerramientaParaAlquilarDTO>() {
                new HerramientaParaAlquilarDTO(1,"Serrucho", "aluminio", "Aluminios Manolo", 25),
                new HerramientaParaAlquilarDTO(2,"Martillo","madera", "Maderas Juan",15),
                new HerramientaParaAlquilarDTO(3, "Clavos", "Metal", "Aceros Manolo", 20),
            };

            var herramientaDTOsTC1 = new List<HerramientaParaAlquilarDTO>() { herraiemntaDTOs[1], herraiemntaDTOs[2] }
                    //the GetMoviesForPurchase method returns the movies ordered by title
                    .OrderBy(m => m.Material).ToList();


            var herramientaDTOsTC2 = new List<HerramientaParaAlquilarDTO>() { herraiemntaDTOs[1] };
            var herramientaDTOsTC3 = new List<HerramientaParaAlquilarDTO>() { herraiemntaDTOs[2] };

            var herramientaDTOsTC4 = new List<HerramientaParaAlquilarDTO>() { herraiemntaDTOs[0], herraiemntaDTOs[1], herraiemntaDTOs[2] }
                //the GetMoviesForPurchase method returns the movies ordered by title
                .OrderBy(m => m.Material).ToList();

            var allTests = new List<object[]>
            {             //filters to apply - expected movies
                                          //by default datefrom=today +1, dateto=today+2, thus movieDTOs[0] cannot be returned
                new object[] { null, null, null, null, herramientaDTOsTC1,  },
                new object[] { "mechanic", null, null, null, herramientaDTOsTC2, },
                new object[] { null, "Drama", null, null, herramientaDTOsTC3, },
                
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaAlquilar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientaParaAlquilar_OK_test(string? filterTitle, string? filterGenre,
            IList<HerramientaParaAlquilarDTO> expectedHerramientas)
        {
            // Arrange
            var controller = new HerramientaController(_context, null!);

            // Act
            var result = await controller.GetHerramientaParaAlquilarDTO(filterTitle, filterGenre);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaAlquilarDTO>>(okResult.Value);
            Assert.Equal(expectedHerramientas, herramientaDTOsActual);

        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetHerramientaParaAlquilar_badrequest_test()
        {
            // Arrange
            var mock = new Mock<ILogger<HerramientaController>>();
            ILogger<HerramientaController> logger = mock.Object;
            var controller = new HerramientaController(_context, logger);

            // Act
            var result = await controller.GetHerramientaParaAlquilarDTO(null, null);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("fromDate must be earlier than toDate", problem);
        }

    }
}