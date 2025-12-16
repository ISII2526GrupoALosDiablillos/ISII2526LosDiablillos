using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_OfertaHerramientas
{
    public class PostOferta_PO : PageObject
    {
        By fechaInicioBy = By.Id("FechaInicio");
        By fechaFinalBy = By.Id("FechaFinal");
        By submitBy = By.Id("Submit");
        By modifyBy = By.Id("ModifyHerramientas");
        By errorsShownBy = By.Id("ErrorsShown");
        By validationSummaryBy = By.CssSelector(".validation-summary-errors, .validation-summary-valid");

        public PostOferta_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void WaitForPage()
        {
            WaitForBeingVisible(fechaInicioBy);
            WaitForBeingVisible(fechaFinalBy);
            WaitForBeingVisible(submitBy);
        }

        public bool IsOnCreateOferta()
        {
            return _driver.FindElements(fechaInicioBy).Count > 0 && _driver.FindElements(fechaFinalBy).Count > 0;
        }

        public void SetFechaInicio(DateTime date)
        {
            var input = _driver.FindElement(fechaInicioBy);
            input.Clear();
            input.SendKeys(date.ToString("yyyy-MM-dd"));
        }

        public void SetFechaFinal(DateTime date)
        {
            var input = _driver.FindElement(fechaFinalBy);
            input.Clear();
            input.SendKeys(date.ToString("yyyy-MM-dd"));
        }

        public void ClearFechaInicio()
        {
            _driver.FindElement(fechaInicioBy).Clear();
        }

        public void ClearFechaFinal()
        {
            _driver.FindElement(fechaFinalBy).Clear();
        }

        public void SetPorcentaje(string herramientaId, string value)
        {
            var by = By.Id($"porcentaje_{herramientaId}");
            WaitForBeingVisible(by);
            var input = _driver.FindElement(by);
            input.Clear();
            input.SendKeys(value ?? "");
        }

        public void ClickSubmit()
        {
            WaitForBeingClickable(submitBy);
            _driver.FindElement(submitBy).Click();
        }

        public void ClickModifyHerramientas()
        {
            WaitForBeingClickable(modifyBy);
            _driver.FindElement(modifyBy).Click();
        }

        public bool IsDialogOpen()
        {
            return _driver.FindElements(By.XPath("//*[contains(normalize-space(.), 'Confirmar Oferta')]")).Count > 0
                || _driver.FindElements(By.CssSelector(".modal, .dialog, [role='dialog']")).Count > 0
                || FindDialogSaveButton() != null;
        }

        public void ClickDialogSave()
        {
            var btn = FindDialogSaveButton();
            if (btn == null) throw new NoSuchElementException("No se encontró el botón de guardar del diálogo.");
            btn.Click();
        }

        public string GetErrorText()
        {
            var e = _driver.FindElements(errorsShownBy);
            if (e.Count > 0 && e[0].Displayed)
            {
                var t = (e[0].Text ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(t)) return t;
            }

            var vs = _driver.FindElements(validationSummaryBy);
            if (vs.Count > 0 && vs[0].Displayed)
            {
                var t = (vs[0].Text ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(t)) return t;
            }

            var alerts = _driver.FindElements(By.CssSelector(".alert.alert-danger, .alert.alert-warning"));
            foreach (var a in alerts)
            {
                if (a.Displayed)
                {
                    var t = (a.Text ?? "").Trim();
                    if (!string.IsNullOrWhiteSpace(t)) return t;
                }
            }

            return "";
        }

        public bool WaitForError(int seconds)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            wait.PollingInterval = TimeSpan.FromMilliseconds(200);
            return wait.Until(_ => !string.IsNullOrWhiteSpace(GetErrorText()));
        }

        private IWebElement FindDialogSaveButton()
        {
            var byId = _driver.FindElements(By.Id("Save"));
            if (byId.Count > 0) return byId[0];

            var buttons = _driver.FindElements(By.CssSelector("button")).ToList();

            foreach (var b in buttons)
            {
                var t = Normalize(b.Text);
                if (t == "save" || t == "guardar" || t == "ok" || t == "aceptar" || t == "si" || t == "sí")
                    return b;
            }

            foreach (var b in buttons)
            {
                var t = Normalize(b.Text);
                if (t.Contains("save") || t.Contains("guardar") || t.Contains("aceptar") || t.Contains("publicar"))
                    return b;
            }

            return null;
        }

        private static string Normalize(string s)
        {
            return (s ?? "").Trim().ToLowerInvariant();
        }
    }
}
