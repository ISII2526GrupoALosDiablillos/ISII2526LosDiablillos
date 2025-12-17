using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_ComprarHerramientas
{
    public class CU_ComprarHerramientas_UIT : UC_UIT
    {

        private SelectHerramientaForCompra_PO selectHerramientasParaComprar_PO;
        private CompraCreate_PO crearCompra_PO;
        private CompraDetails_PO detalleCompra_PO;

        private const string herramienta1Id = "3";
        private const string herramienta1Nombre = "Martillo";
        private const string herramienta1Material = "Madera";
        private const string herramienta1Precio = "20";
        private const string herramienta1Fabricante = "Makita";


        private const string herramienta2Id = "4";
        private const string herramienta2Nombre = "Destornillador";
        private const string herramienta2Material = "Metal";
        private const string herramienta2Precio = "15";
        private const string herramienta2Fabricante = "Bosch";

        public CU_ComprarHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            selectHerramientasParaComprar_PO = new SelectHerramientaForCompra_PO(_driver, _output);
            crearCompra_PO = new CompraCreate_PO(_driver, _output);
            detalleCompra_PO = new CompraDetails_PO(_driver, _output);
        }


        private void InitialStepsForCompraHerramientas()
        {
            Initial_step_opening_the_web_page();

            By id = By.Id("CrearCompra");
            selectHerramientasParaComprar_PO.WaitForBeingVisible(id);

            Thread.Sleep(500);
            _driver.FindElement(id).Click();
        }


        [Theory]
        [InlineData(herramienta1Id, herramienta1Nombre, herramienta1Material, herramienta1Precio, herramienta1Fabricante, "Madera", "")]
        [InlineData(herramienta2Id, herramienta2Nombre, herramienta2Material, herramienta2Precio, herramienta2Fabricante, "", "15")]
        [InlineData(herramienta2Id, herramienta2Nombre, herramienta2Material, herramienta2Precio, herramienta2Fabricante, "Metal", "15")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_3_AFO_filteringPorMaterialPrecio(string herramientaId, string herramientaNombre, string herramientaMaterial, string herramientaPrecio, string herramientaFabricante, string filtroMaterial, string filtroPrecio)
        {
            //Arrange
            InitialStepsForCompraHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { herramientaNombre, herramientaMaterial, herramientaPrecio, herramientaFabricante, "Añadir" }, };

            //Act
            selectHerramientasParaComprar_PO.BuscarHerramientas(filtroMaterial, filtroPrecio);
            Thread.Sleep(500);

            //Assert
            Assert.True(selectHerramientasParaComprar_PO.CheckListOfHerramientasInCart(expectedHerramientas));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_5_FA2_ModificarCarritoCompra()
        {
            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "0");
            Thread.Sleep(500);

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramienta2Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.ModificarCompra();
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.QuitarHerramientaFromCart(herramienta2Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            var expectedHerramientas = new List<string[]>
            {
            new[]
              {
                 $"{herramienta1Nombre} {herramienta1Material}{Environment.NewLine}{herramienta1Precio}"
                }
            };

            Assert.True(crearCompra_PO.CheckListHerramientasEnCompra(expectedHerramientas));



        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_4_FA3_NotHerramientas()
        {

            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "0");
            Thread.Sleep(500);

            //Act
            Thread.Sleep(500);

            //Assert
            Assert.True(selectHerramientasParaComprar_PO.CompraNotAvailable());
        }

        //Flujo Básico
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FB_CompraHerramienta()
        {
            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "0");
            Thread.Sleep(500);
            var expectedHerramientas = new List<string[]>
            {
                new string[] { herramienta1Nombre, herramienta1Material, "1", "5 €", "Duro" }

            };

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.RellenarFormulario("Gonzalo", "Ortiz", "gonzalo@gmail.com", "Mi Casa", "PayPal", "Duro");
            Thread.Sleep(500);
            crearCompra_PO.SubmitCompra();
            Thread.Sleep(500);
            crearCompra_PO.ConfirmCompra();
            Thread.Sleep(500);

            //Assert

            Assert.True(detalleCompra_PO.CheckDetalleCompra("Gonzalo", "Ortiz", "Mi Casa", DateTime.Today, 5), "Falla CheckDetalleCompra");
            Assert.True(detalleCompra_PO.CheckListHerramienta(expectedHerramientas), "Falla CheckListHerramienta");


        }

        [Theory]
        [InlineData("", "Ortiz", "Mi Casa", "PayPal", "Nombre")]
        [InlineData("Gonzalo", "", "Mi Casa", "PayPal", "Apellido")]
        [InlineData("Gonzalo", "Ortiz", "", "PayPal", "Direccion")]
        [InlineData("Gonzalo", "Ortiz", "Mi Casa", "", "MetodoPago")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_6_FA5_camposNoValidos(string nombre, string apellido, string direccion, string metodoPago, string error)
        {

            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "0");
            Thread.Sleep(500);

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.RellenarFormulario(nombre, apellido, correo: "gonzalo@gmail.com", direccion, metodoPago, "Duro");
            Thread.Sleep(500);
            crearCompra_PO.SubmitCompra();
            Thread.Sleep(500);

            //Assert
            Assert.True(crearCompra_PO.CheckError(error));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_6_FA5_noHayCantidad()
        {

            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "0");
            Thread.Sleep(500);

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.RellenarFormulario("Gonzalo", "Ortiz", "gonzalo@gmail.com", "Mi Casa", "PayPal", "Duro");
            Thread.Sleep(500);
            crearCompra_PO.EstablecerCantidadPorNombre(herramienta1Nombre, "0");
            Thread.Sleep(500);
            crearCompra_PO.SubmitCompra();
            Thread.Sleep(500);
            crearCompra_PO.ConfirmCompra();
            Thread.Sleep(500);

            _output.WriteLine("URL actual: " + _driver.Url);

            //Assert
            Assert.True(crearCompra_PO.CheckError("Cantidad"));



        }
    }
}