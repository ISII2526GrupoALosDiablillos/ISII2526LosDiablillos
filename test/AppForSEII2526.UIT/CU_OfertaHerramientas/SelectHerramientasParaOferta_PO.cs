using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_OfertaHerramientas
{
    public class SelectHerramientasParaOferta_PO : PageObject
    {
        By inputFabricante = By.Id("selectFabricante");
        By inputPrecio = By.Id("inputPrecio");
        By buttonSearchHerramientas = By.Id("searchHerramientas");
        By tableOfHerramientasBy = By.Id("TableOfHerramientas");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonCrearOfertaCarrito = By.Id("crearOfertaButton");

        public SelectHerramientasParaOferta_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchHerramientas(string fabricante, string precio)
        {
            Thread.Sleep(1500);

            try
            {
                WaitForBeingClickable(inputPrecio);
                _driver.FindElement(inputPrecio).Clear();
                _driver.FindElement(inputPrecio).SendKeys(precio);
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(1000);
                WaitForBeingClickable(inputPrecio);
                _driver.FindElement(inputPrecio).Clear();
                _driver.FindElement(inputPrecio).SendKeys(precio);
            }

            if (string.IsNullOrEmpty(fabricante)) fabricante = "All";

            SelectElement selectElement = new SelectElement(_driver.FindElement(inputFabricante));
            try { selectElement.SelectByText(fabricante); }
            catch { selectElement.SelectByValue(fabricante); }

            _driver.FindElement(buttonSearchHerramientas).Click();
        }

        public void crearOfertaCarrito()
        {
            Thread.Sleep(500);
            WaitForBeingClickable(buttonCrearOfertaCarrito);
            _driver.FindElement(buttonCrearOfertaCarrito).Click();
        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tableOfHerramientasBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            WaitForBeingVisible(errorShownBy);
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddHerramientaToOfertaCart(string herramientaNombre)
        {
            WaitForBeingVisible(tableOfHerramientasBy);
            By btnAddLocator = By.XPath($"//tr[td[text()='{herramientaNombre}']]//button[contains(@id, 'herramientaParaOferta_')]");

            WaitForBeingClickable(btnAddLocator);
            _driver.FindElement(btnAddLocator).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            try
            {
                wait.Until(d => d.FindElements(By.XPath("//button[contains(@id, 'removeHerramienta_')]")).Count > 0);
            }
            catch { }
        }

        public void RemoveHerramientaFromOfertaCart(string herramientaId)
        {
            By btnRemoveLocator = By.Id("removeHerramienta_" + herramientaId);
            WaitForBeingClickable(btnRemoveLocator);
            _driver.FindElement(btnRemoveLocator).Click();
            Thread.Sleep(500);
        }

        public bool OfertaNotAvailable()
        {
            try
            {
                var element = _driver.FindElement(buttonCrearOfertaCarrito);
                return !element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        public void WaitForBeingVisible(By element)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.FindElement(element).Displayed);
        }
    }
}