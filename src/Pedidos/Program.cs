using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Pedidos
{
    class Program
    {
        static void Main(string[] args)
        {
            // SIMULAÇÂO DO CONTROLLER DA API, CHAMANDO OS SERVICOS DE PEDIDOS, COM DI.

            var serviceProvider = new ServiceCollection()
          .AddSingleton<IPedidosService, PedidosService>()
          .BuildServiceProvider();

            var pedidos = new Processador().LerPedidos();
            var pedidos100 = (from p in pedidos select p).Take(100);

            var pedidosService = serviceProvider.GetService<IPedidosService>();
            pedidosService.ProcessarPedidos(pedidos100.ToList());
        }
    }
}
