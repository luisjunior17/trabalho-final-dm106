using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trabalho_DM106.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Foreign Key
        public int ProductId { get; set; }

        // Navigation property
        public virtual Product Product { get; set; }

        public int quantidade;

        public int OrderId { get; set; }

    }
}