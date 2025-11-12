using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.ComprarDTOs;
using AppForSEII2526.API.DTO.HerramientaDTOs;
using AppForSEII2526.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprarItemController_Test
{
    public class CompraMétodoPost_test : AppForMovies4SqliteUT
    {
        public CompraMétodoPost_test()
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
            Compra compra = new Compra("Mi casa", DateTime.Now, 5, 33.70, new List<CompraItem>(), username);
            var comprasUsuario = new List<CompraItem>() { new CompraItem(20, "Muy grande", 4, 7, 200.88, compra, herramienta[1]) };

            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);
            _context.AddRange(comprasUsuario);
            _context.Add(username);
            _context.Add(compra);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CompraMétodoPost_OK()
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

            var micompra = new CompraForCreateDTO(1,"Destornillador fuerte","Acero",25.99,"Jacinta","De La Vega","Casa de Jacinta",PaymentMethodTypes.CreditCard, 647692817, "jacinta@gmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);
            var SinNombre = new CompraForCreateDTO(1, null, "Acero", 25.99, "Jacinta", "De La Vega", "Casa de Jacinta", PaymentMethodTypes.CreditCard, 647692817, "jacinta@gmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);
            var SinMaterial = new CompraForCreateDTO(1, "Destornillador fuerte", null, 25.99, "Jacinta", "De La Vega", "Casa de Jacinta", PaymentMethodTypes.CreditCard, 647692817, "jacinta@gmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);
            var SinPrecio = new CompraForCreateDTO(1, "Destornillador fuerte", "Acero", 0, "Jacinta", "De La Vega", "Casa de Jacinta", PaymentMethodTypes.CreditCard, 647692817, "jacinta@gmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);
            var SinNombreUsuario = new CompraForCreateDTO(1, "Destornillador fuerte", "Acero", 25.99, null, "De La Vega", "Casa de Jacinta", PaymentMethodTypes.CreditCard, 647692817, "jacinta@gmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);
            var SinApellido = new CompraForCreateDTO(1, "Destornillador fuerte", "Acero", 25.99, "Jacinta", null, "Casa de Jacinta", PaymentMethodTypes.CreditCard, 647692817, "jacinta@gmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);
            var SinDireccion = new CompraForCreateDTO(1, "Destornillador fuerte", "Acero", 25.99, "Jacinta", "De La Vega", null, PaymentMethodTypes.CreditCard, 647692817, "jacinta@gmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);

            var allTests = new List<object[]>
            {             //filters to apply - expected movies
                                          //by default datefrom=today +1, dateto=today+2, thus movieDTOs[0] cannot be returned
                new object[] { micompra, "Su compra se ha realizado correctamente." },
                new object[] { SinNombre, "Error. Introduzca el nombre de la herramienta que desea."},
                new object[] { SinMaterial, "Error. Introduzca el material de la herramienta que desea." },
                new object[] { SinPrecio, "Error. La compra no puede costar 0 euros sin códigos de descuento ni cheques de regalo." },
                new object[] { SinNombreUsuario, "Error. Por favor, introduzca su nombre."},
                new object[] { SinApellido, "Error. Por favor, introduzca sus apellidos." },
                new object[] { SinDireccion, "Error. Por favor, introduzca su dirección para recibir su compra."}
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CompraMétodoPost_OK))]
        public async Task ErrorCrearCompra_test(CompraForCreateDTO compraForCreateDTO, string expectedMessage)
        {
            // Arrange
            var morck = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = morck.Object;


            var controller = new ComprasController(_context, logger);

            // Act
            var result = await controller.CreateCompra(compraForCreateDTO);


            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(expectedMessage, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompraExitosa_test()
        {
            //Arrange
            var morck = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = morck.Object;

            var controller = new ComprasController(_context, logger);

            var compraItems = new List<CompraItemDTO>() { new CompraItemDTO("Martillo", "Piedra", 35.00, "Duro", 2, 10) };

            var compraDTO = new CompraForCreateDTO(2, "Destornillador flojo", "Metal", 40.99, "Manolo", "El de los Cimientos", "Casa de Manolo", PaymentMethodTypes.PayPal, 651967412, "manolo@hotmail.com", new List<CompraItemDTO>(), DateTime.Now, DateTime.Today);

            var expectedCompraDetailDTO = new CompraDetailDTO(5, "Antonia", "De La Torre", "Casa de Antonia", 66.78, DateTime.Now, new List<CompraItemDTO>());

            //Act
            var result = await controller.CreateCompra(compraDTO);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetailDTO = Assert.IsType<CompraDetailDTO>(createdResult.Value);

            Assert.Equal(expectedCompraDetailDTO, actualCompraDetailDTO);
        }


    }
}
