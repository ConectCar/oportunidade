using System;

namespace Pedidos
{
    class Program
    {
        private static Logger logger = new Logger();

        static void Main(string[] args)
        {
            logger.Log("Iniciando aplicação de pedidos");
            var pedidos = new Processador().LerPedidos();
        }
    }
}
