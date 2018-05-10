using System;
using System.Collections.Generic;
using System.Text;

namespace Pedidos
{
    public class Pedido
    {
        internal Pedido() { }

        public Pedido(Guid id, DateTime data, string placa, decimal valor) 
            : this(placa, valor)
        {
            this.Id = id;
            this.Data = data;
        }

        public Pedido(string placa, decimal valor)
        {
            this.Placa = placa;
            this.Valor = valor;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Data { get; set; } = DateTime.Now;
        public string Placa { get; set; }
        public decimal Valor { get; set; }
        public bool EnvioAdesivo { get; set; }
        public bool Cobranca { get; set; }
        public bool Processando { get; set; }
    }
}
