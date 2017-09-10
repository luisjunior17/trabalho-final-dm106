using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trabalho_DM106.Models
{
    public class Order
    {

        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public string emailUser { get; set; }

        public string dataPedido { get; set; }

        public string dataEntrega { get; set; }

        public string statusPedido { get; set; }

        public decimal precoTotal { get; set; }

        public decimal pesoTotal { get; set; }

        public decimal precoFrete { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}