using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Trabalho_DM106.Models
{
    public class Trabalho_DM106Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Trabalho_DM106Context() : base("name=Trabalho_DM106Context")
        {
        }

        public System.Data.Entity.DbSet<Trabalho_DM106.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Trabalho_DM106.Models.Order> Orders { get; set; }
    }
}
