using System;
using System.Collections.Generic;
using System.Text;

namespace Pedidos
{
    public class PedidosDelegates
    {
        public delegate void PedidoProcessadoHandler(Guid pedidoId);

        public delegate void AdesivoEnviadoHandler(Guid pedidoId);

        public delegate void CobrancaRealizadaHandler(Guid pedidoId);
    }
}
