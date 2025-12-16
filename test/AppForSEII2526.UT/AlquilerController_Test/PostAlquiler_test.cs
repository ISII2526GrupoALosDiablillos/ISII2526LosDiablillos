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
    public class PostAlquiler_test : AppForMovies4SqliteUT
    {
        private const string _userName = "iker.valia@alu.uclm.es";
        private const string _customerNameSurname = "Iker Valia";
        private const string _deliveryAddress = "Avda. España s/n, Albacete 02071";

        private const string _herramienta1Nombre = "Serrucho";
        private const string _herramienta1Fabricante = "Aceros Manolo";
        private const string _herramienta2Nombre = "Martillo";
        private const string _herramienta2Fabricante = "Maderas Juan";

        public PostAlquiler_test()
        {

            var fabricante = new List<Fabricante>() {
                new Fabricante(1,_herramienta1Nombre, null),
                new Fabricante(2,_herramienta2Nombre, null),
            };

            var herramienta = new List<Herramienta>(){
                new Herramienta(1,5,_herramienta1Fabricante, _herramienta1Nombre, 25, 50, fabricante[0]),
                new Herramienta(2,10,_herramienta2Fabricante, _herramienta2Nombre, 20, 50, fabricante[1]),
            };

            ApplicationUser user = new ApplicationUser(_customerNameSurname, _userName, "", 675171341, null);

            var alquilar = new Alquilar(_customerNameSurname, "Garcia", _deliveryAddress, DateTime.Now, PaymentMethodTypes.PayPal, DateTime.Today.AddDays(2), DateTime.Today.AddDays(7), null, user);
            alquilar.alquilarItems.Add(new AlquilarItem(1, 25, alquilar, herramienta[0]));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.Add(alquilar);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateAlquilar()
        {
            var alquilerNoItem = new AlquilarForCreateDTO(_customerNameSurname,"",
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<AlquilarItemDTO>());

            var alquilarItem = new List<AlquilarItemDTO>() { new AlquilarItemDTO(1,"Martillo", "Acero",4,120) };

            var alquilerDesdeAyer= new AlquilarForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today, DateTime.Today.AddDays(5), alquilarItem);

            var alquilerAntesDe = new AlquilarForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), alquilarItem);

            var AlquilerApplicationUser = new AlquilarForCreateDTO("iker.valia@alu.uclm.es", _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), alquilarItem);

            var AlquilerHerramientaNoDisponible = new AlquilarForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                new List<AlquilarItemDTO>() { new AlquilarItemDTO(2, "Martillo", "Acero", 4, 120) });


            var allTests = new List<object[]>
            {             //input for createpurchase - Error expected
                new object[] { alquilerNoItem, "Error! Tu deberias incluir al menos una herramienta para ser alquilada",  },
                new object[] { alquilerDesdeAyer, "Error! Tu fecha para alquilar debe empezar mas tarde que hoy", },
                new object[] { alquilerAntesDe, "Error! Tu alquiler deberia acabar despues de empezar", },
                new object[] { AlquilerApplicationUser, "Error! Nombre de usuario no registrado", },
                new object[] { AlquilerHerramientaNoDisponible, "Error! La herramienta llamada 'Serrucho' no esta disponible para ser alquilada", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateAlquilar))]
        public async Task CreateAlquilar_Error_test(AlquilarForCreateDTO alquilerDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilarController>>();
            ILogger<AlquilarController> logger = mock.Object;

            var controller = new AlquilarController(_context, logger);

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

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
        public async Task CreateAlquilar_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilarController>>();
            ILogger<AlquilarController > logger = mock.Object;

            var controller = new AlquilarController(_context, logger);

            DateTime to = DateTime.Today.AddDays(2);
            DateTime from = DateTime.Today.AddDays(7);

            var alquilerDTO = new AlquilarForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                to, from, new List<AlquilarItemDTO>()
                { new AlquilarItemDTO(1,"Martillo", "Acero",4,120) });

            var expectedalquilerDetailDTO = new AlquilarDetailDTO(2, DateTime.Now,
                _customerNameSurname,"",
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                to, from, new List<AlquilarItemDTO>()
                { new AlquilarItemDTO(1,"Martillo", "Acero",4,120) });

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualAlquilerDetailDTO = Assert.IsType<AlquilarDetailDTO>(createdResult.Value);

            Assert.Equal(expectedalquilerDetailDTO, actualAlquilerDetailDTO);

        }

    }
}
