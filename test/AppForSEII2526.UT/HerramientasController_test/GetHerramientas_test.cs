using AppForMovies.UT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetHerramientas_test : AppForMovies4SqliteUT
    {
        public GetHerramientas_test()
        {
            var herramientas = new List<Herramienta>()
            {
                new Herramienta(10,3,"Hierro","Sierra",52,240,new Fabricante(1,"Teodora",null)),
                new Herramienta(11,5,"Hierro","Martillo",90,800,new Fabricante(2,"Genaro",null)),
                new Herramienta(12,2,"Madera","Pinzas",25,120,new Fabricante(3,"Paco",null))
            };
            
            var fabricante = new List<Fabricante>() {
                new Fabricante(1,"Manolo",herramientas),
                new Fabricante(2,"Felipe",herramientas),
                new Fabricante(3,"Patricio",herramientas),
                new Fabricante(4,"Almudena",herramientas)
            };

            ApplicationUser usuario = new ApplicationUser("Ortiz","gonzalo.ortiz@alu.uclm.es","Gonzalo",666799869,null);

            var comprasDeUsuario = new List<Compra>()
            {
                new Compra("micasa",DateTime.Now,30,35.33,null,usuario),
                new Compra("micasa",DateTime.Now,31,60.25,null,usuario),
                new Compra("micasa",DateTime.Now,32,88.69,null,usuario)
            };

            var comprasItem = new List<CompraItem>()
            {
                new CompraItem(3,"Sierra de Hierro",33,10,156,new Compra("micasa",DateTime.Now,33,40.55,null,usuario),new Herramienta(10,3,"Hierro","Sierra",52,240,new Fabricante(1,"Teodora",null)))
            };
        }
    }
}
