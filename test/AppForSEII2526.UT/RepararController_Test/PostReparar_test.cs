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

namespace AppForSEII2526.UT.RepararController_Test
{
    public class PostReparar_test : AppForMovies4SqliteUT
    {
        private const string _userName = "miguel.endrino@alu.uclm.es";
        private const string _customerNameSurname = "Miguel";
        private const string _deliveryAddress = "Calle D, Albacete 02008";

        private const string _herramienta1Nombre = "Serrucho";
        private const string _herramienta1Fabricante = "Aceros Manolo";
        private const string _herramienta2Nombre = "Martillo";
        private const string _herramienta2Fabricante = "Maderas Juan";

        public PostReparar_test()
        {

            var fabricante = new List<Fabricante>() {
                new Fabricante(1,_herramienta1Nombre, null),
                new Fabricante(2,_herramienta2Nombre, null),
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,_herramienta1Fabricante, _herramienta1Nombre, 25, 50, null),
                new Herramienta(2,10,_herramienta2Fabricante, _herramienta2Nombre, 20, 50, null),
            };

            ApplicationUser user = new ApplicationUser(_customerNameSurname, "Miguel", _userName, 123456789, null);

            var reparar = new Reparacion(1, _customerNameSurname, "Endrino", DateTime.Now, DateTime.Today.AddDays(7), 
                MetodosPago.PayPal, 123456789, 50);
            reparar.ReparacionItems.Add(new ReparacionItem(1, reparar.Id, herramienta[0].id, 1, 25, 30, "Hecho"));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.Add(reparar);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateReparar()
        {
            var repararNoItem = new ReparacionForCreateDTO(_customerNameSurname,"Endrino Gonzalez",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RepararItemDTO>());

            var repararItem = new List<RepararItemDTO>() { new RepararItemDTO(1, "taladro", 59, 1) };

            var repararDesdeAyer = new ReparacionForCreateDTO(_customerNameSurname, "Endrino Gonzalez",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RepararItemDTO>());

            var repararAntesDe = new ReparacionForCreateDTO(_customerNameSurname, "Endrino Gonzalez",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RepararItemDTO>());

            var RepararApplicationUser = new ReparacionForCreateDTO(_customerNameSurname, "Endrino Gonzalez",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RepararItemDTO>());

            var RepararHerramientaNoDisponible = new ReparacionForCreateDTO(_customerNameSurname, "Endrino Gonzalez",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                new List<RepararItemDTO>() { new RepararItemDTO(2, "destornillador", 59, 2) });


            var allTests = new List<object[]>
            {             //input for createpurchase - Error expected
                new object[] { repararNoItem, "Error! Debes incluir al menos una herramienta para reparar.",  },
                new object[] { repararDesdeAyer, "Error! Tu Fecha de Reparacion debe empezar más tarde de hoy.", },
                new object[] { repararAntesDe, "Error! Tu reparación debe terminar después de que empiece.", },
                new object[] { RepararApplicationUser, "Error! El Nombre de Usuario no está registrado.", },
                new object[] { RepararHerramientaNoDisponible, "Error! La herramienta '' no está disponible para ser reparado.", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReparar))]
        public async Task CreateReparar_Error_test(ReparacionForCreateDTO repararDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            ILogger<ReparacionesController> logger = mock.Object;

            var controller = new ReparacionesController(_context, logger);

            // Act
            var result = await controller.CreateReparacion(repararDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReparar_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            ILogger<ReparacionesController> logger = mock.Object;

            var controller = new ReparacionesController(_context, logger);

            DateTime to = DateTime.Today.AddDays(2);
            DateTime from = DateTime.Today.AddDays(7);

            var repararDTO = new ReparacionForCreateDTO(_customerNameSurname, "Endrino Gonzalez",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RepararItemDTO>());
                new RepararItemDTO(3, "serrucho", 20, 3);

            var expectedrepararDetailDTO = new RepararDetailDTO(2, _customerNameSurname, "Endrino", 
                to, from, new List<RepararItemDTO>(){ new RepararItemDTO(4,"martillo",50,4) });

            // Act
            var result = await controller.CreateReparacion(repararDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRepararDetailDTO = Assert.IsType<RepararDetailDTO>(createdResult.Value);

            Assert.Equal(expectedrepararDetailDTO, actualRepararDetailDTO);

        }

    }
}