using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.ComprarDTOs;
using AppForSEII2526.API.DTOs;

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
                new Herramienta(1,5,"aluminio", "Serrucho", 25, 50, fabricantes[0]),
                new Herramienta(2,10,"madera", "Martillo", 15, 40, fabricantes[1]),
                new Herramienta(3,15,"acero", "Clavos", 20, 50, fabricantes[2]),
                new Herramienta(4,20,"acero", "Desatornillador", 100, 100, fabricantes[3])
            };

            var username = new ApplicationUser(
                apellidoCliente: "Ortiz",
                correoElectronico: "gonzalo@alu.uclm.es",
                nombreCliente: "Gonzalo",
                telefono: 684512269)
            {
                nombreCliente = "Gonzalo",
                apellidoCliente = "Ortiz",
                Email = "gonzalo@alu.uclm.es",
                PhoneNumber = "684512269",
                UserName = "gonormu"
            };

            var compra = new Compra("Mi casa", DateTime.Today, 33.70, 0, new List<CompraItem>(), username)
            {
                atributos = username,
                compraItem = new List<CompraItem>()
            };

            compra.compraItem.Add(new CompraItem(20, "Muy afilados", 3, 3, 400));

            var usuario = new ApplicationUser(
                apellidoCliente: "Ortiz",
                correoElectronico: "gonzalo@alu.uclm.es",
                nombreCliente: "Gonzalo",
                telefono: 684512269)
            {
                UserName = "gonormu",
                Email = "gonzalo@alu.uclm.es",
                PhoneNumber = "684512269"
            };

            _context.ApplicationUsers.Add(usuario);

            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);
            _context.Add(username);
            _context.Add(compra);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> TestCasesFor_CompraMétodoPost_OK()
        {
            var micompra = new CompraForCreateDTO("gonormu", "Gonzalo", "Ortiz", "Mi casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400, new List<CompraItemDTO>());
            micompra.CompraItems.Add(new CompraItemDTO("Clavos", "acero", 20, "Muy afilados", 20, 3, 3));
            var herramientasParaComprar = micompra.CompraItems;

            var SinHerramientas = new CompraForCreateDTO("gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400, new List<CompraItemDTO>());
            var SinNombre = new CompraForCreateDTO("gonormu", null, "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400, herramientasParaComprar);
            var SinApellido = new CompraForCreateDTO("gonormu", "Gonzalo", null, "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400, herramientasParaComprar);
            var SinDirección = new CompraForCreateDTO("gonormu", "Gonzalo", "Ortiz", null, PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400, herramientasParaComprar);
            var CantidadInaceptable = new CompraForCreateDTO("gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400, new List<CompraItemDTO>() { new CompraItemDTO("Clavos", "Acero", 20, "Muy afilados", 0, 3, 3)});
            var HerramientaInexistente = new CompraForCreateDTO("gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 35, new List<CompraItemDTO>() { new CompraItemDTO("Motosierra", "Hierro", 35, "Nueva herramienta", 1, 5, 3) });
            var MuchasHerramientas = new CompraForCreateDTO("gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400, new List<CompraItemDTO>() { new CompraItemDTO("Clavos", "Acero", 20, "", 3, 3, 4) });
            var Metalico = new CompraForCreateDTO("gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.Cash, 684512269, "gonzalo@alu.uclm.es", 400, herramientasParaComprar);


            return new List<object[]>
            {
                new object[] {SinHerramientas, "Error: No se puede comprar cero herramientas..."},
                new object[] {SinNombre, "Error. Introduzca su nombre."},
                new object[] {SinApellido, "Error. Introduzca su apellido."},
                new object[] {SinDirección, "Error. Introduzca su dirección."},
                new object[] {CantidadInaceptable, "Error: La cantidad debe ser superior a cero."},
                new object[] {HerramientaInexistente, "Error: La herramienta no está disponible."},
                new object[] {MuchasHerramientas, "¡Error! Estas comprando demasiadas herramientas sin descripción."},
                new object[] {Metalico, "¡Error! No aceptamos compras pagadas en metálico." }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_CompraMétodoPost_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task ErrorCrearCompra_test(CompraForCreateDTO compraForCreateDTO, string expectedMessage)
        {
            var morck = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = morck.Object;
            var controller = new ComprasController(_context, logger);

            var result = await controller.CreateCompra(compraForCreateDTO);

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
            var morck = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = morck.Object;
            var controller = new ComprasController(_context, logger);

            var compraDTO = new CompraForCreateDTO(
                "gonormu", "Gonzalo", "Ortiz", "Mi casa",
                PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", 400,
                new List<CompraItemDTO> {
                    new CompraItemDTO("Clavos", "acero", 400, "Muy afilados", 20,3, 3)
                }
            );

            var expectedDTODetailcompra = new CompraDetailDTO(2, "Gonzalo", "Ortiz", "Mi casa", 400, DateTime.Today, new List<CompraItemDTO> { new CompraItemDTO("Clavos", "acero", 400, "Muy afilados", 20, 3, 3) } );

            var result = await controller.CreateCompra(compraDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetailDTO = Assert.IsType<CompraDetailDTO>(createdResult.Value);

            Assert.Equal(expectedDTODetailcompra, actualCompraDetailDTO);
        }
    }
}
