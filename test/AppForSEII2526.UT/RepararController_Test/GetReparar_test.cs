using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.RepararDTOs;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquilerController_Test
{
    public class GetReparar_test : AppForMovies4SqliteUT
    {
        public GetReparar_test()
        {

            var fabricante = new List<Fabricante>() {
                new Fabricante(1,"Alquileres Manolo",null),
                new Fabricante(2,"Cristales Joaquin", null),
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, null),
                new Herramienta(2, 10, "madera", "Martillo", 15, 30, null),
            };

            ApplicationUser user = new ApplicationUser("Endrino González", "miguel.endrino@alu.uclm.es", "Miguel", 123456789);

            var reparar = new Reparacion(1, "Miguel", "Endrino Gonzalez", DateTime.Today, DateTime.Today.AddDays(5), MetodosPago.TarjetaCredito, 123456789, 25);
            reparar.ReparacionItems.Add(new ReparacionItem(1, reparar.Id, herramienta[0].id, 5, 50, 10, "Herramienta Arreglada"));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.Add(reparar);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReparar_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            ILogger<ReparacionesController> logger = mock.Object;

            var controller = new ReparacionesController(_context, logger);

            // Act
            var result = await controller.GetReparacionDetail(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReparar_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            ILogger<ReparacionesController> logger = mock.Object;
            var controller = new ReparacionesController(_context, logger);




            var expectedReparar = new RepararDetailDTO(1, "Miguel", "Endrino Gonzalez", DateTime.Today, DateTime.Today.AddDays(5), null);

            //expectedReparar.ReparacionItems.Add(new ReparacionItem(1, expectedReparar.Id, 1, 5, 50, 10, "Herramienta Arreglada"));
            expectedReparar.ReparacionItems.Add(new RepararItemDTO(1, "Serrucho", 25, 1 , "Herramienta Arreglada"));

            // Act 
            var result = await controller.GetReparacionDetail(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var repararDTOActual = Assert.IsType<RepararDetailDTO>(okResult.Value);
            var eq = expectedReparar.Equals(repararDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedReparar, repararDTOActual);

        }
    }
}