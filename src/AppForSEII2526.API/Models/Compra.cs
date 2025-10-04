using System;

public class Compra
{
	public Compra()
	{
		public string apellidoCliente{ get;set;}
		
		public string correoElectrónico { get;set;}
		
		public string direcciónEnvío { get;set;}
		
		public string fechaCompra {  get;set;}

		[Key]
		public int id { get;set;}

		public string nombreCliente { get;set;}

		public double precioTotal { get;set;}

		public int teléfono { get;set;}

	}
}
