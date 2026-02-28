using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_ComprarHerramientas
{
    public class CompraCreate_PO : PageObject
    {
        public CompraCreate_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        private By _nombreCliente = By.CssSelector("input[name$='NombreCliente']");
        private By _apellidoCliente = By.CssSelector("input[name$='ApellidoCliente']");
        private By _direccionEnvio = By.CssSelector("input[name$='DireccionEnvio']");
        private By _correoElectronico = By.CssSelector("input[name$='CorreoElectronico']");
        private By _metodoPago = By.Id("TiposMetodoPago");

        private By _comprarHerramientas = By.Id("Submit");
        private By _dialogOk = By.Id("Button_DialogOK");

        private By _errorsShown = By.Id("ErrorsShown");
        private By _modificarCompra = By.Id("ModifyCompraItems");
        private By _tableOfItems = By.Id("TableOfRentalItems");

        private By _listaErroresValidacion = By.ClassName("validation-message"); // Errores campo a campo
        private By _resumenErrores = By.ClassName("validation-summary-errors"); // Resumen arriba
        private By _validationSummary = By.CssSelector(".validation-summary-errors, .text-danger, .alert-danger");

        private By _dialogModal = By.Id("DialogOKSaveDelete");

        public void ConfirmarCompra()
        {
            WaitForBeingVisible(_dialogModal);
            WaitForBeingClickable(_dialogOk);
            _driver.FindElement(_dialogOk).Click();
        }

        public void EsperarPaginaCrearCompra()
        {

            var form = By.CssSelector("form");
            WaitForBeingVisible(form);
        }

        public void RellenarFormulario(string nombreCliente, string apellidoCliente, string correo, string direccionEnvio, string metodoPagoId, string descripcion)
        {
            EsperarPaginaCrearCompra();
            WaitForBeingVisible(_nombreCliente);

            _driver.FindElement(_nombreCliente).Clear();
            _driver.FindElement(_nombreCliente).SendKeys(nombreCliente);

            _driver.FindElement(_apellidoCliente).Clear();
            _driver.FindElement(_apellidoCliente).SendKeys(apellidoCliente);

            _driver.FindElement(_correoElectronico).Clear();
            _driver.FindElement(_correoElectronico).SendKeys(correo);

            _driver.FindElement(_direccionEnvio).Clear();
            _driver.FindElement(_direccionEnvio).SendKeys(direccionEnvio);


            WaitForBeingVisible(_tableOfItems);
            var filas = _driver.FindElements(By.CssSelector("#TableOfRentalItems tbody tr"));
            foreach (var fila in filas)
            {
                var descInput = fila.FindElement(By.CssSelector("td:last-child input"));
                descInput.Clear();
                descInput.SendKeys(descripcion);
            }

            var select = new SelectElement(_driver.FindElement(_metodoPago));

            if (!string.IsNullOrWhiteSpace(metodoPagoId))
            {
                try { select.SelectByValue(metodoPagoId); }
                catch (NoSuchElementException) { select.SelectByText(metodoPagoId); }
            }
            else
            {
                var emptyOpt = select.Options.FirstOrDefault(o => (o.GetAttribute("value") ?? "") == "");
                if (emptyOpt != null)
                {
                    select.SelectByValue("");
                    _output.WriteLine("Seleccionada opción vacía (value=\"\").");
                }
                else
                {
                    select.SelectByIndex(0);
                    _output.WriteLine($"Seleccionado índice 0: '{select.SelectedOption.Text}' value='{select.SelectedOption.GetAttribute("value")}'");
                }
            }
        }
        public void PresentarCompra()
        {
            WaitForBeingClickable(_comprarHerramientas);
            _driver.FindElement(_comprarHerramientas).Click();
        }
        public void ModificarCompra()
        {
            WaitForBeingClickable(_modificarCompra);
            _driver.FindElement(_modificarCompra).Click();
        }

        public bool ComprobarError(string expected)
        {
            var textos = new List<string>();

            textos.AddRange(_driver.FindElements(By.CssSelector(".validation-message"))
                .Select(e => e.Text));

            textos.AddRange(_driver.FindElements(By.CssSelector("form ul li"))
                .Select(e => e.Text));

            textos.AddRange(_driver.FindElements(By.CssSelector(".alert.alert-danger"))
                .Select(e => e.Text));

            var todosLosErrores = string.Join(" | ", textos.Where(t => !string.IsNullOrWhiteSpace(t)));
            _output.WriteLine($"Errores encontrados: {todosLosErrores}");

            return todosLosErrores.Contains(expected, StringComparison.OrdinalIgnoreCase);
        }

        public bool ComprobarListaHerramientasEnCompra(List<string[]> expectedCompraItems)
        {
            return CheckBodyTable(expectedCompraItems, _tableOfItems);
        }


        public void EstablecerCantidadPorNombre(string nombre, string cantidad)
        {
            WaitForBeingVisible(_tableOfItems);

            var fila = _driver.FindElement(By.CssSelector($"tr#HerramientaData_{nombre}"));
            var entrada = fila.FindElement(By.CssSelector("td:nth-child(3) input"));

            entrada.Click();
            entrada.SendKeys(Keys.Control + "a");
            entrada.SendKeys(Keys.Delete);

            entrada.SendKeys(cantidad);
            entrada.SendKeys(Keys.Tab);

            //Confirmar lo que queda en la entrada (input).
            var actual = entrada.GetAttribute("value");
            _output.WriteLine($"Cantidad escrita='{cantidad}', value en input='{actual}'");
        }
    }
}