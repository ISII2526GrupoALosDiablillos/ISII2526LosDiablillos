using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.CU_OfertaHerramientas;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_OfertaHerramientas_UIT
{
    public class CU_OfertaHerramientas_UIT : UC_UIT
    {
        private SelectHerramientasParaOferta_PO SelectHerramientasParaOferta_PO;
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

        public CU_OfertaHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            SelectHerramientasParaOferta_PO = new SelectHerramientasParaOferta_PO(_driver, _output);
            PostOferta_PO = new PostOferta_PO(_driver, _output);
        }

        private void InitialStepsForOfertaHerramientas()
        {
            Initial_step_opening_the_web_page();

            By id = By.Id("CreateOferta");
            SelectHerramientasParaOferta_PO.WaitForBeingVisible(id);
            _driver.FindElement(id).Click();
        }

        [Theory]
        [InlineData(herramientaIdMakita, herramientaNombreMakita, herramientaMaterialMakita, herramientaPrecioMakita, herramientaFabricanteMakita, "Makita", "")]
        [InlineData(herramientaIdBosch, herramientaNombreBosch, herramientaMaterialBosch, herramientaPrecioBosch, herramientaFabricanteBosch, "", "15")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_AF0_filteringPorPrecioFabricante(
            string herramientaId, string herramientaNombre, string herramientaMaterial, string herramientaPrecio, string herramientaFabricante,
            string filtroFabricante, string filtroPrecio)
        {
            InitialStepsForOfertaHerramientas();

            var expectedHerramientas = new List<string[]>
            {
                new string[] { herramientaId, herramientaNombre, herramientaMaterial, herramientaPrecio, herramientaFabricante },
            };

            SelectHerramientasParaOferta_PO.SearchHerramientas(filtroFabricante, filtroPrecio);

            Assert.True(SelectHerramientasParaOferta_PO.CheckListOfHerramientas(expectedHerramientas));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_AF1_fechasInvalidas_muestraError_y_permaneceEnPaso5()
        {
            InitialStepsForOfertaHerramientas();

            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombreMakita);
            SelectHerramientasParaOferta_PO.ClickContinuar();

            PostOferta_PO.WaitForPage();

            PostOferta_PO.SetFechaInicio(DateTime.Today.AddDays(-1));
            PostOferta_PO.SetFechaFinal(DateTime.Today.AddDays(-2));
            PostOferta_PO.SetPorcentaje(herramientaIdMakita, "10");

            PostOferta_PO.ClickSubmit();

            Assert.True(PostOferta_PO.IsDialogOpen());

            PostOferta_PO.ClickDialogSave();

            Assert.True(PostOferta_PO.WaitForError(10));
            Assert.True(PostOferta_PO.IsOnCreateOferta());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_AF2_modificarCarrito_eliminarHerramienta_y_quedarSinContinuar()
        {
            InitialStepsForOfertaHerramientas();

            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombreMakita);
            SelectHerramientasParaOferta_PO.ClickContinuar();

            PostOferta_PO.WaitForPage();
            PostOferta_PO.ClickModifyHerramientas();

            Assert.False(PostOferta_PO.IsOnCreateOferta());

            SelectHerramientasParaOferta_PO.RemoveHerramientaFromOfertaCart(herramientaNombreMakita);
            Assert.False(SelectHerramientasParaOferta_PO.IsContinuarEnabled());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_AF3_porcentajeFueraDeRango_desactivaContinuar()
        {
            InitialStepsForOfertaHerramientas();

            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombreMakita);
            SelectHerramientasParaOferta_PO.ClickContinuar();

            PostOferta_PO.WaitForPage();

            PostOferta_PO.SetPorcentaje(herramientaIdMakita, "101");

            PostOferta_PO.ClickSubmit();

            if (PostOferta_PO.IsDialogOpen())
            {
                PostOferta_PO.ClickDialogSave();
                Assert.True(PostOferta_PO.WaitForError(10));
                Assert.True(PostOferta_PO.IsOnCreateOferta());
            }
            else
            {
                var err = PostOferta_PO.GetErrorText();
                Assert.True(!string.IsNullOrWhiteSpace(err));
                Assert.True(PostOferta_PO.IsOnCreateOferta());
            }
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_AF4_sinHerramientas_enCarrito_noActivaContinuar()
        {
            InitialStepsForOfertaHerramientas();
            Assert.False(SelectHerramientasParaOferta_PO.IsContinuarEnabled());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_AF5_datoObligatorioNoRellenado_muestraError_y_vuelveAPaso5()
        {
            InitialStepsForOfertaHerramientas();

            SelectHerramientasParaOferta_PO.AddHerramientaToOfertaCart(herramientaNombreMakita);
            SelectHerramientasParaOferta_PO.ClickContinuar();

            PostOferta_PO.WaitForPage();

            PostOferta_PO.ClearFechaInicio();

            PostOferta_PO.ClickSubmit();

            if (PostOferta_PO.IsDialogOpen())
            {
                PostOferta_PO.ClickDialogSave();
                Assert.True(PostOferta_PO.WaitForError(10));
                Assert.True(PostOferta_PO.IsOnCreateOferta());
            }
            else
            {
                var err = PostOferta_PO.GetErrorText();
                Assert.True(!string.IsNullOrWhiteSpace(err));
                Assert.True(PostOferta_PO.IsOnCreateOferta());
            }
        }
    }
}
