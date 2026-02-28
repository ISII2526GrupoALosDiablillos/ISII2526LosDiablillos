using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.AlquilarDTOs;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquilerController_Test
{
    public class GetAlquiler_test : AppForMovies4SqliteUT
    {
        public GetAlquiler_test()
        {

            var fabricante = new List<Fabricante>() {
                new Fabricante(1,"Alquileres Manolo",null),
                new Fabricante(2,"Cristales Joaquin", null),
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, fabricante[0]),
                new Herramienta(2, 10, "madera", "Martillo", 15, 30, fabricante[1]),
            };

            ApplicationUser user = new ApplicationUser("Valia Garcia", "ikervalia@alu.uclm.es", "Iker", 675171341);

            Alquilar alquilar = new Alquilar("Iker", "Valia Garcia", "calle x", DateTime.Today, PaymentMethodTypes.PayPal, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<AlquilarItem>(), user)
            {
                applicationUser = user,
                alquilarItems = new List<AlquilarItem>()
            };
            alquilar.alquilarItems.Add(new AlquilarItem(50, 25, alquilar, herramienta[0]));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.Add(alquilar);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAlquiler_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilarController>>();
            ILogger<AlquilarController> logger = mock.Object;

            var controller = new AlquilarController(_context, logger);

            // Act
            var result = await controller.GetAlquilar(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetAlquiler_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilarController>>();
            ILogger<AlquilarController> logger = mock.Object;
            var controller = new AlquilarController(_context, logger);


            var expectedAlquilar = new AlquilarDetailDTO(3, DateTime.Now, "iker.valia@alu.uclm.es", "Iker valia",
                        "Avda. España s/n, Albacete 02071", PaymentMethodTypes.CreditCard,
                        DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                        new List<AlquilarItemDTO>());
            expectedAlquilar.AlquilarItem.Add(new AlquilarItemDTO(1,"Martillo","madera",15,30));

            // Act 
            var result = await controller.GetAlquilar(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var alquilerDTOActual = Assert.IsType<AlquilarDetailDTO>(okResult.Value);
            var eq = expectedAlquilar.Equals(alquilerDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedAlquilar, alquilerDTOActual);

        }
    }
}
