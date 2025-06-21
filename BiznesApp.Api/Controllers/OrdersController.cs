using BiznesApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiznesApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private static List<Order> _orders = new List<Order>
        {
            new Order { Id = 1, Name = "Zamówienie na komputery", Status = "W realizacji", Amount = 12000 },
            new Order { Id = 2, Name = "Licencje na oprogramowanie", Status = "Zakończone", Amount = 4500 },
            new Order { Id = 3, Name = "Szkolenie dla zespołu", Status = "Nowe", Amount = 8000 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return Ok(_orders);
        }

        [HttpPost]
        public ActionResult<Order> AddOrder([FromBody] Order order)
        {
            order.Id = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;
            _orders.Add(order);
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            order.Name = updatedOrder.Name;
            order.Amount = updatedOrder.Amount;
            order.Status = updatedOrder.Status;
            order.PhotoPath = updatedOrder.PhotoPath;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            _orders.Remove(order);
            return NoContent();
        }
    }
} 