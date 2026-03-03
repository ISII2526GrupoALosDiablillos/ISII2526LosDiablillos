using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.Shared;
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

        private const string herramienta1Id = "6";
        private const string herramienta1Nombre = "Sierra";
        private const string herramienta1Material = "Acero";
        private const string herramienta1Precio = "20";
        private const string herramienta1Fabricante = "Stanley";

        private const string herramienta2Id = "4";
        private const string herramienta2Nombre = "Astillas";
        private const string herramienta2Material = "Madera";
        private const string herramienta2Precio = "5";
        private const string herramienta2Fabricante = "Milwaukee";

        public CU_ComprarHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            selectHerramientasParaComprar_PO = new SelectHerramientaForCompra_PO(_driver, _output);
            crearCompra_PO = new CompraCreate_PO(_driver, _output);
            detalleCompra_PO = new CompraDetails_PO(_driver, _output);
        }


        private void InitialStepsForCompraHerramientas()
        {
            Initial_step_opening_the_web_page();

            selectHerramientasParaComprar_PO.WaitForBeingVisible(By.Id("CrearCompra"));

            Thread.Sleep(500);

            _driver.FindElement(By.Id("CrearCompra")).Click();
        }

        //Flujo Básico (FB)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FB_CompraHerramienta()
        {
            //Arrange
            var expectedHerramienta = new List<string[]>
            {
                new string[] {herramienta1Nombre, herramienta1Material, "1", "20 €", "Afilada"}
            };

            //Act
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.AñadirHerramientaACarrito(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.RellenarFormulario("Gonza", "Ortiz", "gonzalo@alu.uclm.es", "Mi Casa", "PayPal", "Afilada");
            Thread.Sleep(500);
            crearCompra_PO.EstablecerCantidadPorNombre(herramienta1Nombre, "1");
            Thread.Sleep(500);
            crearCompra_PO.PresentarCompra();
            Thread.Sleep(500);
            crearCompra_PO.ConfirmarCompra();
            Thread.Sleep(500);

            //Assert
            Assert.True(detalleCompra_PO.ComprobarDetallesDeCompra("Gonza", "Ortiz", "Mi Casa", DateTime.Today, 20), "Falla CheckDetalleCompra");
            Assert.True(detalleCompra_PO.ComprobarListaDeHerramienta(expectedHerramienta), "Falla CheckListHerramienta");


        }

        //FB + FA1
        //Flujo Alternativo 1 al Paso 2: Filtrar herramientas.
        [Theory]
        [InlineData(herramienta1Id, herramienta1Nombre, herramienta1Material, herramienta1Precio, herramienta1Fabricante, "Acero", "")]
        [InlineData(herramienta2Id, herramienta2Nombre, herramienta2Material, herramienta2Precio, herramienta2Fabricante, "", "5")]
        [InlineData(herramienta2Id, herramienta2Nombre, herramienta2Material, herramienta2Precio, herramienta2Fabricante, "Madera", "5")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA1_filtroMaterialPrecio(string herramientaId, string herramientaNombre, string herramientaMaterial, string herramientaPrecio, string herramientaFabricante, string filtroMaterial, string filtroPrecio)
        {
            //Arrange
            var expectedHerramientas = new List<string[]> { new string[] { herramientaId, herramientaNombre, herramientaMaterial, herramientaPrecio, herramientaFabricante, "Añadir" }, };

            //Act
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas(filtroMaterial, filtroPrecio);
            Thread.Sleep(500);

            //Assert
            Assert.True(selectHerramientasParaComprar_PO.ListaDeHerramientasDelCarrito(expectedHerramientas));
        }

        //FB + FA2
        //Flujo Alternativo 2 al Paso 5: Modificar Carrito.
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA2_ModificarCarritoCompra()
        {
            //Act
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.AñadirHerramientaACarrito(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.AñadirHerramientaACarrito(herramienta2Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.ModificarCompra();
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.QuitarHerramientaDeCarrito(herramienta2Nombre);
            Thread.Sleep(500);

            Assert.True(selectHerramientasParaComprar_PO.CarritoEstáVacio(),
                "Error: La cesta de la compra está vacía.");
        }

        //FB + FA3
        //Flujo Alternativo 3 al Paso 4: No hay herramientas en el carrito.
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FA3_NotHerramientas()
        {

            //Act
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);

            //Assert
            Assert.True(selectHerramientasParaComprar_PO.CompraNoDisponible());
        }


        //FB + FA4
        //Flujo Alternativo 4 al Paso 6: Campos Obligatorios Vacíos.
        [Theory]
        [InlineData("", "Ortiz", "Mi Casa", "PayPal", "El Nombre del cliente es obligatorio.")]
        [InlineData("Gonza", "", "Mi Casa", "PayPal", "El Apellido del cliente es obligatorio.")]
        [InlineData("Gonza", "Ortiz", "", "PayPal", "La dirección de envío es obligatoria.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_Esc6_FA5_camposNoValidos(string nombre, string apellido, string direccion, string metodoPago, string error)
        {

            //Act
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.AñadirHerramientaACarrito(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.RellenarFormulario(nombre, apellido, correo: "gonzalo@alu.uclm.es", direccion, metodoPago, "Afilada");
            Thread.Sleep(500);
            crearCompra_PO.EstablecerCantidadPorNombre(herramienta1Nombre, "1");
            Thread.Sleep(500);
            crearCompra_PO.PresentarCompra();
            Thread.Sleep(500);
            crearCompra_PO.ConfirmarCompra();
            Thread.Sleep(500);

            //Assert
            Assert.True(crearCompra_PO.ComprobarError(error));

        }

        //FB + FA5
        //Flujo Alternativo 5 al Paso 6: No hay herramientas en Stock.
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_Esc6_FA5_noHayCantidad()
        {

            //Act
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "");
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.AñadirHerramientaACarrito(herramienta1Nombre);
            Thread.Sleep(500);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(500);
            crearCompra_PO.RellenarFormulario("Gonza", "Ortiz", "gonzalo@alu.uclm.es", "Mi Casa", "PayPal", "Afilada");
            Thread.Sleep(500);
            crearCompra_PO.EstablecerCantidadPorNombre(herramienta1Nombre, "0");
            Thread.Sleep(500);
            crearCompra_PO.PresentarCompra();
            Thread.Sleep(500);
            crearCompra_PO.ConfirmarCompra();
            Thread.Sleep(500);

            _output.WriteLine("URL actual: " + _driver.Url);

            //Assert
            Assert.True(crearCompra_PO.ComprobarError("Cantidad"));
        }

        //FB + FA1 + FA1 + FA2
        //Examen Sprint 3 - Marzo 2026
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void ExamenSprint3Marzo()
        {
            //Arrange
            var expectedHerramienta = new List<string[]>
            {
                new string[] {herramienta2Nombre, herramienta2Material, "2", "5 €", "Puntiagudas"}
            };

            //Act
            InitialStepsForCompraHerramientas();
            selectHerramientasParaComprar_PO.BuscarHerramientas("", "20");               //1. Filtra por precio.
            Thread.Sleep(2000);
            selectHerramientasParaComprar_PO.AñadirHerramientaACarrito(herramienta1Nombre); //2. Añade al carrito una herramienta de las que devuelva.
            Thread.Sleep(2000);
            selectHerramientasParaComprar_PO.BuscarHerramientas("Madera", "");              //3. Filtra por material.
            Thread.Sleep(2000);
            selectHerramientasParaComprar_PO.AñadirHerramientaACarrito(herramienta2Nombre); //4. Añade al carrito una nueva herramienta de las que devuelva (distinta al anterior).
            Thread.Sleep(2000);
            selectHerramientasParaComprar_PO.ComprarHerramientas();
            Thread.Sleep(2000);
            crearCompra_PO.ModificarCompra();                                                   //5. Modifica el carrito y elimina la primera herramienta añadida.
            Thread.Sleep(2000);
            selectHerramientasParaComprar_PO.QuitarHerramientaDeCarrito(herramienta1Nombre);
            Thread.Sleep(2000);
            selectHerramientasParaComprar_PO.ComprarHerramientas();                             //6. Continua con el proceso hasta el final del Flujo Básico.
            Thread.Sleep(2000);
            crearCompra_PO.RellenarFormulario("Gonza", "Ortiz", "gonzalo@alu.uclm.es", "Mi Casa", "PayPal", "Puntiagudas");
            Thread.Sleep(500);
            crearCompra_PO.EstablecerCantidadPorNombre(herramienta2Nombre, "2");                //... para realizar una compra comprando una cantidad de 2 herramientas.
            Thread.Sleep(2000);
            crearCompra_PO.PresentarCompra();
            Thread.Sleep(2000);
            crearCompra_PO.ConfirmarCompra();
            Thread.Sleep(2000);

            //Assert
            Assert.True(detalleCompra_PO.ComprobarDetallesDeCompra("Gonza", "Ortiz", "Mi Casa", DateTime.Today, 10), "Falla CheckDetalleCompra");
            Assert.True(detalleCompra_PO.ComprobarListaDeHerramienta(expectedHerramienta), "Falla CheckListHerramienta");
        }
    }
}