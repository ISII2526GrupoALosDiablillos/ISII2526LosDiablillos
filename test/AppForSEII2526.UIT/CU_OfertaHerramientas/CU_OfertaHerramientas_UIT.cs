using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.CU_OfertaHerramientas;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_OfertaHerramientas_UIT
{
    public class CU_OfertaHerramientas_UIT : UC_UIT
    {
        private const string URI = "https://localhost:7081/";

        private SelectHerramientasParaOferta_PO SelectHerramientasParaOferta_PO;
        private DetailOferta_PO detailOferta_PO;
        private PostOferta_PO PostOferta_PO;

        private const string herramientaIdMakita = "3";
        private const string herramientaNombreMakita = "Martillo";
        private const string herramientaMaterialMakita = "Madera";
        private const string herramientaPrecioMakita = "20";
        private const string herramientaFabricanteMakita = "Makita";

        private const string herramientaIdBosch = "4";
        private const string herramientaNombreBosch = "Destornillador";
        private const string herramientaMaterialBosch = "Metal";
        private const string herramientaPrecioBosch = "15";
        private const string herramientaFabricanteBosch = "Bosch";

        private const string porcentaje1 = "-4";
        private const string porcentaje2 = "105";

        public CU_OfertaHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            SelectHerramientasParaOferta_PO = new SelectHerramientasParaOferta_PO(_driver, _output);
            detailOferta_PO = new DetailOferta_PO(_driver, _output);
            PostOferta_PO = new PostOferta_PO(_driver, _output);
        }

        private void InitialStepsForOfertaHerramientas()
        {
            Initial_step_opening_the_web_page();
            _driver.Navigate().GoToUrl(URI + "oferta/selectherramientaparaoferta");
        }

        [Theory]
        [InlineData(herramientaIdMakita, herramientaNombreMakita)]
        [InlineData(herramientaIdBosch, herramientaNombreBosch)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_1_BF_CrearOferta(string herramientaId, string herramientaNombre)
        {
            InitialStepsForOfertaHerramientas();
            Thread.Sleep(500);

            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombre);

            SelectHerramientasParaOferta_PO.crearOfertaCarrito();
            Thread.Sleep(1000);

            PostOferta_PO.addAtributosOferta(herramientaId, DateTime.Today.AddDays(3).ToString("dd/MM/yyyy"), DateTime.Today.AddDays(11).ToString("dd/MM/yyyy"), "PayPal", "Socios", "10");
            Thread.Sleep(1000);

            PostOferta_PO.guardarOfertaDialog();
            Thread.Sleep(1000);

            Assert.True(detailOferta_PO.CheckOfertaDetail(DateTime.Today.AddDays(3), DateTime.Today.AddDays(11), DateTime.Today, "PayPal", "Socios", 1));
        }

        [Theory]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, herramientaMaterialMakita, herramientaPrecioMakita, herramientaFabricanteMakita, "Makita", "")]
        [InlineData(herramientaIdBosch, herramientaNombreBosch, herramientaMaterialBosch, herramientaPrecioBosch, herramientaFabricanteBosch, "", "15")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_4_AF0_filtroPorFabricantePrecio(string herramientaId, string herramientaNombre, string herramientaMaterial, string herramientaPrecio, string herramientaFabricante,
            string filtroFabricante, string filtroPrecio)
        {
            InitialStepsForOfertaHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { herramientaId, herramientaNombre, herramientaMaterial, herramientaPrecio, herramientaFabricante }, };

            SelectHerramientasParaOferta_PO.SearchHerramientas(filtroFabricante, filtroPrecio);
            Thread.Sleep(500);

            Assert.True(SelectHerramientasParaOferta_PO.CheckListOfHerramientas(expectedHerramientas));
        }

        [Theory]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, "26/12/2025", "14/12/2025", "Error! Tu oferta debe terminar después de que empiece")]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, "09/12/2025", "14/12/2025", "Error! La fecha de inicio de tu oferta debe ser posterior a hoy")]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, "20/12/2025", "22/12/2025", "¡Error!, la oferta debe durar al menos una semana")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_5_6_7_AF1_FechasErroneas(string herramientaId, string herramientaNombre, string fechaInicio, string fechaFinal, string expectedError)
        {
            InitialStepsForOfertaHerramientas();
            Thread.Sleep(1000);
            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombre);

            SelectHerramientasParaOferta_PO.crearOfertaCarrito();
            Thread.Sleep(1000);

            PostOferta_PO.addAtributosOferta(herramientaId, fechaInicio, fechaFinal, "Efectivo", "Socios", "10");
            Thread.Sleep(1500);

            Assert.True(PostOferta_PO.CheckValidationError(expectedError), $"Expected error: {expectedError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_AF2_ModificarCarrito()
        {
            InitialStepsForOfertaHerramientas();
            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombreMakita);
            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombreBosch);

            SelectHerramientasParaOferta_PO.crearOfertaCarrito();
            Thread.Sleep(500);

            PostOferta_PO.modificarHerramientas();
            Thread.Sleep(500);

            SelectHerramientasParaOferta_PO.RemoveHerramientaFromOfertaCart(herramientaIdMakita);

            SelectHerramientasParaOferta_PO.crearOfertaCarrito();
            Thread.Sleep(500);

            var expectedOfertaItems = new List<string[]> { new string[] { herramientaIdBosch, herramientaPrecioBosch } };

            Assert.True(PostOferta_PO.CheckListOfOfertaItems(expectedOfertaItems));
        }

        [Theory]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, porcentaje1, "El porcentaje debe estar entre 0 y 100")]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, porcentaje2, "El porcentaje debe estar entre 0 y 100")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_8_9_AF3_PorcentajeErroneo(string herramientaId, string herramientaNombre, string porcentaje, string expectedError)
        {
            InitialStepsForOfertaHerramientas();
            Thread.Sleep(500);
            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombre);

            SelectHerramientasParaOferta_PO.crearOfertaCarrito();
            Thread.Sleep(1000);

            PostOferta_PO.addAtributosOferta(herramientaId, DateTime.Today.AddDays(5).ToString("dd/MM/yyyy"),
                DateTime.Today.AddDays(15).ToString("dd/MM/yyyy"), "Tarjeta de Crédito", "Clientes", porcentaje);
            Thread.Sleep(1000);

            Assert.True(PostOferta_PO.CheckValidationError(expectedError), $"Expected error: {expectedError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_10_AF4_CarritoVacio()
        {
            InitialStepsForOfertaHerramientas();
            Thread.Sleep(500);
            Assert.True(SelectHerramientasParaOferta_PO.OfertaNotAvailable());
        }

        [Theory]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, "20/12/2025", "", "10", "The FechaFinal field must be a date.")]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, "", "30/12/2025", "10", "The FechaInicio field must be a date.")]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, "20/12/2025", "30/12/2025", "", "The Porcentaje field must be a number.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_11_12_AF5_CamposObligatorios(string herramientaId, string herramientaNombre, string fechaInicio, string fechaFinal, string porcentaje, string expectedError)
        {
            InitialStepsForOfertaHerramientas();
            Thread.Sleep(2000);
            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombre);

            SelectHerramientasParaOferta_PO.crearOfertaCarrito();
            Thread.Sleep(2000);

            PostOferta_PO.addAtributosOferta(herramientaId, fechaInicio, fechaFinal, "Efectivo", "Socios", porcentaje);
            Thread.Sleep(2000);

            Assert.True(PostOferta_PO.CheckValidationError(expectedError), $"Expected error: {expectedError}");
        }
    }
}