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

        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaComprar_OK()
        {
            var fabricantes = new List<Fabricante>() {
                new Fabricante(1,"Aceros manolo", null),
                new Fabricante(2,"Maderas Juan", null),
                new Fabricante(3,"Aluminios Carlos", null),
                new Fabricante(4,"Cristales Joaquin", null)
            };

            var herraiemntaDTOs = new List<HerramientaParaComprarDTO>() {
                new HerramientaParaComprarDTO(1,"Maza", "aluminio", fabricantes[0].Nombre, 25),
                new HerramientaParaComprarDTO(2,"Destornillador","hierro", fabricantes[1].Nombre,15),
                new HerramientaParaComprarDTO(3, "Sierra", "Metal", fabricantes[2].Nombre, 20)
            };

            var herramientaDTOsTC1 = new List<HerramientaParaComprarDTO>() { herraiemntaDTOs[1], herraiemntaDTOs[2] }
                    //the GetMoviesForPurchase method returns the movies ordered by title
                    .OrderBy(m => m.id).ToList();


            var herramientaDTOsTC2 = new List<HerramientaParaComprarDTO>() { herraiemntaDTOs[1] };
            var herramientaDTOsTC3 = new List<HerramientaParaComprarDTO>() { herraiemntaDTOs[2] };

            var herramientaDTOsTC4 = new List<HerramientaParaComprarDTO>() { herraiemntaDTOs[0], herraiemntaDTOs[1], herraiemntaDTOs[2] }
                //the GetMoviesForPurchase method returns the movies ordered by title
                .OrderBy(m => m.id).ToList();

            var allTests = new List<object[]>
            {             //filters to apply - expected movies
                                          //by default datefrom=today +1, dateto=today+2, thus movieDTOs[0] cannot be returned
                new object[] { null, null, null, null, herramientaDTOsTC1,  },
                new object[] { "Alicates", null, null, null, herramientaDTOsTC2, },
                new object[] { null, "Madera", null, null, herramientaDTOsTC3, },
                new object[] { null, null, 20.22, null, herramientaDTOsTC4, }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaComprar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientaParaComprar_OK_test(int filterPrice, string? filterMaterial,
            IList<HerramientaParaComprarDTO> expectedHerramientas)
        {
            // Arrange
            var controller = new HerramientaController(_context, null!);

            // Act
            var result = await controller.GetHerramientaParaComprarDTO(filterPrice, filterMaterial);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaComprarDTO>>(okResult.Value);
            Assert.Equal(expectedHerramientas, herramientaDTOsActual);

        }
    }
}