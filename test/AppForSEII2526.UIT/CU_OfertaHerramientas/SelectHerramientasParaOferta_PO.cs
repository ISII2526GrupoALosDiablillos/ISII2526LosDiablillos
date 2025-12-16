using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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
        By buttonContinuarBy = By.Id("ofertarHerramientaButton");

        public SelectHerramientasParaOferta_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchHerramientas(string fabricante, string precio)
        {
            WaitForBeingClickable(inputPrecio);

            var precioInput = _driver.FindElement(inputPrecio);
            precioInput.Clear();
            precioInput.SendKeys(precio ?? "");

            WaitForBeingVisible(inputFabricante);
            WaitUntilSelectHasOptions(inputFabricante, 10);

            SelectFabricanteSafe(fabricante);

            _driver.FindElement(buttonSearchHerramientas).Click();
            WaitUntilTableReady(10);
        }

        public bool CheckListOfHerramientas(List<string[]> expectedHerramientas)
        {
            var actual = GetHerramientasFromTable();

            if (actual.Count != expectedHerramientas.Count)
            {
                _output.WriteLine($"expected rows: {expectedHerramientas.Count} actual rows: {actual.Count}");
                return false;
            }

            for (int i = 0; i < expectedHerramientas.Count; i++)
            {
                var exp = expectedHerramientas[i].Select(x => (x ?? "").Trim()).ToArray();
                var act = actual[i].Select(x => (x ?? "").Trim()).ToArray();

                if (!exp.SequenceEqual(act))
                {
                    _output.WriteLine($"expected row:{string.Join(" ", exp)}");
                    _output.WriteLine($"actual row:{string.Join(" ", act)}");
                    return false;
                }
            }

            return true;
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddHerramientaToOfertaCart(string herramientaNombre)
        {
            WaitUntilTableReady(10);

            var row = FindRowByNombre(herramientaNombre);
            if (row == null) throw new NoSuchElementException($"No se encontró la herramienta '{herramientaNombre}' en la tabla.");

            var addBtn = row.FindElements(By.CssSelector("button, a"))
                            .FirstOrDefault(b => Normalize(b.Text) == "añadir" || Normalize(b.Text) == "anadir");

            if (addBtn == null) throw new NoSuchElementException($"No se encontró botón 'Añadir' para '{herramientaNombre}'.");

            addBtn.Click();
            WaitUntil(() => IsContinuarEnabled(), 10);
        }

        public void RemoveHerramientaFromOfertaCart(string herramientaNombre)
        {
            var byId = By.Id("removeHerramienta_" + herramientaNombre);
            var elems = _driver.FindElements(byId);

            if (elems.Count > 0)
            {
                elems[0].Click();
                WaitUntil(() => !IsContinuarEnabled(), 10);
                return;
            }

            var removeButtons = _driver.FindElements(By.CssSelector("button, a"))
                                       .Where(b =>
                                       {
                                           var t = Normalize(b.Text);
                                           return t.Contains("eliminar") || t.Contains("borrar") || t.Contains("quitar") || t.Contains("remove");
                                       })
                                       .ToList();

            if (removeButtons.Count == 0)
                throw new NoSuchElementException($"No se encontró botón para eliminar '{herramientaNombre}' del carrito.");

            removeButtons[0].Click();
            WaitUntil(() => !IsContinuarEnabled(), 10);
        }

        public bool IsContinuarEnabled()
        {
            var elems = _driver.FindElements(buttonContinuarBy);
            if (elems.Count == 0) return false;
            return elems[0].Enabled;
        }

        public void ClickContinuar()
        {
            WaitForBeingClickable(buttonContinuarBy);
            _driver.FindElement(buttonContinuarBy).Click();
        }

        public List<string[]> GetHerramientasFromTable()
        {
            WaitForBeingVisible(tableOfHerramientasBy);

            var table = _driver.FindElement(tableOfHerramientasBy);
            var rows = table.FindElements(By.CssSelector("tbody tr"));

            var result = new List<string[]>();

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.CssSelector("td"))
                               .Select(td => (td.Text ?? "").Trim())
                               .ToList();

                if (cells.Count >= 6)
                    cells = cells.Take(5).ToList();

                if (cells.Count >= 5)
                    result.Add(cells.Take(5).ToArray());
            }

            return result;
        }

        private IWebElement FindRowByNombre(string herramientaNombre)
        {
            var table = _driver.FindElement(tableOfHerramientasBy);
            var rows = table.FindElements(By.CssSelector("tbody tr"));

            foreach (var r in rows)
            {
                var tds = r.FindElements(By.CssSelector("td"));
                if (tds.Count < 2) continue;

                var nombre = (tds[1].Text ?? "").Trim();
                if (string.Equals(nombre, (herramientaNombre ?? "").Trim(), StringComparison.OrdinalIgnoreCase))
                    return r;
            }

            return null;
        }

        private void WaitUntilTableReady(int seconds)
        {
            WaitForBeingVisible(tableOfHerramientasBy);
            WaitUntil(() =>
            {
                var table = _driver.FindElement(tableOfHerramientasBy);
                var rows = table.FindElements(By.CssSelector("tbody tr"));
                return rows != null && rows.Count > 0;
            }, seconds);
        }

        private void SelectFabricanteSafe(string fabricante)
        {
            var selectElem = _driver.FindElement(inputFabricante);
            var sel = new SelectElement(selectElem);

            string fab = fabricante ?? "";
            if (string.IsNullOrWhiteSpace(fab) || Normalize(fab) == "all")
            {
                TrySelectByNormalizedText(sel, "All");
                return;
            }

            WaitUntil(() => sel.Options.Any(o => Normalize(o.Text) == Normalize(fab)), 10);

            if (!TrySelectByNormalizedText(sel, fab))
            {
                var opciones = string.Join(" | ", sel.Options.Select(o => (o.Text ?? "").Trim()));
                throw new NoSuchElementException($"Cannot locate element with text: {fab}. Opciones disponibles: {opciones}");
            }
        }

        private bool TrySelectByNormalizedText(SelectElement sel, string text)
        {
            var target = Normalize(text);
            var option = sel.Options.FirstOrDefault(o => Normalize(o.Text) == target);
            if (option == null) return false;
            sel.SelectByText(option.Text);
            return true;
        }

        private void WaitUntilSelectHasOptions(By selectBy, int seconds)
        {
            WaitUntil(() =>
            {
                var sel = new SelectElement(_driver.FindElement(selectBy));
                return sel.Options != null && sel.Options.Count > 0;
            }, seconds);
        }

        private void WaitUntil(Func<bool> condition, int seconds)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            wait.PollingInterval = TimeSpan.FromMilliseconds(200);
            wait.Until(_ => condition());
        }

        private static string Normalize(string s)
        {
            return (s ?? "").Trim().ToLowerInvariant();
        }
    }
}
