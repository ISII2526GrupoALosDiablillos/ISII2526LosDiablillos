using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_OfertaHerramientas
{
    public class DetailOferta_PO : PageObject
    {
        By tablaHerramientasOfertadas = By.Id("HerramientasEnOferta");

        public DetailOferta_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckOfertaDetail(DateTime fechaInicio, DateTime fechaFinal, DateTime fechaOferta, string metodoPago, string dirigidaA, int itemsCount)
        {
            WaitForBeingVisible(tablaHerramientasOfertadas);
            bool result = true;

            result = result && _driver.FindElement(By.Id("FechaInicio")).Text.Contains(fechaInicio.ToString("dd/MM/yyyy"));
            result = result && _driver.FindElement(By.Id("FechaFinal")).Text.Contains(fechaFinal.ToString("dd/MM/yyyy"));
            result = result && _driver.FindElement(By.Id("FechaOferta")).Text.Contains(fechaOferta.ToString("dd/MM/yyyy"));

            result = result && _driver.FindElement(By.Id("MetodoPago")).Text.Contains(dirigidaA);
            result = result && _driver.FindElement(By.Id("TipoDirigida")).Text.Contains(metodoPago);

            int count = _driver.FindElements(By.XPath("//table[@id='HerramientasEnOferta']/tbody/tr")).Count;
            result = result && (count == itemsCount);

            return result;
        }

        public bool CheckListaHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientasOfertadas);
        }
    }
}