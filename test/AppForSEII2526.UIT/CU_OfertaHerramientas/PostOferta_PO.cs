using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_OfertaHerramientas
{
    public class PostOferta_PO : PageObject
    {
        By buttonSubmit = By.Id("Submit");
        By button_DialogOK = By.Id("Button_DialogOK");
        By modifyHerramientasButton = By.Id("ModifyHerramientas");
        By tableOfOfertaItems = By.Id("TableOfOfertaItems");
        By errorsLabel = By.Id("ErrorsShown");

        By fechaInicioInput = By.Id("FechaInicio");
        By fechaFinalInput = By.Id("FechaFinal");
        By selectMetodoPago = By.Id("tiposMetodoPago");
        By selectDirigidaOferta = By.Id("DirigidaOferta");

        public PostOferta_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void guardarOfertaDialog()
        {
            WaitForBeingClickable(button_DialogOK);
            _driver.FindElement(button_DialogOK).Click();
        }

        public void modificarHerramientas()
        {
            WaitForBeingClickable(modifyHerramientasButton);
            _driver.FindElement(modifyHerramientasButton).Click();
        }

        private void VaciarFecha(IWebElement inputFecha)
        {
            inputFecha.Click();

            inputFecha.SendKeys(Keys.ArrowLeft);
            inputFecha.SendKeys(Keys.ArrowLeft);
            inputFecha.SendKeys(Keys.ArrowLeft);

            inputFecha.SendKeys(Keys.Delete);

            inputFecha.SendKeys(Keys.ArrowRight);
            inputFecha.SendKeys(Keys.Delete);

            inputFecha.SendKeys(Keys.ArrowRight);
            inputFecha.SendKeys(Keys.Delete);

            inputFecha.SendKeys(Keys.Tab);
        }

        public void addAtributosOferta(string herramientaId, string fechaInicio, string fechaFinal, string metodoPago, string dirigidaA, string porcentaje)
        {
            WaitForBeingVisible(fechaInicioInput);
            var fInicio = _driver.FindElement(fechaInicioInput);

            if (fechaInicio == "")
            {
                VaciarFecha(fInicio);
            }
            else if (fechaInicio != null)
            {
                fInicio.SendKeys(fechaInicio);
                fInicio.SendKeys(Keys.Tab);
            }

            var fFinal = _driver.FindElement(fechaFinalInput);
            if (fechaFinal == "")
            {
                VaciarFecha(fFinal);
            }
            else if (fechaFinal != null)
            {
                fFinal.SendKeys(fechaFinal);
                fFinal.SendKeys(Keys.Tab);
            }

            Thread.Sleep(500);

            if (string.IsNullOrEmpty(metodoPago)) metodoPago = "Tarjeta de Crédito";
            WaitForBeingClickable(selectMetodoPago);
            var elementPago = _driver.FindElement(selectMetodoPago);
            var selectPago = new SelectElement(elementPago);
            try { selectPago.SelectByText(metodoPago); } catch { selectPago.SelectByIndex(0); }

            if (string.IsNullOrEmpty(dirigidaA)) dirigidaA = "Clientes";
            WaitForBeingClickable(selectDirigidaOferta);
            var elementDirigida = _driver.FindElement(selectDirigidaOferta);
            var selectDirigida = new SelectElement(elementDirigida);
            try { selectDirigida.SelectByText(dirigidaA); } catch { selectDirigida.SelectByIndex(0); }

            By porcentajeLocator = By.Id($"porcentaje_{herramientaId}");
            if (porcentaje != null)
            {
                try
                {
                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                    wait.Until(d => d.FindElement(porcentajeLocator).Displayed);
                }
                catch (WebDriverTimeoutException)
                {
                    _output.WriteLine($"ERROR: No se encontró el campo 'porcentaje_{herramientaId}'.");
                    throw;
                }

                var inputPorcentaje = _driver.FindElement(porcentajeLocator);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", inputPorcentaje);
                Thread.Sleep(200);

                inputPorcentaje.Clear();

                if (porcentaje == "")
                {
                    inputPorcentaje.SendKeys(Keys.Delete);
                    inputPorcentaje.SendKeys(Keys.Tab);
                }
                else
                {
                    inputPorcentaje.SendKeys(porcentaje);
                    inputPorcentaje.SendKeys(Keys.Tab);
                }
            }

            Thread.Sleep(1000);

            
            var btn = _driver.FindElement(buttonSubmit);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
        }

        public bool CheckListOfOfertaItems(List<string[]> expectedOfertaItems)
        {
            return CheckBodyTable(expectedOfertaItems, tableOfOfertaItems);
        }

        public bool CheckValidationError(string expectedError)
        {
            try
            {
                Thread.Sleep(1000);
                string pageSource = _driver.PageSource;
                return pageSource.Contains(expectedError);
            }
            catch { return false; }
        }
    }
}