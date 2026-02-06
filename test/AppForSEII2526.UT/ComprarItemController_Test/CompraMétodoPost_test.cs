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
                telefono: 684512269,
                compras: new List<Compra>())
            {
                nombreCliente = "Gonzalo",
                apellidoCliente = "Ortiz",
                Email = "gonzalo@alu.uclm.es",
                PhoneNumber = "684512269",
                UserName = "gonormu"
            };

            var compra = new Compra("Mi casa", DateTime.Today, 5, 33.70, new List<CompraItem>(), username)
            {
                atributos = username,
                compraItem = new List<CompraItem>()
            };

            compra.compraItem.Add(new CompraItem(20, "Muy afilados", 5, 3, 400));

            _context.AddRange(fabricantes);
            _context.AddRange(herramienta);
            _context.Add(username);
            _context.Add(compra);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> TestCasesFor_CompraMétodoPost_OK()
        {
            var micompra = new CompraForCreateDTO(4, /*"Clavos", "acero", 20,*/ "gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", new List<CompraItemDTO>(), DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            micompra.CompraItems.Add(new CompraItemDTO("Clavos", "acero", 20, "Muy afilados", 20, "Makita", 3, 3));
            var CompraItems = micompra.CompraItems;
            /*var SinNombreHerr = new CompraForCreateDTO(4, null, "acero", 20, "gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var SinMaterial = new CompraForCreateDTO(4, "Clavos", null, 20, "gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var SinPrecio = new CompraForCreateDTO(4, "Clavos", "acero", 0, "gonormu","Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));*/
            var SinApellido = new CompraForCreateDTO(4, /*"Clavos", "acero", 20,*/ "gonormu", "Gonzalo", null, "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var SinDireccion = new CompraForCreateDTO(4, /*"Clavos", "acero", 20,*/ "gonormu", "Gonzalo", "Ortiz", null, PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var SinNombreUsuario = new CompraForCreateDTO(4, /*"Clavos", "acero", 20,*/ null, "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var SinNombre = new CompraForCreateDTO(4, /*"Clavos", "acero", 20,*/ "gonormu", null, "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var MuchasHerramientas = new CompraForCreateDTO(4, /*"Clavos", "acero", 20,*/ "gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es", new List<CompraItemDTO>() { new CompraItemDTO("Clavos", "acero", 20, "", 3, "Makita",3, 4) }, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var Metalico = new CompraForCreateDTO(4, /*"Clavos", "acero", 20,*/ "gonormu", "Gonzalo", "Ortiz", "Mi Casa", PaymentMethodTypes.Cash, 684512269, "gonzalo@alu.uclm.es", CompraItems, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));

            return new List<object[]>
            {
                /*new object[] { SinNombreHerr, "Error. Introduzca el nombre de la herramienta que desea." },
                new object[] { SinMaterial, "Error. Introduzca el material de la herramienta que desea." },
                new object[] { SinPrecio, "Error. La compra no puede costar 0 euros sin códigos de descuento ni cheques de regalo." },*/
                new object[] { SinApellido, "Error. Apellido no registrado." },
                new object[] { SinDireccion, "Error. Dirección no registrada." },
                new object[] { SinNombreUsuario, "Error. Nombre de usuario no registrado." },
                new object[] { SinNombre, "Error. Nombre no registrado." },
                new object[] { MuchasHerramientas, "¡Error! Estas comprando demasiadas herramientas sin descripción." },
                new object[] { Metalico, "¡Error! No aceptamos compras pagadas en metálico." }
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

            if (expectedMessage.StartsWith("Su compra se ha realizado correctamente"))
            {
                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                var actualCompraDetailDTO = Assert.IsType<CompraDetailDTO>(createdResult.Value);
                Assert.Equal(compraForCreateDTO.FechaCompra, actualCompraDetailDTO.fechaCompra);
                Assert.Equal(compraForCreateDTO.Nombre_cliente, actualCompraDetailDTO.nombre_cliente);
                Assert.Equal(compraForCreateDTO.Apellidos_cliente, actualCompraDetailDTO.apellido_cliente);
                Assert.Equal(compraForCreateDTO.Id, actualCompraDetailDTO.id);
                Assert.Equal(compraForCreateDTO.DireccionEnvio, actualCompraDetailDTO.direccion);
                /*Assert.Equal(compraForCreateDTO.Precio, actualCompraDetailDTO.preciototal);*/
                Assert.Single(actualCompraDetailDTO.compraItems);
                var item = actualCompraDetailDTO.compraItems[0];
                Assert.Equal(3, item.cantidad);
                Assert.Equal("", item.descripcion);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
                var errorActual = problemDetails.Errors.First().Value[0];

                Assert.StartsWith(expectedMessage, errorActual);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompraExitosa_test()
        {
            var usuario = new ApplicationUser(
                apellidoCliente: "Ortiz",
                correoElectronico: "gonzalo@alu.uclm.es",
                nombreCliente: "Gonzalo",
                telefono: 684512269,
                compras: new List<Compra>())
            {
                UserName = "gonormu",
                Email = "gonzalo@alu.uclm.es",
                PhoneNumber = "684512269"
            };

            _context.ApplicationUsers.Add(usuario);

            var herramientaExistente = _context.Herramientas.Local.FirstOrDefault(h => h.id == 3);
            if (herramientaExistente == null)
            {
                var herramienta = new Herramienta
                {
                    id = 3,
                    nombre = "Clavos",
                    material = "acero",
                    precio = 20
                };
                _context.Herramientas.Add(herramienta);
            }

            await _context.SaveChangesAsync();

            var morck = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = morck.Object;
            var controller = new ComprasController(_context, logger);

            var compraDTO = new CompraForCreateDTO(
                4, /*"Clavos", "acero", 400,*/ "gonormu", "Gonzalo", "Ortiz", "Mi casa",
                PaymentMethodTypes.CreditCard, 684512269, "gonzalo@alu.uclm.es",
                new List<CompraItemDTO> {
                    new CompraItemDTO("Clavos", "acero", 400, "Muy afilados", 20, "Makita",3, 3)
                },
                DateTime.Today.AddDays(1), DateTime.Today.AddDays(2)
            );

            var result = await controller.CreateCompra(compraDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetailDTO = Assert.IsType<CompraDetailDTO>(createdResult.Value);

            Assert.Equal(compraDTO.FechaCompra, actualCompraDetailDTO.fechaCompra);
            Assert.Equal(compraDTO.Nombre_cliente, actualCompraDetailDTO.nombre_cliente);
            Assert.Equal(compraDTO.Apellidos_cliente, actualCompraDetailDTO.apellido_cliente);
            Assert.Equal(compraDTO.DireccionEnvio, actualCompraDetailDTO.direccion);
            Assert.Single(actualCompraDetailDTO.compraItems);
            var item = actualCompraDetailDTO.compraItems[0];
            Assert.Equal("Clavos", item.nombre);
            Assert.Equal("acero", item.material);
            Assert.Equal("Muy afilados", item.descripcion);
            Assert.Equal(3, item.herramientaId);
            Assert.Equal(20, item.cantidad);
            Assert.Equal(20, item.precio);
        }
    }
}
