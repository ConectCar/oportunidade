using System;

namespace Pedidos
{
    /// <summary>
    /// Utilize essa classe para fazer o log solicitado no exercício.
    /// Mas não faça alterações, pois utilizaremos esse formato para os nossos testes.
    /// </summary>
    public class Logger
    {
        public void ProcessandoPedido(Guid pedidoId)
        {
            Log($"Processando pedido: {pedidoId}");
        }

        public void PedidoProcessado(Guid pedidoId)
        {
            Log($"Pedido processado: {pedidoId}");
        }

        public void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:g} Pedidos [INFO] {message}");
        }
    }
}
