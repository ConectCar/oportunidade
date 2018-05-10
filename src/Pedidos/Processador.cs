using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pedidos
{
    public class Processador
    {
        private const string filename = @"./pedidos.json";   

        public List<Pedido> LerPedidos()
        {
            return JsonConvert.DeserializeObject<List<Pedido>>(File.ReadAllText(filename));
        }

        public void GerarPedidosAleatorios()
        {
            var pedidos = new List<Pedido>();

            for (int i = 0; i < 5000; i++)
            {
                pedidos.Add(new Pedido(GeraPlacaAleatoria(), 19.9M));
            }

            File.WriteAllText(filename, JsonConvert.SerializeObject(pedidos));
        }

        private string GeraPlacaAleatoria()
        {
            var random = new Random();
            var letras = $"{(char)random.Next(65, 91)}{(char)random.Next(65, 91)}{(char)random.Next(65, 91)}";
            var numeros = random.Next(1, 10000);

            return $"{letras}-{numeros}";
        }       
    }
}