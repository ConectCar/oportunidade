using System;
using System.Linq;
using Xunit;

namespace Pedidos.Tests
{
    public class PedidosTests
    {
        public PedidosService PedidosService()
        {
            return new PedidosService();
        }

        public Processador Processador()
        {
            return new Processador();
        }

        [Fact]
        public async void ProcessarPedido_ValidarSucesso()
        {
            try
            {
                var pedidosService = PedidosService();
                var processador = Processador();

                var pedidos = processador.LerPedidos();
                var pedidosTop1 = (from p in pedidos select p).Take(1);

                await pedidosService.ProcessarPedidos(pedidosTop1.ToList());

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }
    }
}
