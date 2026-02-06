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

        private const string herramientaId = "4";
        private const string herramientaNombre = "Alicates";
        private const string herramientaMaterial = "Metal";
        private const string herramientaPrecio = "25";
        private const string herramientaFabricante = "Sergio";

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

        //Flujo Básico (FB)
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
                new string[] { herramientaNombre, herramientaMaterial, "1", "5 €", "Duro" }

            };

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramientaNombre);
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

        //FB + FA1
        //Flujo Alternativo 1 al Paso 2: Filtrar herramientas.
        [Theory]
        [InlineData(herramientaId, herramientaNombre, herramientaMaterial, herramientaPrecio, herramientaFabricante, "Metal", "")]
        [InlineData(herramientaId, herramientaNombre, herramientaMaterial, herramientaPrecio, herramientaFabricante, "", "25")]
        [InlineData(herramientaId, herramientaNombre, herramientaMaterial, herramientaPrecio, herramientaFabricante, "Metal", "25")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA1_filtroMaterialPrecio(string herramientaId, string herramientaNombre, string herramientaMaterial, string herramientaPrecio, string herramientaFabricante, string filtroMaterial, string filtroPrecio)
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

        //FB + FA2
        //Flujo Alternativo 2 al Paso 5: Modificar Carrito.
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA2_ModificarCarritoCompra()
        {
            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "0");
            Thread.Sleep(500);

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramientaNombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramientaNombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.ModificarCompra();
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.QuitarHerramientaFromCart(herramientaNombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            var expectedHerramientas = new List<string[]>
            {
            new[]
              {
                 $"{herramientaNombre} {herramientaMaterial}{Environment.NewLine}{herramientaPrecio}"
                }
            };

            Assert.True(crearCompra_PO.CheckListHerramientasEnCompra(expectedHerramientas));



        }

        //FB + FA3
        //Flujo Alternativo 3 al Paso 4: No hay herramientas en el carrito.
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA3_NotHerramientas()
        {

            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);

            //Act
            Thread.Sleep(500);

            //Assert
            Assert.True(selectHerramientasParaComprar_PO.CompraNotAvailable());
        }


        //FB + FA4
        //Flujo Alternativo 4 al Paso 6: Campos Obligatorios Vacíos.
        [Theory]
        [InlineData("", "Ortiz", "Mi Casa", "PayPal", "Nombre")]
        [InlineData("Gonzalo", "", "Mi Casa", "PayPal", "Apellido")]
        [InlineData("Gonzalo", "Ortiz", "", "PayPal", "Direccion")]
        [InlineData("Gonzalo", "Ortiz", "Mi Casa", "", "MetodoPago")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_Esc6_FA5_camposNoValidos(string nombre, string apellido, string direccion, string metodoPago, string error)
        {

            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "0");
            Thread.Sleep(500);

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramientaNombre);
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

        //FB + FA5
        //Flujo Alternativo 5 al Paso 6: No hay herramientas en Stock.
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_Esc6_FA5_noHayCantidad()
        {

            //Arrange
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);

            //Act
            selectHerramientasParaComprar_PO.AñadirHerramientaToCart(herramientaNombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.RellenarFormulario("Gonzalo", "Ortiz", "gonzalo@gmail.com", "Mi Casa", "PayPal", "Duro");
            Thread.Sleep(500);
            crearCompra_PO.EstablecerCantidadPorNombre(herramientaNombre, "0");
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