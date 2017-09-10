using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Description;
using Trabalho_DM106.Models;
using Trabalho_DM106.br.com.correios.ws;
using Trabalho_DM106.CRMClient;

namespace Trabalho_DM106.Controllers
{
    [Authorize(Roles = "ADMIN, USER")]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private Trabalho_DM106Context db = new Trabalho_DM106Context();
        
        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("frete")]
        public IHttpActionResult CalculaFrete()
        {
            string frete;

            CalcPrecoPrazoWS correios = new CalcPrecoPrazoWS();

            cResultado resultado = correios.CalcPrecoPrazo("", "", "40010", "37540000", "37002970", "1", 1, 30, 30, 30, 30, "N", 100, "S");

            if (resultado.Servicos[0].Erro.Equals("0"))
            {
                frete = "Valor do frete: " + resultado.Servicos[0].Valor + " - Prazo de entrega: " + resultado.Servicos[0].PrazoEntrega + " dia(s)";
                return Ok(frete);
            }
            else
            {
                return BadRequest("Código do erro: " + resultado.Servicos[0].Erro + "-" + resultado.Servicos[0].MsgErro);
            }
        }

        [ResponseType(typeof(string))]
        [HttpGet]
        [Route("cep")]
        public IHttpActionResult ObtemCEP()
        {
            CRMRestClient crmClient = new CRMRestClient();
            Customer customer = crmClient.GetCustomerByEmail(User.Identity.Name);

            if (customer != null)
            {
                return Ok(customer.zip);
            }
            else
            {
                return BadRequest("Falha ao consultar o CRM");
            }
        }


        // GET: api/Orders
        [Authorize(Roles = "ADMIN")]
        [ResponseType(typeof(List<Order>))]
        public IHttpActionResult GetOrders()
        {
            List<Order> listOrders = new List<Order>();
            listOrders = db.Orders.ToList();

            if (listOrders.Count > 0)
            {
                return Ok(listOrders);
            }
            else
            {
                return BadRequest("Nenhum pedido cadastrado!!");

            }            
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);

            if (User.IsInRole("ADMIN") || User.Identity.Name.Equals(order.emailUser))
            {
                if (order == null)
                {
                    return BadRequest("Não foi encontrado o pedido!");
                }
                return Ok(order);
            }
            else
            {
                return BadRequest("Não foi possível acessar - Permissão Negada!!");
            }
            
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            if (User.IsInRole("ADMIN") || User.Identity.Name.Equals(order.emailUser))
            {
                db.Entry(order).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest("Não foi possível acessar - Permissão Negada!!");
            }
            
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.statusPedido = "novo";
            order.pesoTotal = 0;
            order.precoFrete = 0;
            order.precoTotal = 0;
            order.dataPedido = "09/2017";

            order.emailUser = User.Identity.Name;

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return BadRequest("Não foi encontrado o pedido!");
            }
            if (User.IsInRole("ADMIN") || User.Identity.Name.Equals(order.emailUser))
            {
                db.Orders.Remove(order);
                db.SaveChanges();

                return Ok("Pedido removido com sucesso!");
            }
            else
            {
                return BadRequest("Não foi possível acessar - Permissão Negada!!");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }

    }
}