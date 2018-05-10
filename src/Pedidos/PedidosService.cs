using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Pedidos.PedidosDelegates;

namespace Pedidos
{
    public class PedidosService : IPedidosService
    {
        /// <summary>
        /// Instacia API para logar aplicação
        /// </summary>
        private Logger logger = new Logger();

        /// <summary>
        /// Instacia API para Enviar Adesivos
        /// </summary>
        private EnvioAdesivos.Integracao enviaAdesivos = new EnvioAdesivos.Integracao();

        /// <summary>
        /// Instacia API para Cobranca
        /// </summary>
        private Cobranca.Integracao cobranca = new Cobranca.Integracao();

        /// <summary>
        /// Evento para possiveis implementações após o processamento do pedido
        /// </summary>
        private PedidoProcessadoHandler pedidoProcessadoHandler = new PedidoProcessadoHandler(PedidoProcessado);

        /// <summary>
        /// Evento para possiveis implementações após o envio do adesivo
        /// </summary>
        private AdesivoEnviadoHandler adesivoEnviadoHandler = new AdesivoEnviadoHandler(AdesivoEnviado);

        /// <summary>
        /// Evento para possiveis implementações após o envio do adesivo
        /// </summary>
        private CobrancaRealizadaHandler cobrancaRealizadaHandler = new CobrancaRealizadaHandler(CobrancaRealizada);

        /// <summary>
        /// Objeto para tratar concorrencia safe-thread
        /// </summary>
        private object lockPedido = new object();

        /// <summary>
        /// Método que é chamado após o processamento do pedido, para realizar implementações futuras como rastreio, notificação por email, etc.
        /// </summary>
        /// <param name="pedidoId"></param>
        public static void PedidoProcessado(Guid pedidoId)
        {

        }

        /// <summary>
        /// Método que é chamado após o processamento do envio do adesivo
        /// </summary>
        /// <param name="pedidoId"></param>
        public static void AdesivoEnviado(Guid pedidoId)
        {

        }

        /// <summary>
        /// Método que é chamado após a cobranca realizada
        /// </summary>
        /// <param name="pedidoId"></param>
        public static void CobrancaRealizada(Guid pedidoId)
        {

        }

        /// <summary>
        /// Método que será exposto na API, para processar um ou mais pedidos, orquestra o envio de adesivos e cobrança,
        /// somente após a conclusão de ambos o pedido é dado como processado
        /// </summary>
        /// <param name="pedidos">Lista com objeto Pedido</param>
        /// <param name="tipoProcessamento"></param>
        /// <returns></returns>
        public async Task ProcessarPedidos(List<Pedido> pedidos)
        {
            logger.Log("Iniciando aplicação de pedidos");

            try
            {
                Task taskEnviarAdesivos = Task.Factory.StartNew(() =>
                {
                    Thread.CurrentThread.Name = "EnviarAdesivos";
                    ProcessarPedidos(pedidos, TIPO_PROCESSAMENTO.ENVIAR_ADESIVO).GetAwaiter().GetResult();
                });

                Task taskCobranca = Task.Factory.StartNew(() =>
                {
                    Thread.CurrentThread.Name = "Cobranca";
                    ProcessarPedidos(pedidos, TIPO_PROCESSAMENTO.COBRANCA).GetAwaiter().GetResult();
                });

                Task.WaitAll(taskEnviarAdesivos, taskCobranca);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Método interno que processa os pedidos
        /// </summary>
        /// <param name="pedidos">Lista com objeto Pedido</param>
        /// <param name="tipoProcessamento"></param>
        /// <returns></returns>
        private async Task ProcessarPedidos(List<Pedido> pedidos, TIPO_PROCESSAMENTO tipoProcessamento)
        {
            List<Pedido> pedidosComErro = new List<Pedido>();

            foreach (var pedido in pedidos)
            {
                lock (lockPedido)
                {
                    if (!pedido.Processando)
                    {
                        logger.ProcessandoPedido(pedido.Id);
                        pedido.Processando = true;
                    }

                    if (pedido.EnvioAdesivo && pedido.Cobranca)
                        continue;
                }

                try
                {
                    if (tipoProcessamento.Equals(TIPO_PROCESSAMENTO.ENVIAR_ADESIVO))
                    {
                        await enviaAdesivos.EnviarAdesivo(pedido.Id);
                        lock (lockPedido)
                        {
                            pedido.EnvioAdesivo = true;
                            adesivoEnviadoHandler(pedido.Id);
                        }
                    }
                    else if (tipoProcessamento.Equals(TIPO_PROCESSAMENTO.COBRANCA))
                    {
                        await cobranca.Cobrar(pedido.Id);
                        lock (lockPedido)
                        {
                            pedido.Cobranca = true;
                            cobrancaRealizadaHandler(pedido.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Log(ex.Message);
                    pedidosComErro.Add(pedido);
                }

                lock (lockPedido)
                {
                    if (pedido.EnvioAdesivo && pedido.Cobranca)
                    {
                        logger.PedidoProcessado(pedido.Id);
                        pedidoProcessadoHandler(pedido.Id);
                    }
                }
            }

            // Se existir erros de processamento esperar 5 segundos e processar novamente
            if (pedidosComErro.Count > 0)
            {
                await Task.Delay(5000);
                await ProcessarPedidos(pedidosComErro, tipoProcessamento);
            }
        }
    }
}

