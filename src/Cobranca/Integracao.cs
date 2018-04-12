using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cobranca
{
    public class Integracao
    {
        public async Task Cobrar(Guid pedidoId)
        {
            var randomError = new Random().Next(0, 10);
            if(randomError == 0)
            {
                throw new Exception($"Erro ao acionar o gateway de pagamento para o pedido {pedidoId}");
            }

            new Logger().Log($"Pedido {pedidoId} cobrado");

            await Task.Delay(200);
        }
    }
}
