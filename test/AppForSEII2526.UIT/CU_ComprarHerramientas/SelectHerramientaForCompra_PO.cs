using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace AppForSEII2526.UIT.CU_ComprarHerramientas
{
    public class SelectHerramientaForCompra_PO : PageObject
    {
        public SelectHerramientaForCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        By inputMaterial = By.Id("inputMaterial");
        By inputPrecio = By.Id("inputPrecio");
        By botonBuscarHerramientas = By.Id("searchHerramientasButton");
        By tablaDeHerramientas = By.Id("herramientasTable");
        By errorMostrado = By.Id("errorsShow");
        By botonComprarHerramienta = By.Id("comprarHerramientaButton");

        public void BuscarHerramientas(string material, string precio)
        {
            WaitForBeingVisible(inputMaterial);
            var materialObtenido = _driver.FindElement(inputMaterial);
            materialObtenido.Clear();
            materialObtenido.SendKeys(material);

            WaitForBeingVisible(inputPrecio);
            var precioObtenido = _driver.FindElement(inputPrecio);
            precioObtenido.Clear();
            precioObtenido.SendKeys(precio);

            _driver.FindElement(botonBuscarHerramientas).Click();

            WaitForBeingVisible(tablaDeHerramientas);
        }

        public bool ComprobarMensajeDeError(string mensajeEsperado)
        {
            try
            {
                WaitForBeingVisible(errorMostrado);
                string textoActual = _driver.FindElement(errorMostrado).Text;
                return textoActual.Contains(mensajeEsperado);
            }
            catch (WebDriverTimeoutException)
            {
                return false;   //El mensaje de error no ha aparecido.
            }
        }
        public void AñadirHerramientaACarrito(string herramientaNombre)
        {
            By añadirBoton = By.Id("HerramientaParaComprar_" + herramientaNombre);
            WaitForBeingClickable(añadirBoton);
            _driver.FindElement(añadirBoton).Click();
        }

        public void QuitarHerramientaDeCarrito(string herramientaNombre)
        {
            By botonBorrar = By.Id("removeHerramienta_" + herramientaNombre);
            WaitForBeingClickable(botonBorrar);
            _driver.FindElement(botonBorrar).Click();
        }
        public void ComprarHerramientas()
        {
            WaitForBeingClickable(botonComprarHerramienta);
            _driver.FindElement(botonComprarHerramienta).Click();
        }
        public bool CompraNoDisponible()
        {
            try
            {
                return _driver.FindElement(botonComprarHerramienta).Displayed == false;
            }
            catch (Exception e)
            {
                return true;
            }
        }
        public bool ListaDeHerramientasDelCarrito(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaDeHerramientas);
        }

        public bool CarritoEstáVacio()
        {
            var continuar = _driver.FindElements(By.Id("comprarHerramientaBoton"));

            if(continuar.Count > 0 && continuar[0].Displayed)
            {
                return false;
                throw new Exception("Error: La cesta de la compra está vacía, pero se puede continuar...");
            }

            return true;
        }
    }
}