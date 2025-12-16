using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using Xunit.Abstractions;

namespace AppForMovies.UIT.Shared
{
    public class UC_UIT : IDisposable
    {
        private bool _pipeline = false;
        private string _browser = "Edge";

        protected IWebDriver _driver;
        protected readonly ITestOutputHelper _output;

        public string _URI => NormalizeBaseUrl(
            Environment.GetEnvironmentVariable("UIT_BASE_URL") ?? "https://localhost:7081/"
        );

        public UC_UIT(ITestOutputHelper output)
        {
            _output = output;

            switch (_browser)
            {
                case "Firefox":
                    SetUp_FireFox4UIT();
                    break;
                case "Edge":
                    SetUp_EdgeFor4UIT();
                    break;
                default:
                    SetUp_Chrome4UIT();
                    break;
            }

            _driver.Manage().Window.Maximize();
        }

        protected void Initial_step_opening_the_web_page()
        {
            WaitForServer(_URI, 30);
            _driver.Navigate().GoToUrl(_URI);
        }

        protected void Perform_login(string email, string password)
        {
            WaitForServer(_URI, 30);
            _driver.Navigate().GoToUrl(_URI + "Account/Login");

            _driver.FindElement(By.Name("Input.Email")).SendKeys(email);
            _driver.FindElement(By.Name("Input.Password")).SendKeys(password);
            _driver.FindElement(By.XPath("/html/body/div[1]/main/article/div/div[1]/section/form/div[4]/button")).Click();
        }

        private static string NormalizeBaseUrl(string url)
        {
            url = (url ?? "").Trim();
            if (!url.EndsWith("/")) url += "/";
            return url;
        }

        private void WaitForServer(string url, int timeoutSeconds)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(2)
            };

            var sw = Stopwatch.StartNew();
            Exception last = null;

            while (sw.Elapsed < TimeSpan.FromSeconds(timeoutSeconds))
            {
                try
                {
                    var resp = client.GetAsync(url).GetAwaiter().GetResult();
                    if ((int)resp.StatusCode >= 200 && (int)resp.StatusCode < 500)
                        return;
                }
                catch (Exception ex)
                {
                    last = ex;
                }

                Thread.Sleep(500);
            }

            throw new InvalidOperationException($"No se pudo conectar a {url}. Último error: {last?.Message}");
        }

        protected void SetUp_Chrome4UIT()
        {
            var options = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if (_pipeline) options.AddArgument("--headless=new");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--no-first-run");
            options.AddArgument("--no-default-browser-check");
            options.AddArgument("--remote-allow-origins=*");

            _driver = new ChromeDriver(options);
        }

        protected void SetUp_FireFox4UIT()
        {
            var options = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if (_pipeline) options.AddArgument("--headless");

            _driver = new FirefoxDriver(options);
        }

        protected void SetUp_EdgeFor4UIT()
        {
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--allow-insecure-localhost");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--no-first-run");
            options.AddArgument("--no-default-browser-check");
            options.AddArgument("inprivate");
            options.AddArgument("--remote-allow-origins=*");

            if (_pipeline) options.AddArgument("--headless=new");

            _driver = new EdgeDriver(options);
        }

        public void Dispose()
        {
            try { _driver.Quit(); } catch { }
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
