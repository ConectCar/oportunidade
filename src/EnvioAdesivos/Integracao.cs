using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnvioAdesivos
{
    public class Integracao
    {
        public async Task EnviarAdesivo(Guid pedidoId)
        {
            var randomError = new Random().Next(0, 10);
            if(randomError == 0)
            {
                throw new Exception($"Erro ao acionar o serviço externo de envio de adesivos para o pedido {pedidoId}");
            }

            new Logger().Log($"Enviado adesivo do pedido {pedidoId}");

            await Task.Delay(200);
        }
    }
}
