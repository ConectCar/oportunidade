using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedidos
{
    public interface IPedidosService
    {
        Task ProcessarPedidos(List<Pedido> pedidos);
    }
}
