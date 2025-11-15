using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.HerramientaDTOs;
using AppForSEII2526.API.DTOs;
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
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, null),
                new Herramienta(2, 10, "madera", "Martillo", 15, 40, null),
                new Herramienta(3, 15, "acero", "Clavos", 20, 50, null),
                //this movie has quantityforpurchase=0 and quantityforrenting=0 so it shouldn't be returned when 
                //quering for movies for being purchased or rented
                new Herramienta(4, 20, "acero", "Desatornillador", 100, 100, null),
            };

            ApplicationUser username = new ApplicationUser("Ortiz", "gonzalo@alu.uclm.es", "Gonzalo", 684512269, new List<Compra>());
            Compra compra = new Compra("Mi casa",DateTime.Now,5,33.70, new List<CompraItem>(), username);

            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);
            _context.AddRange(compra);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetHerramientaParaComprar_badrequest_test()
        {
            // Arrange
            var mock = new Mock<ILogger<HerramientaController>>();
            ILogger<HerramientaController> logger = mock.Object;
            var controller = new HerramientaController(_context, logger);

            var compraReciente = new CompraDetailDTO(1, "Gonza", "Ortiz", "La ESII", 35.50, DateTime.Now, null);
            compraReciente.compraItems.Add(new CompraItemDTO("Metro", "Hierro", 20.00, "Máximo 500 cm", 1, 9));
            // Act
            var result = await controller.GetHerramientaParaComprarDTO(0, null);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("fromDate must be earlier than toDate", problem);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetCompraDetalle_NotFound_test()
        {
            //arange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            //act
            var result = await controller.GetCompraDetails();

            //assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}