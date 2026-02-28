using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_ComprarHerramientas
{
    public class CompraDetails_PO : PageObject
    {
        private By _labelNombreCompleto = By.Id("NameSurname");
        private By _labelDireccion = By.Id("DeliveryAddress");
        private By _labelPrecioTotal = By.Id("TotalPrice");
        private By _tablaItems = By.Id("Herramientas");
        public CompraDetails_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        private By _tableOfItems = By.Id("ComprasHerramientas");
        public bool ComprobarDetallesDeCompra(string nombreCliente, string apellidoCliente, string direccionEnvio, DateTime fechaCompra, int precioTotal)
        {
            WaitForBeingVisible(_tableOfItems);
            bool resultado = true;
            var nombreCompleto = nombreCliente + " " + apellidoCliente;
            resultado = resultado && _driver.FindElement(By.Id("NombreCompleto")).Text.Contains(nombreCompleto);
            resultado = resultado && _driver.FindElement(By.Id("DireccionCompra")).Text.Contains(direccionEnvio);
            resultado = resultado && _driver.FindElement(By.Id("FechaCompra")).Text.Contains(fechaCompra.ToString("dd/MM/yyyy"));
            resultado = resultado && _driver.FindElement(By.Id("PrecioTotal")).Text.Contains(precioTotal.ToString());
            return resultado;
        }
        public bool ComprobarListaDeHerramienta(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, _tableOfItems);
        }
    }
}